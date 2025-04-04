using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApacheCassandra.Models
{
    public class Productos
    {
        //private int idInsert;
        //private string nameInsert;
        //private double priceInsert;
        //private int unitsInsert;

        //public Productos(int idInsert, string nameInsert, double priceInsert, int unitsInsert)
        //{
        //    this.idInsert = idInsert;
        //    this.nameInsert = nameInsert;
        //    this.priceInsert = priceInsert;
        //    this.unitsInsert = unitsInsert;
        //}

        public int id { get; }
        public string name { get; }
        public double price { get; }
        public int units { get; }
        public Productos(int id, string name, double price, int units)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.units = units;
        }

}
}
