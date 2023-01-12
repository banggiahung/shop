using Models.DAL;
using Models.EntityFramework;
using PagedList;
using ShopQuanAo.Areas.admin.Data;

using ShopQuanAo.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopQuanAo.Controllers
{
    public class HomeController : Controller
    {
        private ShopQuanAoDBContext context = null;

        public HomeController()
        {
            this.context = new ShopQuanAoDBContext();
        }

        private void Share()
        {
            var parentCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0);
            ViewBag.parentCategory = parentCategory;
        }

        [ChildActionOnly]
        //hiển thị danh mục sản phẩm cha
        public ActionResult LoadParentMenu()
        {
            var parentCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0 && a.IS_REMOVE == false && a.TRANG_THAI == true).ToList();
            return PartialView("_LoadParentMenu", parentCategory);
        }

        [ChildActionOnly]
        //hiển thị danh mục sản phẩm con
        public ActionResult LoadChilden( int parentID)
        {
            var childenCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == parentID && a.IS_REMOVE == false && a.TRANG_THAI == true).ToList();
            ViewBag.Count = childenCategory.Count();
            return PartialView("_LoadChilden", childenCategory);
        }


        [ChildActionOnly]
        //load sản phẩm phỏ biến
        public ActionResult LoadPopularProducts()
        {
            var arrCategory = new List<LOAI_SAN_PHAM>();
            var parentCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0 && a.TRANG_THAI==true && a.IS_REMOVE == false).ToList();

            /*if (parentCategory.Count() > 0)
            {
                foreach (var item in parentCategory)
                {
                    var listArrCategory = new List<LOAI_SAN_PHAM>();
                    listArrCategory = new DeQuyCategory().getChildenLoaiSanPham(listArrCategory , (int)item.ID_LOAI_SP);
                    if (listArrCategory.Count > 0)
                    {
                        foreach (var key in listArrCategory)
                        {
                            var arr = this.context.SAN_PHAM.Where(c => c.ID_LSP == key.ID_LOAI_SP && c.TRANG_THAI == true).ToList();
                            if (arr.Count() > 0)
                            {
                                arrCategory.Add(item);
                            }

                        }
                    }
                }
            }*/
            return PartialView("_LoadPopularProducts", parentCategory);
        }

        [ChildActionOnly]
        //load 
        public ActionResult LoadContentPopularProduct(int id)
        {
            var listProduct = new List<SAN_PHAM>();
            var listArrCategory = new List<LOAI_SAN_PHAM>();
            listArrCategory = new DeQuyCategory().getChildenLoaiSanPham(listArrCategory, id);

            if(listArrCategory.Count() > 0)
            {
                foreach(var item in listArrCategory)
                {
                    var arr = this.context.SAN_PHAM.Where(c => c.ID_LSP == item.ID_LOAI_SP && c.TRANG_THAI == true && c.IS_REMOVE == false).ToList();
                    if (arr.Count() > 0)
                    {
                        foreach(var i in arr)
                        {
                            listProduct.Add(i);
                        }
                    }
                }
            }
            return PartialView("_LoadContentPopularProduct" , listProduct);
        }

        [ChildActionOnly]
        public ActionResult LoadProductNew()
        {
            // Show 15 sản phẩm mới nhất
            var arrProduct = this.context.SAN_PHAM.Where(a => a.TRANG_THAI == true).OrderByDescending(c=>c.NGAY_TAO).Skip(0).Take(12);
            return PartialView("_LoadProductNew", arrProduct);
        }

       

        [ChildActionOnly]
        public ActionResult LoadPostsHot()
        {
            // Show 6 Bài Viết Nổi bật mới nhất trong trang Index
            var arrBaiViet = this.context.BAI_VIET.Where(a => a.TRANG_THAI == true && a.NOI_BAT == true).OrderByDescending(c => c.NGAY_DANG).Skip(0).Take(6);
            return PartialView("_LoadPostsHot", arrBaiViet);
        }
        [ChildActionOnly]
        public ActionResult LoadMenuTop()
        {
            var childenCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0 && a.IS_REMOVE==false && a.TRANG_THAI == true).Take(4).OrderBy(c=>c.NGAY_TAO).ToList();
           
            return PartialView("_LoadMenuTop", childenCategory);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var arrBaiViet = this.context.BAI_VIET.Where(a => a.TRANG_THAI == true ).OrderByDescending(c => c.NGAY_DANG).Skip(0).Take(3);
            var arrSlides = this.context.SLIDEs.Where(a => a.TRANG_THAI == true && a.IS_REMOVE==false).OrderBy(b => b.STT).OrderBy(c=>c.STT).ToList();
            ViewBag.ProductHot = this.context.SAN_PHAM.Where(a => a.TRANG_THAI == true && a.NOI_BAT == true).OrderByDescending(c => c.NGAY_TAO).Take(12).ToList();
            ViewBag.ListArticleHot = arrBaiViet;
            ViewBag.ListSlide = arrSlides;
            return View();
        }


        [ChildActionOnly]
        public ActionResult LoadMenuLeftDanhMuc(int parent_id , int id)
        {
            var dequy = new DeQuyCategory();
            var dm = new LOAI_SAN_PHAM();
            var data  = new List<LOAI_SAN_PHAM>();
            if(parent_id != 0)
            {
                dequy.getParentLoaiSanPham(data, parent_id);
                foreach (var i in data)
                {
                    if (i.ID_CHA == 0)
                    {
                        dm = i;
                        break;
                    }
                }
            }
            else
            {
                dm = this.context.LOAI_SAN_PHAM.Find(id);
            }
            ViewBag.MenuParent = dm;
            var listChildrenDanhMuc = this.context.LOAI_SAN_PHAM.Select(a=>a).ToList();
            return PartialView("_LoadMenuLeftDanhMuc", listChildrenDanhMuc);
        }

        [ChildActionOnly]
        public ActionResult LoadMenuLeftChildrenDanhMuc(int parentID)
        {
            var childenCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == parentID).ToList();
            ViewBag.CountChildrenDanhMuc = childenCategory.Count();
            return PartialView("_LoadMenuLeftChildrenDanhMuc", childenCategory);
        }

        [HttpGet]
        public ActionResult DanhMuc(string id , int? page , int pageSize = 12 , string price = null)
        {
            var danhmuc = this.context.LOAI_SAN_PHAM.Where(a => a.SLUG == id).FirstOrDefault();
            
            if (danhmuc == null || danhmuc.IS_REMOVE == true || danhmuc.TRANG_THAI == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var str_price = "";
            
            if (price != null)
            {
      
                if (price == "all")
                {
                    str_price = "";
                }
                else if(price =="50000000")
                {
                    str_price = "AND GIA_BAN >= 50000000";
                }
                else
                {
                    var arr = price.Split('-');
                    if(arr.Length == 2)
                    {
                        str_price = String.Format("AND GIA_BAN BETWEEN {0} AND {1};", arr[0] , arr[1]);
                    }
                    else
                    {
                        str_price = "";
                    }
                }
            }

            ViewBag.Search= price;
            ViewBag.Danhmuc = danhmuc;
            var listArrCategory = new List<LOAI_SAN_PHAM>();
            listArrCategory.Add(danhmuc);
            listArrCategory = new DeQuyCategory().getChildenLoaiSanPham(listArrCategory, (int)danhmuc.ID_LOAI_SP);
            var str = "";
            if(listArrCategory.Count() > 0)
            {
                var arr = new List<string>();
                foreach(var item in listArrCategory)
                {
                    arr.Add(item.ID_LOAI_SP.ToString());
                }
                str = string.Join(",", arr.ToArray());
            }
            int pagenumber = (page ?? 1);
            string sql = String.Format("Select * FROM SAN_PHAM WHERE SAN_PHAM.ID_LSP IN({0}) AND IS_REMOVE = 0 AND TRANG_THAI=1 {1}",str,str_price);
            var arrSanPham = this.context.SAN_PHAM.SqlQuery(sql).ToPagedList(pagenumber, pageSize);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(arrSanPham);
        }


        [ChildActionOnly]
        public ActionResult LoadColor(string id)
        {
            var arrColor = new List<COLOR>();
            arrColor = this.context.COLORs.Where(c => c.MA_SP == id).ToList();
            return PartialView("_LoadColor", arrColor);
        }

        [ChildActionOnly]
        public ActionResult LoadImageProductDetalis(string id)
        {
            var arrImg = new List<string>();
            var sp = this.context.SAN_PHAM.Find(id);
            if(sp != null)
            {
                arrImg.Add(sp.LINK_ANH_CHINH);
                foreach(var i in sp.LIST_ANH_KEM.Split(','))
                {
                    arrImg.Add(i);
                }
                var arrColor = this.context.COLORs.Where(c => c.MA_SP == id);
                foreach(var item in arrColor)
                {
                    arrImg.Add(item.IMAGES);
                }
            }
            ViewBag.ArrImage = arrImg;
            return PartialView("_LoadImageProductDetalis");
        }

        /* "/sanpham/ao-da-calic"*/

        [ChildActionOnly]
        public ActionResult LoadProductNoiBat()
        {
            var arrSanPham = this.context.SAN_PHAM.Where(a => a.NOI_BAT == true && a.TRANG_THAI == true && a.IS_REMOVE==false).OrderBy(c=>c.NGAY_TAO).Skip(0).Take(12);
            return PartialView("_LoadProductNoiBat", arrSanPham);
        }
        [HttpGet]
        public ActionResult SanPham(string id)
        {
            var product = this.context.SAN_PHAM.Where(a => a.SLUG == id).FirstOrDefault();
            if(product == null)
            {
                var id_product = this.context.SAN_PHAM_CHI_TIET.Where(a => a.SLUG == id).FirstOrDefault();
                if(id_product!= null)
                {
                    product = this.context.SAN_PHAM.Where(a => a.MA_SP == id_product.MA_SP).FirstOrDefault();
                }
            }
            if(product == null || product.IS_REMOVE == true || product.TRANG_THAI == false)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CategoryName = this.context.LOAI_SAN_PHAM.SingleOrDefault(c => c.ID_LOAI_SP == product.ID_LSP) != null ? this.context.LOAI_SAN_PHAM.SingleOrDefault(c => c.ID_LOAI_SP == product.ID_LSP).TEN_LOAI_SP : "";
            return View(product);
        }
        //ajax load size sản phẩm chi tiết
        // POST: admin/Size/Edit/5
        [HttpGet]
        public ActionResult LoadSize(int id)
        {
            var color = this.context.COLORs.Find(id);
            var arrSize = this.context.SIZEs.SqlQuery("Select * from size where id in (select ID_SIZE from SAN_PHAM_CHI_TIET where Ma_SP ='"+color.MA_SP+"' and ID_COLOR = "+color.ID+")").ToList();

            return Json(new
            {
                data = arrSize
            }, JsonRequestBehavior.AllowGet) ;
        }

        [HttpGet]
        public ActionResult BaiViet(int page = 1)
        {
            var arrBaiViet =this.context.BAI_VIET.Where(a=>a.TRANG_THAI== true && a.IS_REMOVE == false).OrderBy(c=>c.NGAY_DANG).ToPagedList(page , 12);
            return View(arrBaiViet);
        }

        [HttpGet]
        public ActionResult ChiTietBaiViet(string id)
        {
            var baiviet = this.context.BAI_VIET.Where(a => a.SLUG == id && a.IS_REMOVE == false).FirstOrDefault();
            if(baiviet == null)
            {
                return RedirectToAction("BaiViet", "Home");
            }
            return View(baiviet);
        }

        public ActionResult Search( string search = null, int page =1, int pageSize = 12, string price = null)
        {
            ViewBag.SearchPrice = price;
            ViewBag.Search = search;
            
            var str_price = "";

            if (price != null)
            {

                if (price == "all")
                {
                    str_price = "";
                }
                else if (price == "50000000")
                {
                    str_price = "AND GIA_BAN >= 50000000";
                }
                else
                {
                    var arr = price.Split('-');
                    if (arr.Length == 2)
                    {
                        str_price = String.Format("AND GIA_BAN BETWEEN {0} AND {1};", arr[0], arr[1]);
                    }
                    else
                    {
                        str_price = "";
                    }
                }
            }
            var str = (search != null) ? search : "";
          
            string sql = String.Format("Select * FROM SAN_PHAM WHERE SAN_PHAM.TEN_SP LIKE N'%{0}%' {1}", str, str_price);
            var arrSanPham = this.context.SAN_PHAM.SqlQuery(sql).ToPagedList(page, pageSize);
            return View(arrSanPham);
        }

       

        

       
        
      

    }
}