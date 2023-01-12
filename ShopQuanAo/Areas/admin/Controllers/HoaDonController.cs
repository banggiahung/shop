using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DAL;
using Models.EntityFramework;
using ShopQuanAo.Data;
using Models.ADO;
using PagedList;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class HoaDonController : BaseController
    {
     
        private DAL_Bill dalBill { get; set; }

        public HoaDonController()
        {
            this.dalBill = new DAL_Bill();
        }

        // GET: admin/HoaDon
        //quản lý chung: hiển thị danh sách hóa đơn
        public ActionResult Index(int? page  , string search )
        {
            int pagenumber = (page ?? 1) - 1;
            var str_search = "";

            // ?search=1&t=hiep&s=0394599501&d=ba vì&p=1200000&g=đẹp&status=1
            if (!string.IsNullOrEmpty(search))
            {
                str_search += " TEN_NHAN_HANG LIKE '%" + search +"%'";  
            }
            //thông tin tìm kiếm được lưu vào trong viewbag để khi load trang dữ liệu cố định
            ViewBag.Search = search;
            int totalCount = 0;
            var listBill = this.dalBill.GetPageBills( str_search , pagenumber, 10, out totalCount);
            //phân trang sử dụng pagedlist
            IPagedList<ModelBills> pageOrders = new StaticPagedList<ModelBills>(listBill, pagenumber + 1, 10, totalCount);
            return View(pageOrders);
        }

        //thay đổi trạng thái
        public JsonResult changeStatus(int id , FormCollection collection)
        {
            var status =int.Parse( collection["status"]);
            var check = this.dalBill.ChangeStatus(id, status);
            return Json(new
            {
                status = check
            }, JsonRequestBehavior.AllowGet); 
        }
    }
}
