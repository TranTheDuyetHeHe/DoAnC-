using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DTO
{
    public class Table
    {
        public Table(int id, string name, string satatus)
        {
            this.iD = id;
            this.Name = name;
            this.Status = status;
        }
        private string status;
        public string Status 
        {
            get { return status; }
            set { status = value; }
        }

        public Table(DataRow row) 
        {
            this.iD = (int)row["ID"];
            this.Name = row["Name"].ToString();
            this.Status = row["status"].ToString();
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private int iD;

        public int Id 
        {
            get { return iD; }  
            set { iD = value; } 
        }

       
    }
}
