using Models.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL
{
    public class DAL_LoaiSanPham
    {
        private ShopQuanAoDBContext context = null;

        public DAL_LoaiSanPham()
        {
            this.context = new ShopQuanAoDBContext();
        }

        public List<LOAI_SAN_PHAM> getDanhSach()
        {
            return this.context.LOAI_SAN_PHAM.ToList();
        }

        public IEnumerable<LOAI_SAN_PHAM> ListAllPaging(int page, int pageSize , string search)
        {
            if(search != null)
            {
                return this.context.LOAI_SAN_PHAM.Where(x =>x.TEN_LOAI_SP.Contains(search) && x.IS_REMOVE == false ).OrderBy(x => x.ID_CHA).ToPagedList(page, pageSize);
            }
            return this.context.LOAI_SAN_PHAM.Where(a=> a.IS_REMOVE == false).OrderBy(x => x.ID_CHA).ToPagedList(page, pageSize);
        }
        

        public bool insertLoaiSanPham(LOAI_SAN_PHAM lsp)
        {
            try
            {
                lsp.IS_REMOVE = false;
                lsp.NGAY_TAO = DateTime.Now;
                lsp.TRANG_THAI = false;
                this.context.LOAI_SAN_PHAM.Add(lsp);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateLoaiSanPham( int id , LOAI_SAN_PHAM lsp)
        {
            try
            {
               
                var lspEdit = this.context.LOAI_SAN_PHAM.Find(id);
                lspEdit.SLUG = lsp.SLUG;
                lspEdit.ID_CHA = lsp.ID_CHA;
                lspEdit.TEN_LOAI_SP = lsp.TEN_LOAI_SP;
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

       

        public bool Check_isset_danh_muc_insert(string slug)
        {
           
                var lsp = this.context.LOAI_SAN_PHAM.Where(f => String.Compare(f.SLUG, slug, true) == 0).FirstOrDefault();
                if (lsp == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
           
        }

        public bool Check_isset_danh_muc_update(int id, string slug)
        {
           
                var lsp = this.context.LOAI_SAN_PHAM.Where(c => String.Compare(c.SLUG, slug, true) == 0 && c.ID_LOAI_SP != id).FirstOrDefault();
                if (lsp == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
           
        }

        public bool ChangeStatus(int id)
        {
           
            var lsp = this.context.LOAI_SAN_PHAM.Find(id);
            lsp.TRANG_THAI = !lsp.TRANG_THAI;
            this.context.SaveChanges();
            return (bool)!lsp.TRANG_THAI;
            
        }

        public bool DeleteLoaiSanPham(int id)
        {
            try
            {
                var lsp = this.context.LOAI_SAN_PHAM.Find(id);
                lsp.IS_REMOVE = true;
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public LOAI_SAN_PHAM returnLoaiSanPham(int id)
        {
            return this.context.LOAI_SAN_PHAM.Find(id);
        }

    }
}
