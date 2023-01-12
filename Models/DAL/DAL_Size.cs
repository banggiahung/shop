using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityFramework;
namespace Models.DAL
{
    public class DAL_Size
    {
        private ShopQuanAoDBContext context = null;
        public DAL_Size()
        {
            this.context = new ShopQuanAoDBContext();
        }
        public List<SIZE> getDanhSach()
        {
            return this.context.SIZEs.Select(a => a).ToList();
        }

        public bool insertSize(string ten, string slug, string mota)
        {
            try
            {
                var size = new SIZE();
                size.TEN_SIZE = ten;
                size.SLUG = slug;
                size.MO_TA = mota;
                size.TRANG_THAI = true;
                size.IS_REMOVE = false;
                this.context.SIZEs.Add(size);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool editSize(int id, string ten, string slug, string mota)
        {
            try
            {
                var size = this.context.SIZEs.Find(id);
                size.MO_TA = mota;
                size.TEN_SIZE = ten;
                size.SLUG = slug;
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public SIZE returnSize(int id)
        {
            return this.context.SIZEs.Find(id);
        }

        public bool ChangeStatus(int id)
        {
            
                var size = this.context.SIZEs.Find(id);
                size.TRANG_THAI = !size.TRANG_THAI;
                this.context.SaveChanges();
                return (bool)!size.TRANG_THAI;
            
        }

        public bool ValidateInsertSize(string key, string value)
        {

            var sql = "SELECT * FROM Size WHERE " + key + " = '" + value + "';";
                var size = this.context.SIZEs.SqlQuery(sql).FirstOrDefault();
                if (size != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
        }

        public bool ValidateUpdateSize(int id, string key, string value)
        {
        
                var size = this.context.SIZEs.SqlQuery("SELECT * FROM Size WHERE ID != " + id + " AND " + key + " = '" + value + "';").FirstOrDefault();
                if (size != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
        }

        public bool deleteSize(int id)
        {
            try
            {
                
                    var size = this.context.SIZEs.Find(id);
                    size.IS_REMOVE = true;
                    this.context.SaveChanges();
                    return true;
               
            }
            catch
            {
                return false;
            }
        }
    }
}
