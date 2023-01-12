using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityFramework;
namespace Models.DAL
{
    public class DAL_NhanVien
    {
        private ShopQuanAoDBContext context = null;
        public DAL_NhanVien()
        {
            this.context = new ShopQuanAoDBContext();
        }
        public List<NHAN_VIEN> getDanhSach()
        {
            return this.context.NHAN_VIEN.Select(a => a).ToList();
        }

        public bool insertNhanV(string ten, int id)
        {
            try
            {
                var nhanv = new NHAN_VIEN();
                nhanv.MA_NV = id;
                nhanv.TEN_NHAN_VIEN = ten;

                this.context.NHAN_VIEN.Add(nhanv);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool editNhanV(int id, string ten)
        {
            try
            {
                var NhanV = this.context.NHAN_VIEN.Find(id);
                NhanV.MA_NV = id;
                NhanV.TEN_NHAN_VIEN = ten;
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public NHAN_VIEN returnNhanV(int id)
        {
            return this.context.NHAN_VIEN.Find(id);
        }

        public bool ChangeStatus(int id)
        {

            var NhanV = this.context.NHAN_VIEN.Find(id);

            return true;


        }

        public bool ValidateInsertNhanV(string key, string value)
        {

            var sql = "SELECT * FROM NhanV WHERE " + key + " = '" + value + "';";
            var NhanV = this.context.NHAN_VIEN.SqlQuery(sql).FirstOrDefault();
            if (NhanV != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ValidateUpdateNhanV(int id, string key, string value)
        {

            var NhanV = this.context.NHAN_VIEN.SqlQuery("SELECT * FROM NhanVien WHERE MA_NV != " + id + " AND " + key + " = '" + value + "';").FirstOrDefault();
            if (NhanV != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool deleteNhanV(int id)
        {
            try
            {

                var NhanV = this.context.NHAN_VIEN.Find(id);

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}