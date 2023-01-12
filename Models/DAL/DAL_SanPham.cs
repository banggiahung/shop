using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ADO;
using Models.EntityFramework;
namespace Models.DAL
{
    public class DAL_SanPham
    {
        private ShopQuanAoDBContext context = null;

        public DAL_SanPham()
        {
            this.context = new ShopQuanAoDBContext();
        }

        /// insert dữ liệu vào bảng Sản Phẩm ;
        public bool insertSanPham(SAN_PHAM sp)
        {
            try
            {
                sp.IS_REMOVE = false;
                this.context.SAN_PHAM.Add(sp);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //phân trang
        public List<ModelsSanPham> GetPageSanPham(int page, int itemsPerPage, out int totalCount , string search)
        {
            var list = new List<ModelsSanPham>();
            var arrSanPham = new List<SAN_PHAM>();
            if (search!= null)
            {
                arrSanPham = this.context.SAN_PHAM.Where(a => a.TEN_SP.Contains(search) && a.IS_REMOVE == false).OrderBy(c => c.NGAY_TAO).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
                totalCount = this.context.SAN_PHAM.Where(a => a.TEN_SP.Contains(search) && a.IS_REMOVE == false).Count();
            }
            else
            {//skip: loại bỏ số phần tử từ đầu
                //take: lấy số phần tử từ đầu
                arrSanPham = this.context.SAN_PHAM.Where(a => a.IS_REMOVE == false).OrderBy(c => c.NGAY_TAO).Skip(itemsPerPage * page).Take(itemsPerPage)
                     .ToList();
                totalCount = this.context.SAN_PHAM.Where(a => a.IS_REMOVE == false).Count();
            }
            foreach ( var item in arrSanPham)
            {
                list.Add(convertSanPham(item));
            }
            return list;
        }
        //chuyển sản phẩm để lấy cả sản phẩm, bảng size và màu
        public ModelsSanPham convertSanPham(SAN_PHAM item)
        {
            var DAL_Size = new DAL_Size();
            var DAL_Color = new DAL_Color();
            var DAL_spct = new DAL_SanPhamChiTiet();
            var DAL_LSP = new DAL_LoaiSanPham();
            var sp = new ModelsSanPham();
            if(item == null)
            {
                return sp;
            }
            sp.MA_SP = item.MA_SP;
            sp.TEN_SP = item.TEN_SP;
            sp.MO_TA = item.MO_TA;
            sp.MO_TA_CHI_TIET = item.MO_TA_CHI_TIET;
            sp.SLUG = item.SLUG;
            sp.NOI_BAT = item.NOI_BAT;
            sp.LINK_ANH_CHINH = item.LINK_ANH_CHINH;
            sp.LIST_ANH_KEM = item.LIST_ANH_KEM;
            sp.NGAY_TAO = item.NGAY_TAO;
            sp.SO_LUONG_TONG = item.SO_LUONG_TONG;
            sp.TRANG_THAI = item.TRANG_THAI;
            sp.LUOT_XEM = item.LUOT_XEM;
            sp.ID_LSP = item.ID_LSP;
            sp.GIA_NHAP = item.GIA_NHAP;
            sp.GIA_BAN = item.GIA_BAN;
            sp.GIAM_GIA = item.GIAM_GIA;
            sp.DON_VI_TINH = item.DON_VI_TINH;
            sp.TenLoaiSP = DAL_LSP.returnLoaiSanPham(sp.ID_LSP).TEN_LOAI_SP;
            var arrSanPhamDetails = DAL_spct.getListSanPhamDetails(item.MA_SP);
            var listDetails = new List<SanPhamDetails>();
            foreach (var details in arrSanPhamDetails)
            {
                var spct = new SanPhamDetails();
                spct.ID = details.ID;
                spct.ID_COLOR = details.ID_COLOR;
                spct.ID_SIZE = details.ID_SIZE;
                spct.Image = DAL_Color.returnColor(details.ID_COLOR).IMAGES;
                spct.TenSize = DAL_Size.returnSize(details.ID_SIZE).TEN_SIZE;
                spct.MoTaSize = DAL_Size.returnSize(details.ID_SIZE).MO_TA;
                spct.MA_SP = details.MA_SP;
                spct.TRANG_THAI = details.TRANG_THAI;
                spct.SO_LUONG = details.SO_LUONG;
                spct.TenColor = DAL_Color.returnColor(details.ID_COLOR).TEN_MAU;
                spct.NGAY_TAO = details.NGAY_TAO;
                spct.SLUG = details.SLUG;
                listDetails.Add(spct);
            }
            sp.listSanPhamDetails = listDetails;

            var arrColor = DAL_Color.getColorMasp(item.MA_SP);
            var listColor = new List<ColorProduct>();
            foreach (var color in arrColor)
            {
                var col = new ColorProduct();
                col.ID = color.ID;
                col.MA_SP = color.MA_SP;
                col.TRANG_THAI = color.TRANG_THAI;
                col.TEN_MAU = color.TEN_MAU;
                col.IMAGES = color.IMAGES;
                col.MA_MAU = color.MA_MAU;
                col.Slug = color.SLUG;
                listColor.Add(col);
            }
            sp.listSanPhamColor = listColor;
            return sp;
        }

        public ModelsSanPham getSanPham(string ma)
        {
            var sp = this.context.SAN_PHAM.Find(ma);
            return this.convertSanPham(sp);
        }

        public SAN_PHAM ReturnSAN_PHAM(string ma)
        {
            return this.context.SAN_PHAM.Find(ma);
        }


        public bool checkQueryFirstOrDefault(string sql)
        {
            var sp = this.context.SAN_PHAM.SqlQuery(sql).FirstOrDefault();
            if (sp != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool UpdateSanPham( string masp ,  SAN_PHAM sp)
        {
            try
            {
                var sanpham = this.context.SAN_PHAM.Find(masp);
                if(sanpham != null)
                {
                    sanpham.ID_LSP = sp.ID_LSP;
                    sanpham.GIA_BAN = sp.GIA_BAN;
                    sanpham.GIA_NHAP = sp.GIA_NHAP;
                    sanpham.GIAM_GIA = sp.GIAM_GIA;
                    if (sp.LINK_ANH_CHINH != "")
                    {
                        sanpham.LINK_ANH_CHINH = sp.LINK_ANH_CHINH;
                    }
                    if (sp.NOI_BAT != null)
                    {
                        sanpham.NOI_BAT = sp.NOI_BAT;
                    }
                    if (sp.TRANG_THAI != null)
                    {
                        sanpham.TRANG_THAI = sp.TRANG_THAI;
                    }
                    if (sp.LIST_ANH_KEM != "")
                    {
                        sanpham.LIST_ANH_KEM = sp.LIST_ANH_KEM;
                    }
                    sanpham.MO_TA = sp.MO_TA;
                    sanpham.MO_TA_CHI_TIET = sp.MO_TA_CHI_TIET;
                    sanpham.DON_VI_TINH = sp.DON_VI_TINH;
                    sanpham.TEN_SP = sp.TEN_SP;
                    sanpham.SLUG = sp.SLUG;
                    this.context.SaveChanges();
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteSP(string id)
        {
            var sp = this.context.SAN_PHAM.SingleOrDefault(a => a.MA_SP == id);
            if(sp!= null)
            {
                sp.IS_REMOVE = true;
                this.context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    

}
