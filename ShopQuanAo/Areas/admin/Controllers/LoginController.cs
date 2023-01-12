using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopQuanAo.Areas.admin.Data;
using Models.EntityFramework;
using Models.DAL;
using ShopQuanAo.Session;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class LoginController : Controller
    {
        public ShopQuanAoDBContext context { set; get; }
        public LoginController()
        {
            this.context = new ShopQuanAoDBContext();
        }
        // GET: admin/Login
        public ActionResult Index()
        {
            return View();
        }

        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var dal = new DAL_Users();
                userLogin.MatKhau = MaHoaMD5.GetHash(userLogin.MatKhau);
                var user = dal.getAcountEmail(userLogin.TaiKhoan);
                if (user != null && user.TRANG_THAI == true)
                {
                    if (user.MAT_KHAU == userLogin.MatKhau )
                    {
                        var userSession = new UserSession();
                        userSession.UserName = user.HO_TEN;
                        userSession.UserID = user.ID;
                        userSession.image = user.LINK_ANH;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        return RedirectToAction("Index", "TrangChu");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng!");
                    }
                }
             
                else
                    {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                /* else if (result == -2)
                 {
                     ModelState.AddModelError("", "Tài khoản đang bị khóa!");
                 }*/
            }
        
                return View("Index");
           
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(CommonConstants.USER_SESSION);
            return RedirectToAction("Index");
        }
    }
}