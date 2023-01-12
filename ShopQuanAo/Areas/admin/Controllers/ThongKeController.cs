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
                              idTrangThai = st.ID,
                              ngay = (DateTime)hoadon.NGAY_MUA,
                              
                          };
            var d  = dtThang.ToList();

            int[] arr = new int[d.Count];
            int i = 0;
            d.ForEach(t => {
                arr[i] = t.ngay.Month;
                i++;
            });
            ViewBag._dtThang = d;
            ViewBag.thang  = arr;


            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var item in d)
            {
                dataPoints.Add(new DataPoint(String.Format("{0:d}", item.ngay), item.tienhang));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();

        }
        public List<HoaD> sortByMoth(List<HoaD> hoaDs, int i)
        {
            if (i == null)
            {
                i = 1;
            }
            //return hoaDs.Where(i=> i.ngay.Month == i);
            var a =  from rs in hoaDs where rs.ngay.Month == i select new HoaD
            {
                id = rs.id,
                tienhang = rs.tienhang,
                trangThai = rs.trangThai,
                idTrangThai = rs.idTrangThai,
                ngay = rs.ngay,
            };

            return a.ToList();
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