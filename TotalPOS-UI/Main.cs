using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTable;
using OpenTable.Models;

namespace TotalPOS_UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        // Evento al cargar el formulario (se ejecuta cuando se abre la aplicación)
        private void Main_Load(object sender, EventArgs e)
        {
            LoadTables();  // Cargar las mesas automáticamente al iniciar
        }

        private void btnApri_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una mesa
            if (listViewTavolo.SelectedItems.Count > 0)
            {
                // Obtener el ID de la mesa seleccionada
                int tableId = int.Parse(listViewTavolo.SelectedItems[0].SubItems[0].Text);

                MessageBox.Show(OpenTable.Database.prueba(tableId).ToString());

                // Buscar la mesa en la lista de mesas cargada desde la base de datos
                Table selectedTable = OpenTable.Database.GetAllTables().FirstOrDefault(t => t.Id == tableId);

                if (selectedTable != null)
                {
                    // Abrir el formulario Tavolo y pasar la mesa seleccionada
                    Tavolo tavoloForm = new Tavolo(selectedTable);  // Aquí pasas la mesa seleccionada al formulario Tavolo

                    // Suscribirse al evento FormClosed para actualizar el ListView al cerrar el formulario
                    tavoloForm.FormClosed += tavoloForm_Closed;  // Aquí asignas el evento correctamente

                    tavoloForm.ShowDialog(); // Mostrar el formulario como modal
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
            // Llamar al método para actualizar el ListView al cerrarse Form2
            UpdateListView();
        }

        private void UpdateListView()
        {
            // Llamar al método para cargar las mesas nuevamente desde la base de datos
            LoadTables();
        }

        // Método para cargar las mesas desde la base de datos y mostrarlas en el ListView
        public void LoadTables()
        {    
            List<Table> tables = OpenTable.Database.GetAllTables(); // Obtener todas las mesas desde la base de datos

            listViewTavolo.Items.Clear(); // Limpiar el ListView antes de cargar los nuevos datos

            foreach (var table in tables)
            {

                //double total = OpenTable.Database.GetTableTotal(table.Id);
                // Crear un nuevo item para el ListView con la información de la mesa
                ListViewItem item = new ListViewItem(new[]
                {
                    table.Id.ToString(),
                    table.Name,
                    table.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    "CHF    "+table.Total.ToString("F2"),
                    table.TotalArt.ToString()
                });

                listViewTavolo.Items.Add(item); // Agregar el item al ListView
            }
        }

        private void listViewTavolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Este método puede usarse para hacer algo cuando el usuario selecciona una mesa
            // Por ejemplo, podrías mostrar un mensaje con el nombre de la mesa seleccionada:
            if (listViewTavolo.SelectedItems.Count > 0)
            {
                string selectedTableName = listViewTavolo.SelectedItems[0].SubItems[1].Text;
                // Aquí puedes hacer algo con el nombre de la mesa seleccionada si lo deseas
            }
        }

        private void btnNuovo_Click(object sender, EventArgs e)
        {
            // Obtener todas las mesas desde la base de datos
            List<Table> tables = OpenTable.Database.GetAllTables();

            // Obtener todos los números de las mesas existentes
            var existingNumbers = tables
                .Select(t => int.TryParse(t.Name.Split(' ').Last(), out int number) ? number : -1)
                .Where(number => number != -1)
                .OrderBy(number => number)
                .ToList();

            // Buscar el primer número disponible (el número que falta en la secuencia)
            int newTableNumber = 1; // Empezar desde el 1

            // Encontrar el primer número que falta en la secuencia
            foreach (var number in existingNumbers)
            {
                if (number != newTableNumber)
                {
                    break; // El número libre encontrado es el primero que falta
                }
                newTableNumber++;
            }

            // Crear una nueva mesa con el número encontrado
            string tableName = "Tavolo " + newTableNumber;

            // Guardar la nueva mesa en la base de datos
            OpenTable.Database.SaveNewTable(tableName);

            // Actualizar el ListView
            LoadTables();
        }
    


        private void btnCancellare_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una mesa
            if (listViewTavolo.SelectedItems.Count > 0)
            {
                // Obtener el ID de la mesa seleccionada
                int tableId = int.Parse(listViewTavolo.SelectedItems[0].SubItems[0].Text);

                // Eliminar la mesa de la base de datos
                OpenTable.Database.DeleteTable(tableId);

                // Actualizar el ListView para reflejar los cambios
                LoadTables();
            }
            else
            {
                MessageBox.Show("Seleziona una tabella da eliminare.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
