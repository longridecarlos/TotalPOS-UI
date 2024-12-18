using System;
using Newtonsoft.Json;

namespace OpenTable.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // Método para serializar el objeto Product a JSON
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);  // Serializa el objeto en JSON
        }

        // Método para actualizar la cantidad y calcular el total
        public double GetTotal()
        {
            return Quantity * Price;
        }
    }
}
