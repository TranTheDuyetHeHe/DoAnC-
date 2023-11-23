using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWitdh = 80;
        public static int TableHeight = 80;

        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USB_Switchtable @idtable1 , @idtable2", new object[] {id1 , id2});
        }

        public List<Table> LoadTableList()
        {
            List<Table> tablelist = new List<Table>();
            DataTable data =  DataProvider.Instance.ExecuteQuery("USB_GetTableList");

            foreach (DataRow item in data.Rows) 
            {
                Table table = new Table(item);
                tablelist.Add(table);
            }

            return tablelist; 
        }

        public List<Table> GetTableList() 
        {
            List<Table> list = new List<Table>();

            string query = "select * from tablefood ";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                list.Add(table);
            }

            return list;
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("insert tablefood (name )  values (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(int id, string name)
        {
            string query = string.Format("update tablefood set name = N'{0}' , status  =  N'Trống'  where id = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int id)
        {
            BillInfoDao.Instance.DeleteBillInfoFoodID(id);

            string query = string.Format("delete tablefood where id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
