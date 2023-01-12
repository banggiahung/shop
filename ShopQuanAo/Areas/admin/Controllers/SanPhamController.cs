using Models.ADO;
using Models.DAL;
using Models.EntityFramework;
using Newtonsoft.Json;
using PagedList;
using ShopQuanAo.Areas.admin.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class SanPhamController : BaseController
    {
        // GET: admin/SanPham
        protected ShopQuanAoDBContext db = new ShopQuanAoDBContext();
        protected DAL_LoaiSanPham DalLSP { get; set; }
        protected DAL_Color DalColor { get; set; }
        protected DAL_SanPham DalSP { get; set; }
        protected DAL_Size DalSize { get; set; }
        protected DAL_SanPhamChiTiet DalSPCT { get; set; }

        public SanPhamController()
        {
            this.DalColor = new DAL_Color();
            this.DalSize = new DAL_Size();
            this.DalSP = new DAL_SanPham();
            this.DalSPCT = new DAL_SanPhamChiTiet();
            this.DalLSP = new DAL_LoaiSanPham();
        }

        public ActionResult Index(int ?page , string search = null)
        {
            int pagenumber = (page ?? 1) - 1; 
            int totalCount = 0;
            ViewBag.Search = search;
            var listSanPham = this.DalSP.GetPageSanPham(pagenumber, 5, out totalCount , search);
            IPagedList<ModelsSanPham> pageOrders = new StaticPagedList<ModelsSanPham>(listSanPham, pagenumber + 1, 5, totalCount);
            return View(pageOrders);
        }

        public void setSelectOptionSize(int? selected = null)
        {
            var listSize = this.DalSize.getDanhSach();
            ViewBag.ListSize = new SelectList(listSize, "ID", "TEN_SIZE", selected);
        }

        //Ajax form create detail Sản phẩm :
        public JsonResult ajaxSelectOptionSize()
        {
            var listSize = this.DalSize.getDanhSach(); 
            return Json(new
            {
                arrSize = listSize
            }, JsonRequestBehavior.AllowGet);
        }

        //Ajax form create Sản phẩm :
        public JsonResult ValidateInsertSlug(FormCollection collection)
        {
           var key = collection["key"];
            var value = collection["value"];
            var sql = "SELECT * FROM SAN_PHAM WHERE " + key + " = '" + value + "';";
            var result = this.DalSP.checkQueryFirstOrDefault(sql);
            return Json(new
            {
                status = result
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: admin/SanPham/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }

        // GET: admin/SanPham/Create
        public ActionResult Create()
        {
            this.SetSelectOptionLSP();
            this.setSelectOptionSize();
            return View();
        }

        public void SetSelectOptionLSP(int? selected = null)
        {
            var ListSelectOption = new DeQuySelectOption().ListSelectOption().Where(c=>c.IS_REMOVE ==false).ToList();
            ViewBag.MovieType = new SelectList(ListSelectOption, "ID_LOAI_SP", "TEN_LOAI_SP", selected);
        }


        // POST: admin/SanPham/Create  => Sử dụng Ajax
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                var sanPham = new SAN_PHAM();
                sanPham.MA_SP = collection["ma-sp"];
                sanPham.ID_LSP = int.Parse(collection["loai-san-pham"]);
                sanPham.TEN_SP = collection["ten-sp"];
                sanPham.SLUG = collection["slug"];
                sanPham.DON_VI_TINH = collection["don-vi-tinh"];
                sanPham.MO_TA = collection["mo-ta"];
                sanPham.MO_TA_CHI_TIET = collection["mo-ta-chi-tiet"];
                sanPham.GIAM_GIA = int.Parse(collection["giam-gia"]);
                sanPham.GIA_BAN = decimal.Parse(collection["gia-ban"]);
                sanPham.GIA_NHAP = decimal.Parse(collection["gia-nhap"]);
                sanPham.LINK_ANH_CHINH = this.SaveUploadImage(Request.Files["anh-chinh"]);
                sanPham.NOI_BAT = Boolean.Parse(collection["noi-bat"]);
                sanPham.TRANG_THAI = Boolean.Parse(collection["hien-thi"]); 
                var str_list_anh = "";
                foreach (var file in Request.Files.GetMultiple("list-anh"))
                {
                    str_list_anh += SaveUploadImage(file) + ",";
                }
                sanPham.LIST_ANH_KEM = str_list_anh.Trim(',');
                sanPham.NGAY_TAO = DateTime.Now;
                sanPham.LUOT_XEM = 0;
                sanPham.SO_LUONG_TONG = 0;

                if (this.DalSP.insertSanPham(sanPham))
                {
                    var jsonDetails = JsonConvert.DeserializeObject<List<JsonDetails>>(collection["details"]);
                    var imageColor = Request.Files.GetMultiple("anh-mau");

                    for(var i = 0; i < jsonDetails.Count; i++)
                    {
                        var color = new COLOR();
                        color.MA_MAU = jsonDetails[i].maMau;
                        color.TEN_MAU = jsonDetails[i].tenMau;
                        color.IMAGES = SaveUploadImage(imageColor[0]);
                        color.SLUG = jsonDetails[i].slug;
                        color.MA_SP = sanPham.MA_SP;
                        var id = this.DalColor.insertColor(color);

                        foreach (var item in jsonDetails[i].sizeQuantity)
                        {

                            var spct = new SAN_PHAM_CHI_TIET();
                            spct.ID_COLOR = id;
                            spct.SO_LUONG =int.Parse( item.quantity);
                            spct.ID_SIZE = int.Parse(item.size);
                            spct.NGAY_TAO = DateTime.Now;
                            spct.TRANG_THAI = false;
                            spct.SLUG = sanPham.SLUG + "-" + color.SLUG + "-" + this.DalSize.returnSize(int.Parse(item.size)).SLUG;
                            spct.MA_SP = sanPham.MA_SP;
                            var check = this.DalSPCT.insertSanPhamChiTiet(spct);
                        }

                    }

                    return Json(new
                    {
                        status = "success"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    this.DropFileImages(sanPham.LINK_ANH_CHINH);
                    foreach(var i in str_list_anh.Split(','))
                    {
                        this.DropFileImages(i);
                    }
                    return Json(new
                    {
                        status = "error"
                    }, JsonRequestBehavior.AllowGet);
                }
                /// thông tin chi tiết từng màu - size : 
            }
            catch
            {
                return Json(new
                {
                    status = "error"
                }, JsonRequestBehavior.AllowGet);
            }
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

       
        [HttpGet]

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAN_PHAM sAN_PHAM = this.DalSP.ReturnSAN_PHAM(id.Trim());
            
            if (sAN_PHAM == null)
            {
                return HttpNotFound();
            }
            this.setSelectOptionSize();
            this.SetSelectOptionLSP(sAN_PHAM.ID_LSP);
            ViewBag.DropDownColor = this.DalColor.getColorMasp(id.Trim());
            ViewBag.Message = TempData["status-edit"];
            ViewBag.StatusCreate = TempData["status-create"];
            return View(sAN_PHAM);
        }

        //Ajax
        [Route("edit-details-product")]
        public JsonResult getForChangeColor( string masp , int id)
        {
            var color = this.DalColor.returnColor(id);
            var arrSp = this.DalSPCT.getMaspAndIdColor(masp, id);
            return Json(new
            {
                arrColor = color,
                arrSizeQuantity = arrSp,
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: admin/SanPham/Edit/5
        
       
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_SP,ID_LSP,TEN_SP,SLUG,MO_TA,MO_TA_CHI_TIET,LINK_ANH_CHINH,GIA_NHAP,GIA_BAN,GIAM_GIA,DON_VI_TINH")] SAN_PHAM sAN_PHAM)
        {
            this.setSelectOptionSize();
            ViewBag.DropDownColor = this.DalColor.getColorMasp(sAN_PHAM.MA_SP);
            this.SetSelectOptionLSP(sAN_PHAM.ID_LSP);
            sAN_PHAM.LIST_ANH_KEM = "";
            sAN_PHAM.LINK_ANH_CHINH = "";
            sAN_PHAM.NOI_BAT = Request.Form["NoiBat"].Contains("true") ? true : false;
            sAN_PHAM.TRANG_THAI = Request.Form["HienThi"].Contains("true") ? true : false;
            if (ModelState.IsValid)
            {
                var Sql = "Select * from SAN_PHAM where MA_SP!='"+sAN_PHAM.MA_SP+"' AND SLUG = '"+sAN_PHAM.SLUG+"'";
                if (!this.DalSP.checkQueryFirstOrDefault(Sql))
                {
                    sAN_PHAM.LINK_ANH_CHINH = (Request.Files.Get("product-images").ContentLength > 0 ) ? SaveUploadImage(Request.Files.Get("product-images")) :this.DalSP.getSanPham(sAN_PHAM.MA_SP).LINK_ANH_CHINH;
                    string str_list_anh = "";
                    foreach (var file in Request.Files.GetMultiple("product-list-images"))
                    {
                        str_list_anh += SaveUploadImage(file) + ",";
                    }
                    sAN_PHAM.LIST_ANH_KEM = (str_list_anh.Trim(',')!="")?str_list_anh.Trim(','): this.DalSP.getSanPham(sAN_PHAM.MA_SP).LIST_ANH_KEM;

                    if(this.DalSP.UpdateSanPham(sAN_PHAM.MA_SP , sAN_PHAM))
                    {

                        ViewBag.Status = "Success";
                        return View(this.DalSP.ReturnSAN_PHAM(sAN_PHAM.MA_SP));
                    }
                    else
                    {
                        ViewBag.Status = "Error";
                        return View(sAN_PHAM);
                    }
                    
                    
                }
                else
                {
                    ViewBag.Status = "Sản Phẩm đã tồn tại";
                    return View(sAN_PHAM);
                }
                
            }
            return View(sAN_PHAM);
        }


        /// hàm method post sửa màu và hình ảnh , thông tin sản phẩm theo màu
        [HttpPost]
        [Route("edit-details-product")]
        public ActionResult editDetails(string masp , int id , FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
               
                var color = new COLOR();
                color.TEN_MAU = collection["ten-mau-edit"];
                color.SLUG = collection["slug-mau-edit"];
                color.MA_SP = masp;
                color.MA_MAU = collection["ma-mau-edit"];
                color.IMAGES = (Request.Files["anh-mau-edit"].ContentLength > 0)?SaveUploadImage(Request.Files[0]):"";
                var arrSize = collection["size-edit"].Split(',');
                var arrSoLuong = collection["quantity-edit"].Split(',');
                var arrIdSpct = collection["id_spct"];
                
                if (this.DalColor.updateColor(id , color))
                {
                    for(var i = 0; i < arrSize.Length; i++)
                    {
                        var spct = new SAN_PHAM_CHI_TIET();
                        spct.ID = int.Parse(arrIdSpct);
                        spct.MA_SP = masp;
                        spct.ID_SIZE = int.Parse(arrSize[i]);
                        spct.SO_LUONG = int.Parse(arrSoLuong[i]);
                        spct.ID_COLOR = id;
                        spct.SLUG = this.DalSP.ReturnSAN_PHAM(masp).SLUG;
                        var result = this.DalSPCT.updateSPCT(int.Parse(arrIdSpct), spct);
                    }
                    TempData["status-edit"] = "Sửa thành công !!!";
                }
                else
                {
                    TempData["status-edit"] = "Sửa Không Thành Công !!!";
                }
                return RedirectToAction("Edit" , "SanPham" , new { id = masp });

            }
            catch
            {
                return RedirectToAction("Edit", "SanPham", new { id = masp });
            }
        }


        //  hàm method post tạo mới 1 màu - hình ảnh - size - trong form (edit)
        [HttpPost]
        public ActionResult CreateDetails(string id, FormCollection collection)
        {
            var check = this.DalColor.checkIssetInsertColorMasp(id, collection["slug-create"]);
            if (check)
            {
                TempData["status-create"] = "Màu sản phẩm đã tồn tại !!!";
                return RedirectToAction("Edit", "SanPham", new { id = id });
            }
            else
            {
                var arrSize = collection["size-create"].Split(',');
                var arrSL = collection["quantity-create"].Split(',');
                var color = new COLOR();
                color.IMAGES = SaveUploadImage(Request.Files.Get("image-color-create"));
                color.MA_MAU = collection["ma-mau-create"];
                color.TEN_MAU = collection["ten-mau-create"];
                color.TRANG_THAI = true;
                color.SLUG = collection["slug-create"];
                color.MA_SP = id;
                var idColor = this.DalColor.insertColor(color);
                try
                {
                    for (var i = 0; i < arrSize.Length; i++)
                    {
                        var spct = new SAN_PHAM_CHI_TIET();
                        spct.ID_COLOR = idColor;
                        spct.MA_SP = id;
                        spct.ID_SIZE = int.Parse(arrSize[i]);
                        spct.SO_LUONG = int.Parse(arrSL[i]);
                        spct.SLUG = this.DalSP.ReturnSAN_PHAM(id).SLUG;
                        spct.TRANG_THAI = true;
                        spct.NGAY_TAO = DateTime.Now;
                        this.DalSPCT.insertSanPhamChiTiet(spct);
                    }
                }
                catch (Exception ex)
                {
                    db.COLORs.Remove(db.COLORs.Find(idColor));
                }

                TempData["status-create"] = "success";
                return RedirectToAction("Edit", "SanPham", new { id = id });
            }

        }

        // GET: admin/SanPham/Delete/5
        public ActionResult Delete(string id)
        {
            var result = new DAL_SanPham().deleteSP(id);

            return Json(new
            {
                status = (result) ? "success" : "error",
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
