using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DTO
{
     public class Menu
    {
        private string foodname;
        private int count;
        private float price;
        private float totalprice;

        public Menu(string foodname, int count, float price, float totalprice)
        {
            this.Foodname = foodname;
            this.Count = count;
            this.Price = price;
            this.Totalprice = totalprice;
        }

        public Menu(DataRow row)
        {
            this.Foodname = row["name"].ToString();
            this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.Totalprice = (float)Convert.ToDouble(row["totalprice"].ToString());
        }

        public string Foodname
        {
            get { return foodname; }
            set { foodname = value; }
        }

        public int Count 
        { get { return count; }
          set { count = value; }
        }

        public float Price 
        { get { return price; }
          set { price = value; }  
        }

        public float Totalprice 
        {
            get { return totalprice; }  
            set { totalprice = value; } 
        }
    }
}
