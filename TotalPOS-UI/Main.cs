using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTable.Models;

namespace TotalPOS_UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadTables();  // Load tables on startup
        }

        private void btnApri_Click(object sender, EventArgs e)
        {
            // Check if a table has been selected
            if (listViewTavolo.SelectedItems.Count > 0)
            {
                // Get the ID of the selected table
                int tableId = int.Parse(listViewTavolo.SelectedItems[0].SubItems[0].Text);

                // Find the table in the list of tables loaded from the database
                Table selectedTable = OpenTable.Database.GetAllTables().FirstOrDefault(t => t.Id == tableId);

                if (selectedTable != null)
                {
                    // Open the Tavolo form and move the selected table
                    Tavolo tavoloForm = new Tavolo(selectedTable);  // Here you pass the selected table to the Tavolo form

                    // Subscribe to the FormClosed event to update the ListView when the form is closed
                    tavoloForm.FormClosed += tavoloForm_Closed;  // Here you assign the event correctly

                    tavoloForm.ShowDialog(); // Display the form as modal
                }
                else
                {
                    MessageBox.Show("La tabella selezionata non è stata trovata.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleziona una tabella da aprire.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tavoloForm_Closed(object sender, FormClosedEventArgs e)
        {
            // Call method to update ListView when Tavolo.cs is closed
            UpdateListView();
        }

        private void UpdateListView()
        {
            // Limpia el ListView antes de agregar nuevos elementos
            listViewTavolo.Items.Clear();
            // Call the method to load the tables again from the database
            LoadTables();
            listViewTavolo.Invalidate();  // Forzar la actualización visual
            listViewTavolo.Refresh();     // Refrescar el control
        }

        // Method to load tables from the database and display them in the ListView
        public void LoadTables()
        {
            // Get all tables from the database
            List<Table> tables = OpenTable.Database.GetAllTables();

            // Clear the ListView before loading new data
            listViewTavolo.Items.Clear();

            foreach (var table in tables)
            {
                // Create a new item for the ListView with the information from the table
                ListViewItem item = new ListViewItem(new[]
                {
                    table.Id.ToString(),
                    table.Name,
                    table.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    "CHF    "+table.Total.ToString("F2"),
                    table.TotalArt.ToString(),
                    table.IsOpen.ToString(),
                });

                // Add the item to the ListView
                listViewTavolo.Items.Add(item);
            }
        }


        private void btnNuovo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                MessageBox.Show("Riempi la casella di testo con un codice a barre", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Get all tables from the database
                List<Table> tables = OpenTable.Database.GetAllTables();

                // Check if the code already exists in the list of tables
                string inputCode = txtCode.Text.Trim();
                bool codeExists = false;

                foreach (Table table in tables)
                {
                    if (string.Equals(table.Code, inputCode, StringComparison.OrdinalIgnoreCase))
                    {
                        codeExists = true;
                        break; // Exit the loop as soon as we find a match
                    }
                }

                if (codeExists)
                {
                    MessageBox.Show("Il codice a barre inserito è già associato a un altro tavolo.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Stop the method execution if the code already exists
                }

                // Get all existing table numbers
                var existingNumbers = tables
                    .Select(t => int.TryParse(t.Name.Split(' ').Last(), out int number) ? number : -1)
                    .Where(number => number != -1)
                    .OrderBy(number => number)
                    .ToList();

                // Find the first available number (the number missing in the sequence)
                int newTableNumber = 1; // Start from 1

                // Find the first missing number in the sequence
                foreach (var number in existingNumbers)
                {
                    if (number != newTableNumber)
                    {
                        break; // The free number found is the first missing one
                    }
                    newTableNumber++;
                }

                // Create a new table with the found number
                string tableName = "Tavolo " + newTableNumber;

                // Save the new table in the database
                OpenTable.Database.SaveNewTable(tableName, inputCode);

                // Update the ListView
                LoadTables();
            }
        }



        private void btnCancellare_Click(object sender, EventArgs e)
        {
            // Check if a table has been selected
            if (listViewTavolo.SelectedItems.Count > 0)
            {
                // Get the ID of the selected table
                int tableId = int.Parse(listViewTavolo.SelectedItems[0].SubItems[0].Text);

                // Delete table from database
                OpenTable.Database.DeleteTable(tableId);

                // Update the ListView to see changes
                LoadTables();
            }
            else
            {
                MessageBox.Show("Seleziona una tabella da eliminare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /*private void txtSearch_TextChanged(string code)
        {

            // Get table from the database
            Table table = OpenTable.Database.GetTable(code);

            if (table != null)
            {
                // Open the Tavolo form and move the selected table
                Tavolo tavoloForm = new Tavolo(table);  // Here you pass the selected table to the Tavolo form

                // Subscribe to the FormClosed event to update the ListView when the form is closed
                tavoloForm.FormClosed += tavoloForm_Closed;  // Here you assign the event correctly

                tavoloForm.ShowDialog(); // Display the form as modal
            }
            else
            {
                MessageBox.Show("La tua tabella non è stata trovata", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Get all tables from the database
            /*List<Table> tables = OpenTable.Database.GetAllTables();

            

            foreach (Table table in tables)
            {
                if(code == table.Code)
                {
                    // Open the Tavolo form and move the selected table
                    Tavolo tavoloForm = new Tavolo(table);  // Here you pass the selected table to the Tavolo form

                    // Subscribe to the FormClosed event to update the ListView when the form is closed
                    tavoloForm.FormClosed += tavoloForm_Closed;  // Here you assign the event correctly

                    tavoloForm.ShowDialog(); // Display the form as modal
                }
                else
                {
                    MessageBox.Show("La tua tabella non è stata trovata", "Errore",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    

                }
            }


        }*/

        /* private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
         {
             e.KeyChar = (char)e.KeyChar;
         }*/

        

        private void btnInsieme_Click(object sender, EventArgs e)
        {
            // Check if more than one table is selected
            if (listViewTavolo.SelectedItems.Count > 1)
            {
                // Create a list for the selected tables
                List<Table> selectedTables = new List<Table>();

                // Move throw the tables
                foreach (ListViewItem item in listViewTavolo.SelectedItems)
                {
                    int tableId = int.Parse(item.SubItems[0].Text);
                    Table selectedTable = OpenTable.Database.GetAllTables().FirstOrDefault(t => t.Id == tableId);
                    if (selectedTable != null)
                    {
                        selectedTables.Add(selectedTable);
                    }
                }

                if (selectedTables.Count > 1)
                {
                    // Call to the function for combine the tables
                    CombineTables(selectedTables);
                }
                else
                {
                    MessageBox.Show("Seleziona più di una tabella per combinarle.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("È necessario selezionare almeno due tabelle da combinare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void CombineTables(List<Table> selectedTables)
        {
            // Create a new table for the mix
            string combinedTableName = "Tavolo Combinato " + selectedTables[0].Name;
            string combinedTableCode = selectedTables[0].Code.ToString() + selectedTables[1].Code.ToString();

            // Check if the mix code already exists in the mixed tables
            List<Table> allTables = OpenTable.Database.GetAllTables(); // Get all the tables from the database

            // Check if the mix code already exists in the preexisting tables
            if (allTables.Any(t => t.Code == combinedTableCode))
            {
                // If the code already exists, show the error message and stop
                MessageBox.Show("Errore: Il codice combinato esiste già. Non è possibile combinare queste tabelle.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop the execution
            }

            try
            {
                // If the code doesn't exists, go with the mix
                OpenTable.Database.SaveNewTable(combinedTableName, combinedTableCode);
                Table combinedTable = OpenTable.Database.GetTable(combinedTableCode);

                //double combinedTotal = selectedTables[0].Total+selectedTables[1].Total;
                double combinedTotal = 0.0;
                //int combinedTotalArt = selectedTables[0].TotalArt+selectedTables[1].TotalArt;
                int combinedTotalArt = 0;

                // Add the products of the selected tables to the newest
                foreach (Table table in selectedTables)
                {
                    // Get the products of the table
                    List<Product> tableProducts = OpenTable.Database.GetTableProducts(table.Id);

                    // Add each product to the new table
                    foreach (Product product in tableProducts)
                    {
                        OpenTable.Database.AddItemToTable(combinedTable.Id, product, tableProducts.Count); // Add the product to the new table
                    }

                    // Add the total and the total of products
                    combinedTotal += table.Total;
                    combinedTotalArt += table.TotalArt;

                    // Delete the products and the original table
                    OpenTable.Database.DeleteTableProducts(table.Id);
                    OpenTable.Database.DeleteTable(table.Id);
                }

                // Update the total of the new combined table
                OpenTable.Database.UpdateTableTotal(combinedTable.Id, combinedTotal);
                OpenTable.Database.UpdateTableTotalArt(combinedTable.Id, combinedTotalArt);

                // Update the interface
                UpdateListView();
            }
            catch (InvalidOperationException ex)
            {
                // Show the error message from the database
                MessageBox.Show($"Errore: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyValue.Equals(13))
            {
                string code = txtSearch.Text;
                //txtSearch_TextChanged(code);
                // Get table from the database
                Table table = OpenTable.Database.GetTable(code);

                if (table != null)
                {
                    // Open the Tavolo form and move the selected table
                    Tavolo tavoloForm = new Tavolo(table);  // Here you pass the selected table to the Tavolo form

                    // Subscribe to the FormClosed event to update the ListView when the form is closed
                    tavoloForm.FormClosed += tavoloForm_Closed;  // Here you assign the event correctly

                    tavoloForm.ShowDialog(); // Display the form as modal
                }
                else
                {
                    MessageBox.Show("La tua tabella non è stata trovata", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }

        }
    }
}