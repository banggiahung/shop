using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EntityFramework;
using ShopQuanAo.Data;
using Models.DAL;
using PagedList;
using System.IO;
using ShopQuanAo.Areas.admin.Data;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class UsersController : BaseController
    {
        private ShopQuanAoContext db = new ShopQuanAoContext();

        // GET: admin/Users
        private DAL_Users dalUsers { get; set; }
        public UsersController()
        {
            this.dalUsers = new DAL_Users();
        }
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1) - 1;
            int totalCount = 0;
            var arrUsers = this.dalUsers.GetPageUsers(pagenumber, 10, out totalCount);
            IPagedList<ACOUNT> pageOrders = new StaticPagedList<ACOUNT>(arrUsers, pagenumber + 1, 10, totalCount);
            return View(pageOrders);
        }

        // GET: admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACOUNT aCOUNT = db.ACOUNTs.Find(id);
            if (aCOUNT == null)
            {
                return HttpNotFound();
            }
            return View(aCOUNT);
        }

        // GET: admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAT_KHAU, EMAIL,HO_TEN,PHONE,DIA_CHI")] ACOUNT aCOUNT)
        {
            try
            {
                aCOUNT.TRANG_THAI = Request.Form["HienThi"].Contains("true") ? true : false;
                aCOUNT.IS_REMOVE = false;
                var sql = "Select * from ACOUNT where EMAIL = N'" + aCOUNT.EMAIL + "';";
                var checksdt = this.dalUsers.sqlQueryFristOrDefault("Select * from ACOUNT where PHONE = N'" + aCOUNT.PHONE + "';");
                var checkemail = this.dalUsers.sqlQueryFristOrDefault(sql);
                if (ModelState.IsValid)
                {
                    if (Request.Files.Get("images").ContentLength == 0)
                    {
                        ViewBag.Err = "Images không được để trống !!!";
                        return View(aCOUNT);
                    }
                    
                    else
                    {
                        aCOUNT.LINK_ANH = SaveUploadImage(Request.Files.Get("images"));
                    }

                    if (aCOUNT.MAT_KHAU == null)
                    {
                        ViewBag.Err = "Mật khẩu không được để trống !!!";
                        return View(aCOUNT);
                    }

                    if (aCOUNT.MAT_KHAU.Length < 6 || aCOUNT.MAT_KHAU.Length >100)
                    {
                        ViewBag.Err = "Mật khẩu có độ dài từ 6 đến 100 ký tự !!!";
                        return View(aCOUNT);
                    }

                    if (checksdt)
                    {
                        ViewBag.Err = "Số điện thoại đã tồn tại !!!";
                        return View(aCOUNT);
                    }
                    if (checkemail)
                    {
                        ViewBag.Err = "Email đã tồn tại !!!";
                        return View(aCOUNT);
                    }
                    aCOUNT.MAT_KHAU = MaHoaMD5.GetHash(aCOUNT.MAT_KHAU);
                    if (!this.dalUsers.InsertUsers(aCOUNT))
                    {
                        ViewBag.Err = "Lỗi không thể chèn tài khoản !!!";
                        return View(aCOUNT);
                    }
                    else
                    {
                        ViewBag.Success = "Chèn Thành Công !!!";
                        return RedirectToAction("Index");
                    }


                }
                // TODO: Add insert logic here

                return View(aCOUNT);

            }
            catch
            {
                return View(aCOUNT);
            }
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

        private void DropFileImages(string strFile)
        {
            var file = Server.MapPath(strFile);
            System.IO.File.Delete(file);
        }

        // GET: admin/Users/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACOUNT aCOUNT = this.dalUsers.getUser(id);
            if (aCOUNT == null)
            {
                return HttpNotFound();
            }
            return View(aCOUNT);
        }

        // POST: admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID ,EMAIL ,DIA_CHI,HO_TEN,PHONE")] ACOUNT aCOUNT)
        {
            try
            {
                aCOUNT.TRANG_THAI = Request.Form["HienThi"].Contains("true") ? true : false;
                var sql = "Select * from ACOUNT where ID !='"+aCOUNT.ID+"' AND EMAIL = N'" + aCOUNT.EMAIL + "';";
                var checksdt = this.dalUsers.sqlQueryFristOrDefault("Select * from ACOUNT where ID !='" + aCOUNT.ID + "' AND PHONE = N'" + aCOUNT.PHONE + "';");
                var checkemail = this.dalUsers.sqlQueryFristOrDefault(sql);
                if(Request["password"] != "")
                {
                    aCOUNT.MAT_KHAU = MaHoaMD5.GetHash(Request["password"]);
                }
                if (ModelState.IsValid && checkemail == false)
                {
                    var str_file  = this.dalUsers.getUser(aCOUNT.ID).LINK_ANH;
                    if (Request.Files.Get("images").ContentLength == 0)
                    {
                        aCOUNT.LINK_ANH = str_file;
                    }
                    else
                    {
                        aCOUNT.LINK_ANH = SaveUploadImage(Request.Files.Get("images"));
                    }
                    if (checksdt)
                    {
                        ViewBag.Err = "Số điện thoại đã tồn tại !!!";
                        if(aCOUNT.LINK_ANH != str_file) { DropFileImages(aCOUNT.LINK_ANH); }
                        return View(aCOUNT);
                    }
                    if (checkemail)
                    {
                        ViewBag.Err = "Email đã tồn tại !!!";
                        if (aCOUNT.LINK_ANH != str_file) { DropFileImages(aCOUNT.LINK_ANH); }
                        return View(aCOUNT);
                    }
                    
                    if (!this.dalUsers.EditUsers(aCOUNT.ID , aCOUNT))
                    {
                        ViewBag.Err = "Lỗi không thể sửa tài khoản !!!";
                        if (aCOUNT.LINK_ANH != str_file) { DropFileImages(aCOUNT.LINK_ANH); }
                        return View(aCOUNT);
                    }
                    else
                    {

                        ViewBag.Success = "Sửa Thành Công !!!";
                       
                        return RedirectToAction("Index");
                    }


                }
                // TODO: Add insert logic here

                return View(aCOUNT);

            }
            catch
            {
                return View(aCOUNT);
            }
        }

        // GET: admin/Users/Delete/5
        public ActionResult Delete(int id)
        {
            var result = new DAL_Users().DeleteUser(id);

            return Json(new
            {
                status = (result) ? "success" : "error",
            }, JsonRequestBehavior.AllowGet);
        }

       

        
    }
}
