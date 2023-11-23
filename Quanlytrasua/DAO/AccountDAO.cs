using Quanlytrasua.DAO;
using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        {
            get { if (instance == null) instance = new AccountDAO();return instance; }
            private set { instance = value; }
        }
        private AccountDAO() {  }


        public DataTable GetListAccounṭ()
        {
            return DataProvider.Instance.ExecuteQuery("select username , displayname , type from account");
        }

        public bool Login1(string userName, string passWord) 
        {
            string query = "USB_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });
            return result.Rows.Count > 0; 
        }

        public bool UpdateAccount(string username , string displayname , string pass , string newpass) 
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USB_UpdateAccount @username  , @displayname , @password  , @newpassword ", new object[]{ username, displayname, pass, newpass });
            return result > 0;

        }

        public Account GetAccountByUserName(string userName)
        {
           DataTable data =  DataProvider.Instance.ExecuteQuery("select * from account where username = '" + userName +"'");

            foreach (DataRow item in data.Rows) 
            {
                return new Account(item);
            }

            return null;
        }

        public bool InsertAccount(string username, string displayname , int type)
        {
            string query = string.Format("insert account (username, displayname, type) values (N'{0}', N'{1}', {2})", username, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount( string username, string displayname, int type)
        {
            string query = string.Format("update account set displayname = N'{1}' , type = {2} where username = N'{0}'",  username, displayname, type );
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            
            string query = string.Format("delete account where username = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public bool ResetPassword(string name)
        {

            string query = string.Format("update account set password = '0' where username = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
