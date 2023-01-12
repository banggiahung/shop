using Models.ADO;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL
{
    public class DAL_Color
    {
        private ShopQuanAoDBContext context = null;

        public DAL_Color()
        {
            this.context = new ShopQuanAoDBContext();
        }

        public int insertColor(COLOR color)
        {
            try
            {
                COLOR cl = this.context.COLORs.Add(color);
                this.context.SaveChanges();
                return cl.ID;
            }
            catch
            {
                return 0;
            }
        }

        public COLOR returnColor(int id)
        {
            return this.context.COLORs.Find(id);
        }

        public List<COLOR> getColorMasp(string masp)
        {
            return this.context.COLORs.Where(a => a.MA_SP == masp).ToList();
        }
        public List<ColorProduct> convertGetColorProduct(string masp)
        {
            var list = new List<ColorProduct>();

            var arr = this.getColorMasp(masp);
            if (arr.Count > 0)
            {
                foreach(var item in arr)
                {
                    var color = new ColorProduct();
                    color.ID = item.ID;
                    color.IMAGES = item.IMAGES;
                    color.MA_MAU = item.MA_MAU;
                    color.TEN_MAU = item.TEN_MAU;
                    color.TRANG_THAI = item.TRANG_THAI;
                    color.Slug = item.SLUG;
                    list.Add(color);
                }
            }

            return list;
        }

        public bool updateColor( int id , COLOR c)
        {
            try
            {
                var color = this.context.COLORs.Find(id);
                if(color != null)
                {
                    color.TEN_MAU = c.TEN_MAU;
                    color.MA_MAU = c.MA_MAU;
                    color.MA_SP = c.MA_SP;
                    if(c.IMAGES!= "")
                    {
                        color.IMAGES = c.IMAGES;
                    }
                    color.SLUG = c.SLUG;
                    
                }
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkIssetInsertColorMasp(string masp , string slug)
        {
            var sql = "SELECT * FROM COLOR WHERE COLOR.MA_SP = '" + masp + "' AND COLOR.SLUG='" + slug + "';";
            var color = this.context.COLORs.SqlQuery(sql).FirstOrDefault();
            if(color != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
