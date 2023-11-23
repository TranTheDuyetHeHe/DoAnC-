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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            
        }

       
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn thật sự muốn thoát chương trình ?","Thông Báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK) 
                { e.Cancel = true;
            }
        }

        private void btnLogin1_Click(object sender, EventArgs e)
        {   
            string userName = txbUserName1.Text;
            string passWord = txbPassWord1.Text;
            if (Login1(userName,passWord)) 
            {
                Account loginaccount = AccountDAO.Instance.GetAccountByUserName(userName);
                fTableManager f = new fTableManager(loginaccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!!");
            }
        }
        bool Login1(string userName,string passWord)
        {  
            return AccountDAO.Instance.Login1(userName, passWord);  
        }
        private void btnThoat1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
