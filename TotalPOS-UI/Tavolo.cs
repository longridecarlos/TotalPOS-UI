using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTable.Models;

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

            // Show the name of the table in the label
            lbTitulo.Text = SelectedTable.Name;

            // Load the products of the table from the database
            LoadProductsInTable();
        }

        private void LoadProductsInTable()
        {
            // Clean te ListView before load the products
            listViewTavolos.Items.Clear();

            // Get the products from the database of the selected table
            productsInTable = OpenTable.Database.GetTableProducts(SelectedTable.Id);

            // Load products in the ListView
            foreach (var product in productsInTable)
            {
                var item = new ListViewItem(product.Quantity.ToString());
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Price.ToString("F2"));
                item.SubItems.Add(product.GetTotal().ToString("F2"));
                //item.Tag=product;

                item.Checked = false; // Initially unchecked
                listViewTavolos.Items.Add(item);
            }

            // Insert the empty row only if there are products
            if (productsInTable.Count > 0)
            {
                var emptyRow = new ListViewItem("");
                listViewTavolos.Items.Add(emptyRow);
            }

            // Update the total in the listView
            UpdateTotalRow();
        }

        private void UpdateLv()
        {
            // Clean the ListView and reaload the products
            listViewTavolos.Items.Clear();

            foreach (var product in productsInTable)
            {
                var item = new ListViewItem(product.Quantity.ToString());
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Price.ToString("F2"));
                item.SubItems.Add(product.GetTotal().ToString("F2"));

                item.Checked = false; // Initially unchecked
                listViewTavolos.Items.Add(item);
            }

            // Insert an empty row between products and total if there are products
            if (productsInTable.Count > 0)
            {
                var emptyRow = new ListViewItem("");
                listViewTavolos.Items.Add(emptyRow);
            }

            // Update the total general in the List View
            UpdateTotalRow();
        }

        private void UpdateTotalRow()
        {
            // Calculate the total general of the products
            double total = productsInTable.Sum(p => p.GetTotal());

            // Check if a total row exists
            var totalRow = listViewTavolos.Items.Cast<ListViewItem>()
                .FirstOrDefault(item => item.SubItems[0].Text == "Total");

            if (totalRow != null)
            {
                // If exists, update the total
                totalRow.SubItems[3].Text = total.ToString("F2");
            }
            else
            {
                // If not exists, add a new row
                var totalItem = new ListViewItem("Total");
                totalItem.SubItems.Add(""); // Let the column of quantity empty
                totalItem.SubItems.Add(""); // Let the column of price empty
                totalItem.SubItems.Add(total.ToString("F2")); // Total general
                listViewTavolos.Items.Add(totalItem);
            }
        }

        private void btnCocaCola_Click(object sender, EventArgs e)
        {
            string productName = "Coca Cola";
            double productPrice = 4.00;

            // Search if there is a Coca Cola in the list
            var existingProduct = productsInTable.FirstOrDefault(p => p.Name == productName);
            if (existingProduct != null)
            {
                // If it's already exists increase one
                existingProduct.Quantity += 1;
            }
            else
            {
                // If doesn't exists, we add it
                productsInTable.Add(new Product { Name = productName, Price = productPrice, Quantity = 1 });
            }

            // Update the ListView
            UpdateLv();

            try
            {
                // Current products are removed from the database first
                OpenTable.Database.DeleteTableProducts(SelectedTable.Id);

                // Then we add each product to the database
                foreach (var product in productsInTable)
                {
                    OpenTable.Database.AddItemToTable(SelectedTable.Id, product, product.Quantity);
                }

                // Calculate the total of the table
                double total = productsInTable.Sum(p => p.GetTotal());

                int numberOfProducts = 0;
                foreach (ListViewItem item in listViewTavolos.Items)
                {
                    if (int.TryParse(item.SubItems[0].Text, out int value)) // Convert to number
                    {
                        numberOfProducts += value;
                    }
                }

                // Save the total
                OpenTable.Database.UpdateTableTotal(SelectedTable.Id, total);
                OpenTable.Database.UpdateTableTotalArt(SelectedTable.Id, numberOfProducts);

            }
            catch (Exception ex)
            {
                // If there is any error, display the error message
                MessageBox.Show($"Errore durante il salvataggio dell'ordine: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnPizza_Click(object sender, EventArgs e)
        {
            string productName = "Pizza";
            double productPrice = 15.00;

            // Search if a Pizza already exists in the list
            var existingProduct = productsInTable.FirstOrDefault(p => p.Name == productName);
            if (existingProduct != null)
            {
                // If it already exists, increase the amount
                existingProduct.Quantity += 1;
            }
            else
            {
                // If it doesn't exist, we add it
                productsInTable.Add(new Product { Name = productName, Price = productPrice, Quantity = 1 });
            }

            // Update the ListView
            UpdateLv();

            try
            {
                // Current products are first deleted from the database.
                OpenTable.Database.DeleteTableProducts(SelectedTable.Id);

                // Then we add each one to the database
                foreach (var product in productsInTable)
                {
                    OpenTable.Database.AddItemToTable(SelectedTable.Id, product, product.Quantity);
                }

                // Calculate the total of the table
                double total = productsInTable.Sum(p => p.GetTotal());

                int numberOfProducts = 0;
                foreach (ListViewItem item in listViewTavolos.Items)
                {
                    if (int.TryParse(item.SubItems[0].Text, out int value)) // Try to convert to number
                    {
                        numberOfProducts += value; // Add if it is a valid number
                    }
                }

                // Save total
                OpenTable.Database.UpdateTableTotal(SelectedTable.Id, total);
                OpenTable.Database.UpdateTableTotalArt(SelectedTable.Id, numberOfProducts);


            }
            catch (Exception ex)
            {
                // If there is any error, display the error message
                MessageBox.Show($"Errore durante il salvataggio dell'ordine: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public delegate void TableRemovedEventHandler(int tableId);
        public event TableRemovedEventHandler TableRemoved;

        private void btnPagare_Click(object sender, EventArgs e)
        {
            double totalToPay = 0;
            bool anyProductSelected = false;

            // We check if the total or a product has been marked
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
                MessageBox.Show("Non hai selezionato nessun prodotto da pagare.", "Errore di pagamento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (totalToPay == 0.00)
            {
                MessageBox.Show("Il conteggio non può essere zero.", "Errore di pagamento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"¿Sei sicuro di voler pagare un totale di CHF {totalToPay:F2}?", "Conferma il pagamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Delete products from the database and product list
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
                        //OpenTable.Database.DeleteTable(SelectedTable.Id); // Delete table
                        OpenTable.Database.UpdateIsOpen(SelectedTable.Id);
                    }

                    UpdateLv();

                    if (productsInTable.Count == 0)
                    {
                        // Notify the Main form that the table has been deleted
                        TableRemoved?.Invoke(SelectedTable.Id);

                        this.Close();
                    }

                    MessageBox.Show("Il pagamento è stato effettuato correttamente.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante l'elaborazione del pagamento: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pagamento annullato.", "Annullato", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSalvare_Click(object sender, EventArgs e)
        {
            /*try
            {
                // Current products are first deleted from the database.
                OpenTable.Database.DeleteTableProducts(SelectedTable.Id);

                // Then we add each one to the database
                foreach (var product in productsInTable)
                {
                    OpenTable.Database.AddItemToTable(SelectedTable.Id, product, product.Quantity);
                }

                // Calculate the total of the table
                double total = productsInTable.Sum(p => p.GetTotal());

                int numberOfProducts = 0;
                foreach (ListViewItem item in listViewTavolos.Items)
                {
                    if (int.TryParse(item.SubItems[0].Text, out int value)) // Try to convert to number
                    {
                        numberOfProducts += value; // Add if it is a valid number
                    }
                }


                // Save total
                OpenTable.Database.UpdateTableTotal(SelectedTable.Id, total);
                OpenTable.Database.UpdateTableTotalArt(SelectedTable.Id, numberOfProducts);


                // Message of all good
                MessageBox.Show("Ordine salvato con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Close
                this.Close();
            }
            catch (Exception ex)
            {
                // If there is any error, display the error message
                MessageBox.Show($"Errore durante il salvataggio dell'ordine: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void btnRimuovi_Click(object sender, EventArgs e)
        {
            // Check if a product has been selected in the ListView
            if (listViewTavolos.SelectedItems.Count > 0)
            {
                // Get the selected item
                var selectedItem = listViewTavolos.SelectedItems[0];

                // Get the name of the product
                string productName = selectedItem.SubItems[1].Text;

                // Get the product name
                var productToRemove = productsInTable.FirstOrDefault(p => p.Name == productName);

                if (productToRemove != null)
                {
                    // If the product has more than one unit, decrease the quantity
                    if (productToRemove.Quantity > 1)
                    {
                        productToRemove.Quantity -= 1;
                    }
                    else
                    {
                        // If you only have one unit, remove the product from the list
                        productsInTable.Remove(productToRemove);
                    }

                    // Update the ListView
                    UpdateLv();
                    try
                    {
                        // Current products are removed from the database first
                        OpenTable.Database.DeleteTableProducts(SelectedTable.Id);

                        // Then we add each product to the database
                        foreach (var product in productsInTable)
                        {
                            OpenTable.Database.AddItemToTable(SelectedTable.Id, product, product.Quantity);
                        }

                        // Calculate the total of the table
                        double total = productsInTable.Sum(p => p.GetTotal());

                        int numberOfProducts = 0;
                        foreach (ListViewItem item in listViewTavolos.Items)
                        {
                            if (int.TryParse(item.SubItems[0].Text, out int value)) // Convert to number
                            {
                                numberOfProducts += value;
                            }
                        }

                        // Save the total
                        OpenTable.Database.UpdateTableTotal(SelectedTable.Id, total);
                        OpenTable.Database.UpdateTableTotalArt(SelectedTable.Id, numberOfProducts);

                    }
                    catch (Exception ex)
                    {
                        // If there is any error, display the error message
                        MessageBox.Show($"Errore durante il salvataggio dell'ordine: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // If no item has been selected, display the alert message
                MessageBox.Show("Seleziona un prodotto da eliminare.", "Avvertimento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
