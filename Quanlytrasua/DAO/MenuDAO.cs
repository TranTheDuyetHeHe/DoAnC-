using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DAO
{
        public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; } 
            private set { MenuDAO.Instance = value; } 
        }

        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> list = new List<Menu>();

            string query = "select f.name, bi.count, f.price, f.price*bi.count as totalprice from billinfo as bi , bill as b , food as f where bi.idbill = b.id and bi.idfood = f.id and b.status = 0 and b.idtable =  "+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                list.Add(menu);
            }

            return list;
        }
    }
}
