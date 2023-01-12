using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityFramework;
namespace Models.DAL
{
    public class DAL_Slides
    {
        protected ShopQuanAoDBContext context { get; set; }
        public DAL_Slides()
        {
            this.context = new ShopQuanAoDBContext();
        }
        //phân trang
        public  List<SLIDE> GetPageSlides(int page, int itemsPerPage, out int totalCount)
        {
            var list = new List<SLIDE>();
            //láy ra ds các slide mà is_remove ==false sau đó sắp xếp
            var arrSanPham = this.context.SLIDEs.Where(a => a.IS_REMOVE==false).OrderBy(c => c.NGAY_DANG).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            foreach (var item in arrSanPham)
            {
                list.Add(item);
            }//đếm số phần từ
            totalCount = this.context.SLIDEs.Count();
            return list;
        }
        
        public bool sqlQueryFristOrDefault(string sql)
        {
            var sp = this.context.SLIDEs.SqlQuery(sql).FirstOrDefault();
            if (sp != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //thêm mới
        public bool InsertSlides(SLIDE slide)
        {
            try
            {
                slide.NGAY_DANG = DateTime.Now;
                slide.NOI_BAT = false;
                slide.IS_REMOVE = false;
                this.context.SLIDEs.Add(slide);
                this.context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        //cập nhật
        public bool EditSlides(int id , SLIDE slide)
        {
            try
            {
                var sl = this.context.SLIDEs.Find(id);
                if(sl != null)
                {
                    sl.TIEU_DE = slide.TIEU_DE;
                    sl.LINK = slide.LINK;
                    sl.STT = slide.STT;
                    sl.IMAGES = slide.IMAGES;
                    if(slide.TRANG_THAI != null)
                    {
                        sl.TRANG_THAI =slide.TRANG_THAI;
                    }
                    else
                    {
                        sl.TRANG_THAI = false;
                    }
                    this.context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public SLIDE returnSlides(int id)
        {
            return this.context.SLIDEs.Find(id);
        }

        public bool DeleteSlide(int id)
        {
            var slide = this.context.SLIDEs.Find(id);
            slide.IS_REMOVE = true;
            this.context.SaveChanges();
            return true;
        }
    }
}
