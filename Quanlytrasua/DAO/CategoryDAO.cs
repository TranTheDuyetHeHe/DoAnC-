using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance 
        { 
            get { if (instance == null)instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory() 
        {
            List<Category> listcategory = new List<Category>();

            string query = "select * from foodcategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

             foreach (DataRow item in data.Rows) 
            {
                Category category = new Category(item);
                listcategory.Add(category);
            }

            return listcategory;
        }

        public Category GetCategoryById(int id)
        {
            Category category = null;

            string query = "select * from foodcategory where id =" + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }

            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = string.Format("insert foodcategory (name) values (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(string name, int id)
        {
            string query = string.Format("update foodcategory set name = N'{0}' where id ={1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int id)
        {
            FoodDAO.Instance.DeleteFood(id);
            string query = string.Format("delete foodcategory where id ={0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
