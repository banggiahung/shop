
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
    public class SlidesController : BaseController
    {
        // GET: admin/Slides
        private DAL_Slides DalSlides { get; set; }
        public SlidesController()
        {
            this.DalSlides = new DAL_Slides();
        }
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1) - 1;
            int totalCount = 0;
            var listSlides = this.DalSlides.GetPageSlides(pagenumber, 10, out totalCount);
            IPagedList<SLIDE> pageOrders = new StaticPagedList<SLIDE>(listSlides, pagenumber + 1, 10, totalCount);
            return View(pageOrders);
        }

        // GET: admin/Slides/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: admin/Slides/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Slides/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TIEU_DE , LINK , STT")] SLIDE slides)
        {
            try
            {
                var sql = "Select * from SLIDE where TIEU_DE = N'" + slides.TIEU_DE + "';";
                var check = this.DalSlides.sqlQueryFristOrDefault(sql);
                if (ModelState.IsValid && check==false)
                { 
                    if(Request.Files.Get("images").ContentLength == 0)
                    {
                        ViewBag.Err = "Images không được để trống !!!";
                        return View(slides);
                    }
                    else
                    {
                        slides.IMAGES = SaveUploadImage(Request.Files.Get("images"));
                    }
                    if (Request.Form["is_active"] != null)
                    {
                        slides.TRANG_THAI = Boolean.Parse(Request.Form["is_active"]);
                    }
                    else
                    {
                        slides.TRANG_THAI = false;
                    }
                    if (!this.DalSlides.InsertSlides(slides))
                    {
                        ViewBag.Err = "Lỗi không thể chèn Slides !!!";
                        return View(slides);
                    }
                    else
                    {
                        ViewBag.Success = "Chèn Thành Công !!!";
                        return RedirectToAction("Index");
                    }
                    
                }
                // TODO: Add insert logic here
                else
                {
                    ViewBag.Err = "Tiêu đề Slides đã tồn tại";
                    return View(slides);
                }
            }
            catch
            {
                return View(slides);
            }
        }


        public string SaveUploadImage(HttpPostedFileBase file)
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

        // GET: admin/Slides/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLIDE slide = this.DalSlides.returnSlides(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            
            return View(slide);
        }

        // POST: admin/Slides/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID , TIEU_DE , LINK , STT")] SLIDE slides)
        {
            try
            {
                var sql = "Select * from SLIDE where id!='"+slides.ID+"' and  TIEU_DE = N'" + slides.TIEU_DE + "';";
                bool check = DalSlides.sqlQueryFristOrDefault(sql);
                if (Request.Form["is_active"] != null)
                {
                    slides.TRANG_THAI = Boolean.Parse(Request.Form["is_active"]);
                }
                if (ModelState.IsValid && check == false)
                {
                    if (Request.Files.Get("images").ContentLength == 0)
                    {
                        slides.IMAGES = this.DalSlides.returnSlides(slides.ID).IMAGES;
                    }
                    else
                    {
                        slides.IMAGES = SaveUploadImage(Request.Files.Get("images"));
                    }
                    if (!this.DalSlides.EditSlides(slides.ID , slides))
                    {
                        ViewBag.Err = "Lỗi không thể chèn Slides !!!";
                        return View(slides);
                    }
                    else
                    {
                        ViewBag.Success = "Sửa Thành Công !!!";
                        return RedirectToAction("Index");
                    }

                }
                // TODO: Add insert logic here
                else
                {
                    ViewBag.Err = "Tiêu đề Slides đã tồn tại";
                    return View(slides);
                }
            }
            catch
            {
                return View(slides);
            }
        }

        // GET: admin/Slides/Delete/5
        public ActionResult Delete(int id)
        {
            var result = new DAL_Slides().DeleteSlide(id);

            return Json(new
            {
                status = (result) ? "success" : "error",
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: admin/Slides/Delete/5
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
