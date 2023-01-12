using System.Web.Mvc;

namespace ShopQuanAo.Areas.admin
{
    public class adminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            

            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller ="TrangChu", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "edit-details-product",
                "admin/sanpham/{action}/{masp}/{id}",
                new { controller = "SanPham", action = "Index", masp = UrlParameter.Optional, id = UrlParameter.Optional }
            );
        }
    }
}