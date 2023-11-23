using Quanlytrasua.DAO;
using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlytrasua
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categorylist = new BindingSource();
        BindingSource tablelist = new BindingSource();
        BindingSource accountlist = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            Load();         
        }

        #region Methods

        List<Food> SearchFoodByName(string name) 
        {
            List<Food> listfood = FoodDAO.Instance.SearchFoodByName(name);
            return listfood;
        }

        void Load() 
        {
            dtgvAccount.DataSource = accountlist;
            dtgvTable.DataSource = tablelist;
            dtgvCategory.DataSource = categorylist;
            dtgvFood.DataSource = foodList;
            LoadAccount();
            LoadDateTimePickerBill();
            LoadListByDate(dtpFromDate.Value, dtpToDate.Value);
            LoadFoodList();
            AddFoodBinding();
            LoadCategoryToComboBox(cbFoodCategory);
            LoadCategory();
            AddCategoryBinding();
            LoadTable();
            AddTableBinding();
            AddAccountBinding();
        }

        void LoadDateTimePickerBill() 
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        
        void LoadListByDate(DateTime checkin , DateTime checkout)   
        {
            dtgvBill.DataSource = BILLDAO.Instance.GetBillListByDate(checkin, checkout);
        }

        void AddFoodBinding() 
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryToComboBox(ComboBox cb) 
        {
            cbFoodCategory.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadFoodList()
        {         
           foodList.DataSource = FoodDAO.Instance.GetListFood();

        }

        void LoadCategory() 
        {
            categorylist.DataSource = CategoryDAO.Instance.GetListCategory();

        }

        void AddCategoryBinding() 
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbcategoryname.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding() 
        {
            txbtableid.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
            txbstatustable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "username" , true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "displayname" , true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "type", true, DataSourceUpdateMode.Never));
        }

        void LoadTable() 
        {
            tablelist.DataSource = TableDAO.Instance.GetTableList();
        }

        void LoadAccount() 
        {
            accountlist.DataSource = AccountDAO.Instance.GetListAccounṭ();
        }

        void AddAccount(string username, string displayname, int type) 
        {
           if( AccountDAO.Instance.InsertAccount(username, displayname, type)) 
            {
                MessageBox.Show("Thêm tài khoản thành công !!!!!");
            }
           else
            {
                MessageBox.Show("Có lỗi khi thêm tài khoản !!!!");
            }
            LoadAccount();         
        }

        void EditAccount(string username, string displayname, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, displayname, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công !!!!!");
            }
            else
            {
                MessageBox.Show("Có lỗi khi cập nhật tài khoản !!!!");
            }
            LoadAccount();
        }


        void DeleteAccount(string username )
        {
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Xóa tài khoản thành công !!!!!");
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa tài khoản !!!!");
            }
            LoadAccount();
        }


        void ResetPassword(string username) 
        {
            if (AccountDAO.Instance.ResetPassword(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công !!!!!");
            }
            else
            {
                MessageBox.Show("Có lỗi khi đặt lại mật khẩu !!!!");
            }
        }

        #endregion

        #region Events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListByDate(dtpFromDate.Value, dtpToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadFoodList();
        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["idcategory"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryById(id);
                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.Id == category.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            {

            }
        }

        private void txbFoodName_TextChanged(object sender, EventArgs e)
        { 
               
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int idcategory = (cbFoodCategory.SelectedItem as Category).Id;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, idcategory, price))
            {
                MessageBox.Show("Thêm món thành công !!!!!");
                LoadFoodList();
                if(insertfood != null) 
                {
                    insertfood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món !!!!");
            }
        }

        private void btnUpdateFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int idcategory = (cbFoodCategory.SelectedItem as Category).Id;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, idcategory, price))
            {
                MessageBox.Show("Sửa tên món thành công !!!!!");
                LoadFoodList();
                if (updatefood != null) 
                {
                    updatefood(this , new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa tên món !!!!");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công !!!!!");
                LoadFoodList();
                if (deletefood != null) 
                {
                    deletefood(this , new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món !!!!");
            }
        }

        private event EventHandler insertfood;
        public event EventHandler InsertFood 
        { 
            add { insertfood += value; }
            remove { insertfood -= value; }
        }

        private event EventHandler deletefood;
        public event EventHandler DeleteFood
        {
            add { deletefood += value; }
            remove { deletefood -= value; }
        }

        private event EventHandler updatefood;
        public event EventHandler UpdateFood
        {
            add { updatefood += value; }
            remove { updatefood -= value; }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
           foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbcategoryname.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục mới thành công !!!!!");
                LoadCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục!!!!");
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            string name = txbcategoryname.Text;
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.UpdateCategory(name, id))
            {
                MessageBox.Show("Sửa danh mục thành công !!!!!");
                LoadCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục!!!!");
            }
        }


        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công !!!!!");
                LoadCategory();  
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục!!!!");
            }
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;          

            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn mới thành công !!!!!");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn mới!!!!");
            }
        }

        private void btnUpdateTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            int id = Convert.ToInt32(txbtableid.Text);

            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Sửa bàn thành công !!!!!");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn!!!!");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbtableid.Text);

            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công !!!!!");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn!!!!");
            }
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            string displayname = txbDisplayName.Text;
            int type = (int)nmType.Value;

            AddAccount(username, displayname, type);

        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            DeleteAccount(username);
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            string displayname = txbDisplayName.Text;
            int type = (int)nmType.Value;

            EditAccount(username, displayname, type);
        }

        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            ResetPassword(username);
        }

        #endregion


    }
}
