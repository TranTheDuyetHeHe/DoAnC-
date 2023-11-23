using Quanlytrasua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlytrasua.DAO
{
    public class BillInfoDao
    {
        private static BillInfoDao instance;

        public static BillInfoDao Instance 
        { get { if (instance == null) instance = new BillInfoDao(); return BillInfoDao.instance; }
          private set { BillInfoDao.instance = value; }
        }

        private BillInfoDao() { }

        public void DeleteBillInfoFoodID(int id) 
        {
            DataProvider.Instance.ExecuteQuery("delete billinfo where idfood = " + id + "");
        }

        public List<BillInfo> GetlistBillInfo(int id) 
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from billinfo where idbill = "+ id +"");

            foreach (DataRow item in data.Rows) 
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void insertbillinfo(int idbill, int idfood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USB_insertBillinfo @idbill  , @idfood  , @count ", new object[] { idbill, idfood, count });
        }
    }
}
