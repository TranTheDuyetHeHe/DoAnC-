using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlytrasua
{
    public partial class fAccountProfile : Form
    {
        private Account loginaccount;

        public Account Loginaccount
        {
            get { return loginaccount; }
            set { loginaccount = value; ChangeAccount(loginaccount); }
        }


        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            Loginaccount = acc;
        }


        void ChangeAccount(Account acc)
        {
            txbUserName.Text = Loginaccount.Username;
            txbDisplayName.Text = loginaccount.Displayname;

        }

        void UpdateAccount()
        {
            string dispalyName = txbDisplayName.Text;
            string password = txbPassWord1.Text;
            string newpass = txtNewPass.Text;
            string reenterpass = txtReEnterPass.Text;
            string username = txbUserName.Text;

            if (!newpass.Equals(reenterpass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới !!!!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(username, dispalyName, password, newpass))
                {
                    MessageBox.Show("Cập nhập thành công !!!");
                    if(updateAccount != null)
                    {
                        updateAccount(this,new AccountEvent(AccountDAO.Instance.GetAccountByUserName(username)));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> Updateaccount 
        {
            add { updateAccount += value; } 
            remove { updateAccount -= value; }
        }


            

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbDisplayName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }
    }

    public class AccountEvent : EventArgs 
    {
        private Account acc;

        public Account Acc 
        { 
            get { return acc; } 
            set { acc  = value; } 
        }

        public AccountEvent(Account acc)
        {
           this.Acc = acc;         
        }
    }
}
