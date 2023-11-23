using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DTO
{
    public class Food
    {
        private string name;
        private int id;
        private float price;
        private int idCategory;

        public Food(int id, string name, int idCategory, float price) 
        {
            this.Id = id;
            this.Name = name;
            this.IdCategory = idCategory;
            this.Price = price;
        }

        public Food(DataRow row)
        {
            this.Id = (int)row["id"];   
            this.Name = row["name"].ToString();
            this.IdCategory = (int)row["idCategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }


        public int Id 
        { 
            get { return id; } 
            set { id = value; }
        }
        public string Name 
        { 
            get { return name; }
            set { name = value; }
        }
        public float Price 
        { 
            get { return price; }
            set { price = value; }
        }
        public int IdCategory 
        { 
            get { return idCategory; }
            set { idCategory = value; }
        }
    }
}
