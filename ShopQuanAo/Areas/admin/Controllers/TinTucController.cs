using Models.DAL;
using Models.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class TinTucController : BaseController
    {
        // GET: admin/TinTuc
        private DAL_TinTuc dalTinTuc { get; set; }
        public TinTucController()
        {
            this.dalTinTuc = new DAL_TinTuc();
        }
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1) - 1;
            int totalCount = 0;
            var listSlides = this.dalTinTuc.GetPageSlides(pagenumber, 10, out totalCount);
            IPagedList<BAI_VIET> pageOrders = new StaticPagedList<BAI_VIET>(listSlides, pagenumber + 1, 10, totalCount);
            return View(pageOrders);
        }

        // GET: admin/TinTuc/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        private void DropFileImages(string strFile)
        {
            var file = Server.MapPath(strFile);
            System.IO.File.Delete(file);
        }

        private string SaveUploadImage(HttpPostedFileBase file)
        {
            var str_file = "";
            Random random = new Random();
            if (file != null && file.ContentLength > 0)
            {
                //Định nghĩa đường dẫn lưu file trên server
                var ServerSavePath = Path.Combine(Server.MapPath("~/UploadFileImage/images/") + file.FileName);
                string newFileName = file.FileName;
                //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                while (System.IO.File.Exists(ServerSavePath) == true)
                {
                    string filename = Path.GetFileNameWithoutExtension(newFileName);
                    string extension = Path.GetExtension(newFileName);
                    newFileName = filename + "_" + random.Next(1, 1000) + extension;
                    ServerSavePath = Path.Combine(Server.MapPath("~/UploadFileImage/images/") + newFileName);
                }
                //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                file.SaveAs(ServerSavePath);

                str_file = "/UploadFileImage/images/" + newFileName;
            }
            return str_file;// trả về đường dẫn file trong thư mục
        }

        // GET: admin/TinTuc/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "SLUG,TIEU_DE,MO_TA,NOI_DUNG ")] BAI_VIET tintuc)
        {


            tintuc.NOI_BAT = Request.Form["NoiBat"].Contains("true") ? true : false ;
            tintuc.TRANG_THAI = Request.Form["HienThi"].Contains("true") ? true : false;
            var sql = "SELECT * FROM BAI_VIET WHERE SLUG = '"+tintuc.SLUG+"';";     
            if (ModelState.IsValid)
            {
                if (Request.Files.Get("images").ContentLength == 0)
                {
                    ViewBag.Err = "Images không được để trống !!!";
                    return View(tintuc);
                }
                else
                {
                    tintuc.IMAGES = SaveUploadImage(Request.Files.Get("images"));
                }

                var check = this.dalTinTuc.sqlQueryFristOrDefault(sql);
                if (check)
                {
                    ViewBag.Err = "Tiêu đề tin tức đã tồn tại !!!";
                    this.DropFileImages(tintuc.IMAGES);
                    return View(tintuc);
                }
                else
                {
                    if (this.dalTinTuc.CreateTinTuc(tintuc)) {

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Err = "Không thể chèn tin tức  !!!";
                        this.DropFileImages(tintuc.IMAGES);
                        return View(tintuc);

                    }

                }
            }

            return View(tintuc);
        }

     
        // GET: admin/TinTuc/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAI_VIET bAI_VIET = this.dalTinTuc.returnBaiViet(id);
            if (bAI_VIET == null)
            {
                return HttpNotFound();
            }
            return View(bAI_VIET);
        }

        // POST: admin/TinTuc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MA_BV,SLUG,TIEU_DE,MO_TA,NOI_DUNG ")] BAI_VIET tintuc)
        {

            try
            {

                tintuc.NOI_BAT = Request.Form["NoiBat"].Contains("true") ? true : false;
                tintuc.TRANG_THAI = Request.Form["HienThi"].Contains("true") ? true : false;

                var sql = "SELECT * FROM BAI_VIET WHERE MA_BV != '"+tintuc.MA_BV+"' AND SLUG = '" + tintuc.SLUG + "';";
                    if (ModelState.IsValid)
                    {
                        var old_img = this.dalTinTuc.returnBaiViet(tintuc.MA_BV).IMAGES;
                    if (Request.Files.Get("images").ContentLength == 0)
                        {
                        tintuc.IMAGES = old_img;
                        }
                        else
                        {
                            tintuc.IMAGES = SaveUploadImage(Request.Files.Get("images"));
                        }

                        var check = this.dalTinTuc.sqlQueryFristOrDefault(sql);
                        if (check)
                        {
                            ViewBag.Err = "Tiêu đề tin tức đã tồn tại !!!";
                            if (tintuc.IMAGES != old_img) { DropFileImages(tintuc.IMAGES); }
                            return View(tintuc);
                        }
                        else
                        {
                            if (this.dalTinTuc.EditTinTuc(tintuc.MA_BV ,tintuc))
                            {

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.Err = "Không thể sửa tin tức  !!!";
                            if (tintuc.IMAGES != old_img) { DropFileImages(tintuc.IMAGES); }
                            return View(tintuc);

                            }

                        }
                    }

                    return View(tintuc);
                
            }
            catch
            {
                return View();
            }
        }

        // GET: admin/TinTuc/Delete/5
        public ActionResult Delete(int id)
        {
            var result = new DAL_TinTuc().DeleteSlide(id);

            return Json(new
            {
                status = (result) ? "success" : "error",
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: admin/TinTuc/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
