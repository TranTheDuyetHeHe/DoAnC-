using Quanlytrasua.DAO;
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
    public partial class fTableManager : Form
    {
        private Account loginaccount;

        public Account LoginAccount 
        { 
            get { return loginaccount; }
            set { loginaccount = value; ChangeAccount(loginaccount.Type); } 
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();

            LoadCaetogory();

            LoadComboboxTable(cbSwitchTable);
        }

        #region Method

        void ChangeAccount(int type) 
        {
            adminToolStripMenuItem.Enabled = type == 1 == true;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.Displayname + ") ";
        }

        void LoadCaetogory() 
        {
            List<Category> list = CategoryDAO.Instance.GetListCategory();
            cbcategory.DataSource = list;
            cbcategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id) 
        {
            List<Food> listfood = FoodDAO.Instance.GetFoodsByCategoryID(id);
            cbfood.DataSource = listfood;
            cbfood.DisplayMember = "Name";
        }

        void LoadTable() 
        {
            flpTable.Controls.Clear();
           List<Table> tablelist = TableDAO.Instance.LoadTableList();
            
            foreach (Table item in tablelist) 
            {
                Button btn = new Button() { Width = TableDAO.TableWitdh, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status ;
                btn.Click += Btn_Click; 
                btn.Tag = item;

                switch (item.Status) 
                {
                    case "Trống":
                        btn.BackColor = Color.LightPink;
                        break;
                    default:
                        btn.BackColor = Color.Aqua; 
                        break;
                }

                flpTable.Controls.Add(btn); 
            }
        }

        void Showbil(int id) 
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listbilinfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalprice = 0;
            foreach (DTO.Menu item in listbilinfo) 
            {
              
                ListViewItem lsvItem = new ListViewItem(item.Foodname.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.Totalprice.ToString());
                totalprice += item.Totalprice;
                lsvBill.Items.Add(lsvItem);
            }
            txbtotalprice.Text = totalprice.ToString("c"); 

            
        }

        void LoadComboboxTable(ComboBox cb) 
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }


        #endregion

        #region Events

        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).Id;
            lsvBill.Tag = (sender as Button).Tag;
            Showbil(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.Updateaccount += F_Updateaccount;
            f.ShowDialog(); 
        }

        private void F_Updateaccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.Displayname + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += F_InsertFood;
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;
            f.ShowDialog();     
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbcategory.SelectedItem as Category).Id);
            if(lsvBill.Tag != null)
                Showbil((lsvBill.Tag as Table).Id);
            LoadTable();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbcategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                Showbil((lsvBill.Tag as Table).Id);
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbcategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                Showbil((lsvBill.Tag as Table).Id);
        }

        private void cbcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.Id;

            LoadFoodListByCategoryID(id);

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if(table == null) 
            {
                MessageBox.Show("Hãy chọn bàn !!!!");
                return;
            }

            int idbill = BILLDAO.Instance.GetUncheckbillIDbyTableID(table.Id);
            int idfood = (cbfood.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value; 

            if (idbill == -1)
            {
                BILLDAO.Instance.insertbill(table.Id);
                BillInfoDao.Instance.insertbillinfo(BILLDAO.Instance.GetMaxIdBill(), idfood, count);
            }
            else 
            {
                BillInfoDao.Instance.insertbillinfo(idbill, idfood, count);
            }
            Showbil(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BILLDAO.Instance.GetUncheckbillIDbyTableID(table.Id);
            int discount = (int)nmDisCount.Value;

            double totalprice = Convert.ToDouble(txbtotalprice.Text.Split(',')[0]);
            double finalltotalprice = totalprice - (totalprice / 100) * discount;

            if(idBill != -1) 
            {
                if (MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán không {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n => {1} - ({1} / 100) x {2} = {3} ",table.Name,totalprice,discount,finalltotalprice),"Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK);
                {
                    BILLDAO.Instance.Checkout(idBill, discount,(float)finalltotalprice);
                    Showbil(table.Id);
                    LoadTable();
                }

            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
          

            int id1 = (lsvBill.Tag as Table).Id;

            int id2 = (cbSwitchTable.SelectedItem as Table).Id;

            if (MessageBox.Show(string.Format(" Bạn thật sự có muốn chuyển bàn {0} sang bàn {1} ", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name),"Thông báo",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {    
            TableDAO.Instance.SwitchTable(id1, id2);

            LoadTable();
            }
        }

        #endregion


    }
}
