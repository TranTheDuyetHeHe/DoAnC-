using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DAO
{
    public class BILLDAO
    {
        private static BILLDAO instance;

        public static BILLDAO Instance 
        {   get { if (instance == null) instance = new BILLDAO(); return BILLDAO.instance; }
            private set { BILLDAO.instance = value; }
        }

        private BILLDAO() { }

        public int GetUncheckbillIDbyTableID(int id) 
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from bill where idtable = "+ id +" and status = 0");

            if (data.Rows.Count > 0) 
            {
                BILL bill = new BILL(data.Rows[0]);
                return bill.ID;
            }

            return -1;
        }

        public void Checkout(int id, int discount, float totalprice) 
        {
            string query = "update bill set datecheckout = getdate() , status = 1," + " discount = " + discount + ", totalprice = " + totalprice + " where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);  
        }

        public DataTable GetBillListByDate(DateTime checkin, DateTime checkout)
        {
           return DataProvider.Instance.ExecuteQuery("exec USB_GetListBilByDate @checkin , @checkout ", new object[]{checkin, checkout});
        }


        public void insertbill(int id) 
        {
            DataProvider.Instance.ExecuteNonQuery("exec USB_insertBill @idtable", new object[] { id });
        }

        public int GetMaxIdBill() 
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select max(id) from bill");
            }
            catch 
            {
                return 1;
            }
        }

    }
}
