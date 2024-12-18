using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTable.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TotalPOS_UI
{
    public partial class Tavolo : Form
    {
        public Table SelectedTable { get; set; }
        private List<Product> productsInTable = new List<Product>();

        public Tavolo()
        {
            InitializeComponent();
        }

        public Tavolo(Table selectedTable) : this()
        {
            SelectedTable = selectedTable;

            // Mostrar el nombre de la mesa en lbTitulo
            lbTitulo.Text = SelectedTable.Name;

            // Cargar los productos de la mesa desde la base de datos
            LoadProductsInTable();
        }

        private void LoadProductsInTable()
        {
            // Limpiar el ListView antes de cargar los productos
            listViewTavolos.Items.Clear();

            // Obtener los productos de la base de datos para la mesa seleccionada
            productsInTable = OpenTable.Database.GetTableProducts(SelectedTable.Id);

            // Cargar productos en el ListView
            foreach (var product in productsInTable)
            {
                var item = new ListViewItem(product.Quantity.ToString());
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Price.ToString("F2"));
                item.SubItems.Add(product.GetTotal().ToString("F2"));
                item.SubItems.Add(""); // Columna para checkbox

                item.Checked = false; // Inicialmente desmarcado
                listViewTavolos.Items.Add(item);
            }

            // Insertar la fila vacía solo si hay productos
            if (productsInTable.Count > 0)
            {
                var emptyRow = new ListViewItem("");
                listViewTavolos.Items.Add(emptyRow);
            }

            // Actualizar el total general en el ListView
            UpdateTotalRow();
        }

        private void UpdateLv()
        {
            // Limpiar el ListView y recargar los productos
            listViewTavolos.Items.Clear();

            foreach (var product in productsInTable)
            {
                var item = new ListViewItem(product.Quantity.ToString());
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Price.ToString("F2"));
                item.SubItems.Add(product.GetTotal().ToString("F2"));
                item.SubItems.Add(""); // Columna para checkbox

                item.Checked = false; // Inicialmente desmarcado
                listViewTavolos.Items.Add(item);
            }

            // Insertar una fila vacía entre productos y total solo si hay productos
            if (productsInTable.Count > 0)
            {
                var emptyRow = new ListViewItem("");
                listViewTavolos.Items.Add(emptyRow);
            }

            // Actualizar el total general en el ListView
            UpdateTotalRow();
        }

        private void UpdateTotalRow()
        {
            // Calcular el total general de todos los productos
            double total = productsInTable.Sum(p => p.GetTotal());

            // Revisar si ya existe una fila de total en el ListView
            var totalRow = listViewTavolos.Items.Cast<ListViewItem>()
                .FirstOrDefault(item => item.SubItems[0].Text == "Total");

            if (totalRow != null)
            {
                // Si ya existe, actualizar el total
                totalRow.SubItems[3].Text = total.ToString("F2");
            }
            else
            {
                // Si no existe, agregar una nueva fila de total
                var totalItem = new ListViewItem("Total");
                totalItem.SubItems.Add(""); // Dejar la columna de cantidad vacía
                totalItem.SubItems.Add(""); // Dejar la columna de precio vacía
                totalItem.SubItems.Add(total.ToString("F2")); // Total general
                listViewTavolos.Items.Add(totalItem);
            }
        }

        private void btnCocaCola_Click(object sender, EventArgs e)
        {
            string productName = "Coca Cola";
            double productPrice = 4.00;

            // Buscar si ya existe una Coca Cola en la lista
            var existingProduct = productsInTable.FirstOrDefault(p => p.Name == productName);
            if (existingProduct != null)
            {
                // Si ya existe, incrementar la cantidad
                existingProduct.Quantity += 1;
            }
            else
            {
                // Si no existe, lo agregamos
                productsInTable.Add(new Product { Name = productName, Price = productPrice, Quantity = 1 });
            }

            // Actualizar el ListView
            UpdateLv();
        }

        private void btnPizza_Click(object sender, EventArgs e)
        {
            string productName = "Pizza";
            double productPrice = 15.00;

            // Buscar si ya existe una Pizza en la lista
            var existingProduct = productsInTable.FirstOrDefault(p => p.Name == productName);
            if (existingProduct != null)
            {
                // Si ya existe, incrementar la cantidad
                existingProduct.Quantity += 1;
            }
            else
            {
                // Si no existe, lo agregamos
                productsInTable.Add(new Product { Name = productName, Price = productPrice, Quantity = 1 });
            }

            // Actualizar el ListView
            UpdateLv();
        }

        public delegate void TableRemovedEventHandler(int tableId);
        public event TableRemovedEventHandler TableRemoved;

        private void btnPagare_Click(object sender, EventArgs e)
        {
            double totalToPay = 0;
            bool anyProductSelected = false;

            // Verificamos si se ha marcado el total o un producto
            foreach (ListViewItem item in listViewTavolos.Items)
            {
                if (item.Checked && item.SubItems[0].Text == "Total")
                {
                    totalToPay = productsInTable.Sum(p => p.GetTotal());
                    anyProductSelected = true;
                    break;
                }
                else if (item.Checked && item.SubItems[0].Text != "Total")
                {
                    string productName = item.SubItems[1].Text;
                    var product = productsInTable.FirstOrDefault(p => p.Name == productName);
                    if (product != null)
                    {
                        totalToPay += product.GetTotal();
                        anyProductSelected = true;
                    }
                }
            }

            if (!anyProductSelected)
            {
                MessageBox.Show("No has seleccionado ningún producto para pagar.", "Error de pago", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (totalToPay == 0.00)
            {
                MessageBox.Show("La cuenta no puede ser cero.", "Error de pago", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"¿Estás seguro que deseas pagar un total de CHF {totalToPay:F2}?", "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Eliminar los productos de la base de datos y de la lista de productos
                    foreach (ListViewItem item in listViewTavolos.Items)
                    {
                        if (item.Checked && item.SubItems[0].Text != "Total")
                        {
                            string productName = item.SubItems[1].Text;
                            var productToRemove = productsInTable.FirstOrDefault(p => p.Name == productName);
                            if (productToRemove != null)
                            {
                                OpenTable.Database.DeleteProduct(productToRemove.Id);
                                productsInTable.Remove(productToRemove);
                            }
                        }
                    }

                    if (listViewTavolos.Items.Cast<ListViewItem>().Any(i => i.SubItems[0].Text == "Total" && i.Checked))
                    {
                        foreach (var product in productsInTable)
                        {
                            OpenTable.Database.DeleteProduct(product.Id);
                        }
                        productsInTable.Clear();
                        OpenTable.Database.DeleteTable(SelectedTable.Id); // Eliminar la mesa
                    }

                    UpdateLv();

                    if (productsInTable.Count == 0)
                    {
                        MessageBox.Show("La mesa ha sido eliminada porque no quedan productos.", "Mesa eliminada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Notificar al formulario Main que la mesa ha sido eliminada
                        TableRemoved?.Invoke(SelectedTable.Id);

                        this.Close();
                    }

                    MessageBox.Show("El pago se ha realizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al procesar el pago: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pago cancelado.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSalvare_Click(object sender, EventArgs e)
        {
            try
            {
                // Primero se borran los productos actuales de la base de datos
                OpenTable.Database.DeleteTableProducts(SelectedTable.Id);

                // Luego, agregamos cada uno a la base de datos
                foreach (var product in productsInTable)
                {
                    OpenTable.Database.AddItemToTable(SelectedTable.Id, product, product.Quantity);
                }

                // Calcular el total de la mesa
                double total = productsInTable.Sum(p => p.GetTotal());

                int numberOfProducts = 0;
                foreach (ListViewItem item in listViewTavolos.Items)
                {
                    if (int.TryParse(item.SubItems[0].Text, out int value)) // Intenta convertir a número
                    {
                        numberOfProducts += value; // Sumar si es un número válido
                    }
                }


                //Guardar total
                OpenTable.Database.UpdateTableTotal(SelectedTable.Id, total);
                MessageBox.Show(total.ToString());
                OpenTable.Database.UpdateTableItems(SelectedTable.Id, numberOfProducts);


                // Mensaje de todo bien
                MessageBox.Show("Ordine salvato con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Cerrar
                this.Close();
            }
            catch (Exception ex)
            {
                // Si hay algún error, mostrar el mensaje de error
                MessageBox.Show($"Error al guardar el pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRimuovi_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un producto en el ListView
            if (listViewTavolos.SelectedItems.Count > 0)
            {
                // Obtener el item seleccionado
                var selectedItem = listViewTavolos.SelectedItems[0];

                // Obtener el nombre del producto
                string productName = selectedItem.SubItems[1].Text;

                // Buscar el producto en la lista de productos de la mesa
                var productToRemove = productsInTable.FirstOrDefault(p => p.Name == productName);

                if (productToRemove != null)
                {
                    // Si el producto tiene más de una unidad, disminuir la cantidad
                    if (productToRemove.Quantity > 1)
                    {
                        productToRemove.Quantity -= 1;
                    }
                    else
                    {
                        // Si solo tiene una unidad, eliminar el producto de la lista
                        productsInTable.Remove(productToRemove);
                    }

                    // Actualizar el ListView
                    UpdateLv();
                }
            }
        }
    }
}
