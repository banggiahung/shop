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
    public class ThongKeController : Controller
    {
        private ShopQuanAoDBContext db = new ShopQuanAoDBContext();

        // GET: admin/ThongKe
        public ActionResult Index()
        {
           

            var dtThang = from hoadon in this.db.HOA_DON.ToList()
                          join st in this.db.STATUS_HOA_DON.ToList() on hoadon.TRANG_THAI equals st.ID
                          
                          select new HoaD
                          {
                              id = hoadon.MA_HD,
                              tienhang = (double)hoadon.TONG_TIEN,
                              trangThai = st.STATUS_ORDER,
                              idTrangThai = st.ID
                          };
            ViewBag._dtThang = dtThang.ToList();




            return View();

        }
        public ActionResult DoanhThuNhanVien()
        {
            var doanhThuNV = from sp in this.db.CHI_TIET_HOA_DON.ToList()
                            join lsp in this.db.HOA_DON.ToList()
                            on sp.ID_HD equals lsp.MA_HD
                            select new HoaDon
                            {
                                tenNV = lsp.TEN_NHAN_HANG,
                                idHD = sp.ID_HD,
                                tienHang =  (double)lsp.TONG_TIEN,
                                idTrangThai = (int)lsp.TRANG_THAI,

                            };


            var rs = from hd in this.db.STATUS_HOA_DON.ToList()
                     join dt in doanhThuNV on hd.ID equals dt.idTrangThai
                     select new HoaDon
                     {
                         tenNV = dt.tenNV,
                         idHD = dt.idHD,
                         tienHang = dt.tienHang,
                         idTrangThai = dt.idTrangThai,
                         trangT = hd.STATUS_ORDER
                     };

            ViewBag._doanhThuNV = rs.ToList();
            return View();
        }



    }
}