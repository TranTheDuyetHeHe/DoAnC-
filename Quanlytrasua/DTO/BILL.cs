using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DTO
{
    public class BILL
    {
        public BILL(int id, DateTime? DateCheckIn , DateTime? DateCheckOut, int status, int discount = 0) 
        {
            this.ID = id;
            this.DateCheckIn1 = DateCheckIn;
            this.DateCheckOut1 = DateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }

        public BILL(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DateCheckIn1 = (DateTime?)row["DateCheckIn"];
            
            var dataCheckOutTemp = row["DateCheckOut"];
            if (dataCheckOutTemp.ToString() != "" ) 
            {
                this.DateCheckOut1 = (DateTime?)row["DateCheckOut"];
            }

            this.Status = (int)row["status"];

            if (row["discount"].ToString() != "")
             this.Discount = (int)row["discount"];
        }

        private int discount;

        private int status;

        private DateTime? DateCheckOut;

        private DateTime? DateCheckIn;

        private int iD;

        public int ID
        { get {  return iD; } 
          set { iD = value; }
        }

        public DateTime? DateCheckIn1 
        { get { return DateCheckIn; } 
          set { DateCheckIn = value; } 
        }

        public DateTime? DateCheckOut1 
        { get { return DateCheckOut; } 
          set { DateCheckOut = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Discount 
        { 
            get { return discount; } 
            set { discount = value; }
        }
    }
}
