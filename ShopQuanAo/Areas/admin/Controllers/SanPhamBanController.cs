using Models.DAL;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopQuanAo.Object;
using Newtonsoft.Json;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class SanPhamBanController : Controller
    {
        private ShopQuanAoDBContext db = new ShopQuanAoDBContext();

        // GET: admin/ThongKe
        public ActionResult Index()
        {
            var spBanChay = from sp in this.db.SAN_PHAM.ToList()
                            join lsp in this.db.LOAI_SAN_PHAM.ToList()
                            on sp.ID_LSP equals lsp.ID_LOAI_SP
                            where sp.NOI_BAT == true
                            select new SP
                            {
                                MaSP = sp.MA_SP,
                                loaiSP = lsp.TEN_LOAI_SP,
                                TenSP = sp.TEN_SP,
                                LinkAnh = sp.LINK_ANH_CHINH,
                                soLuong = (int)sp.SO_LUONG_TONG,
                                donViTinh = sp.DON_VI_TINH,
                                giaNhap = sp.GIA_NHAP,
                                giaBan = sp.GIA_BAN,
                                trangThai = (bool)sp.TRANG_THAI
                            };

            ViewBag._SanPhamBanChay = spBanChay.ToList();
            return View();
        }
        public ActionResult SanPhamHetHang()
        {
            var sphetHang = from sp in this.db.SAN_PHAM.ToList()
                            join lsp in this.db.LOAI_SAN_PHAM.ToList()
                            on sp.ID_LSP equals lsp.ID_LOAI_SP
                            where sp.SO_LUONG_TONG <= 10000
                            select new SP
                            {
                                MaSP = sp.MA_SP,
                                loaiSP = lsp.TEN_LOAI_SP,
                                TenSP = sp.TEN_SP,
                                LinkAnh = sp.LINK_ANH_CHINH,
                                soLuong = (int)sp.SO_LUONG_TONG,
                                donViTinh = sp.DON_VI_TINH,
                                giaNhap = sp.GIA_NHAP,
                                giaBan = sp.GIA_BAN,
                                trangThai = (bool)sp.TRANG_THAI
                            };
            ViewBag._spHetHang = sphetHang.ToList();
            return View();
        }
        public ActionResult SanPhamKhongBanDuoc()
        {
            var spKhoangBandc = from sp in this.db.SAN_PHAM.ToList()
                                join lsp in this.db.LOAI_SAN_PHAM.ToList()
                                on sp.ID_LSP equals lsp.ID_LOAI_SP
                                where sp.NOI_BAT == false
                                select new SP
                                {
                                    MaSP = sp.MA_SP,
                                    loaiSP = lsp.TEN_LOAI_SP,
                                    TenSP = sp.TEN_SP,
                                    LinkAnh = sp.LINK_ANH_CHINH,
                                    soLuong = (int)sp.SO_LUONG_TONG,
                                    donViTinh = sp.DON_VI_TINH,
                                    giaNhap = sp.GIA_NHAP,
                                    giaBan = sp.GIA_BAN,
                                    trangThai = (bool)sp.TRANG_THAI
                                };
            ViewBag._spKhoangBandc = spKhoangBandc.ToList();
            return View();
        }

    }
}