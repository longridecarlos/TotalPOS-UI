using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Newtonsoft.Json;
using OpenTable.Models;

namespace OpenTable
{
    public static class Database
    {
        private static readonly string ConnectionString = "Data Source=database.db;Version=3;";

        public static void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string createTablesQuery = @"
                CREATE TABLE IF NOT EXISTS sync_opentable (
                    ot_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ot_name TEXT NOT NULL,
                    ot_created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                    ot_total REAL DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS sync_opentable_items (
                    oti_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    oti_table_id INTEGER NOT NULL,
                    oti_created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                    oti_product_id INTEGER NOT NULL,
                    oti_qty INTEGER NOT NULL,
                    oti_amount REAL NOT NULL,
                    oti_total REAL GENERATED ALWAYS AS (oti_qty * oti_amount) VIRTUAL,
                    oti_serialized TEXT NOT NULL,
                    FOREIGN KEY (oti_table_id) REFERENCES sync_opentable (ot_id)
                );
            ";

                using (SQLiteCommand command = new SQLiteCommand(createTablesQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Table> GetAllTables()
        {
            List<Table> tables = new List<Table>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ot_id, ot_name, ot_created_at, ot_total, (SELECT SUM(oti_qty) FROM sync_opentable_items WHERE oti_table_id = ot_id) AS TotalArt FROM sync_opentable;";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new Table
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                CreatedAt = reader.GetDateTime(2),
                                Total = reader.GetDouble(3),
                                TotalArt = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return tables;
        }

        public static double prueba(int id)
        {
            double total = 0.0;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ot_total FROM sync_opentable WHERE ot_id=@id;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Usamos el parámetro correcto para evitar inyecciones SQL
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Si hay resultados, leemos el valor de 'ot_total'
                        if (reader.Read())
                        {
                            // Si 'ot_total' es NULL, devolvemos 0.0
                            total = reader.IsDBNull(reader.GetOrdinal("ot_total")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("ot_total"));
                        }
                    }
                }
            }

            return total;
        }


        public static void SaveNewTable(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO sync_opentable (ot_name) VALUES (@name)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", tableName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> GetTableProducts(int tableId)
        {
            List<Product> products = new List<Product>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT p.oti_id, p.oti_product_id, p.oti_qty, p.oti_amount, p.oti_total, p.oti_serialized " +
                               "FROM sync_opentable_items p " +
                               "WHERE p.oti_table_id = @tableId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tableId", tableId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string serializedProduct = reader.GetString(5);
                            Product product = JsonConvert.DeserializeObject<Product>(serializedProduct);
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public static void DeleteTable(int tableId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Eliminar los productos asociados a la mesa
                string deleteItemsQuery = "DELETE FROM sync_opentable_items WHERE oti_table_id = @id";
                using (SQLiteCommand command = new SQLiteCommand(deleteItemsQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", tableId);
                    command.ExecuteNonQuery();
                }

                // Eliminar la mesa
                string deleteTableQuery = "DELETE FROM sync_opentable WHERE ot_id = @id";
                using (SQLiteCommand command = new SQLiteCommand(deleteTableQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", tableId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteProduct(int productId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Eliminar los elementos asociados al producto
                string deleteItemsQuery = "DELETE FROM sync_opentable_items WHERE oti_product_id = @id";
                using (SQLiteCommand command = new SQLiteCommand(deleteItemsQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", productId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddItemToTable(int tableId, Product product, int quantity)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO sync_opentable_items (oti_table_id, oti_product_id, oti_qty, oti_amount, oti_serialized) VALUES (@tableId, @productId, @qty, @amount, @serialized)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tableId", tableId);
                    command.Parameters.AddWithValue("@productId", product.Id);
                    command.Parameters.AddWithValue("@qty", quantity);
                    command.Parameters.AddWithValue("@amount", product.Price);
                    command.Parameters.AddWithValue("@serialized", product.Serialize());
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProductQuantity(int tableId, int productId, int quantity)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string updateQuery = @"UPDATE sync_opentable_items SET oti_qty = @quantity WHERE oti_table_id = @tableId AND oti_product_id = @productId";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@tableId", tableId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteTableProducts(int tableId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string deleteItemsQuery = "DELETE FROM sync_opentable_items WHERE oti_table_id = @tableId";
                using (SQLiteCommand command = new SQLiteCommand(deleteItemsQuery, connection))
                {
                    command.Parameters.AddWithValue("@tableId", tableId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateTableTotal(int tableId, double total)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE sync_opentable SET ot_total = @total WHERE ot_id = @tableId";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@total", total);
                    command.Parameters.AddWithValue("@tableId", tableId);
                    command.ExecuteNonQuery();
                }
            }
        }

        /*public static double GetTableTotal(int tableId)
        {
            double total = 0.0;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT ot_total FROM sync_opentable WHERE ot_id = @tableId";

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@tableId", tableId);

                    // Ejecutar el comando y leer el valor
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Si existe el registro
                        {
                            total = reader.IsDBNull(reader.GetOrdinal("ot_total")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("ot_total"));
                        }
                    }
                }
            }

            return total;
        }*/

        /*public static int GetTableItems(int tableId)
        {
            int totalItems = 0;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT SUM(oti_qty) FROM sync_opentable_items WHERE oti_table_id = @tableId";

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@tableId", tableId);

                    totalItems = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return totalItems;
        }*/

        public static void UpdateTableItems(int tableId, int totalItems)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE sync_opentable SET ot_total = @totalItems WHERE ot_id = @tableId";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@totalItems", totalItems);
                    command.Parameters.AddWithValue("@tableId", tableId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
