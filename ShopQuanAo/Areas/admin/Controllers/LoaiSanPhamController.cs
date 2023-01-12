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
using ShopQuanAo.Areas.admin.Data;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class LoaiSanPhamController : BaseController
    {
        private ShopQuanAoDBContext db = new ShopQuanAoDBContext();

        // GET: admin/LOAI_SAN_PHAM
        public ActionResult Index(int page = 1, string search = null)
        {
            var dalLSP = new DAL_LoaiSanPham();
            ViewBag.Search = search;
            var model = dalLSP.ListAllPaging(page, 5 , search);
            return View(model);
        }

        // GET: admin/LOAI_SAN_PHAM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_SAN_PHAM lOAI_SAN_PHAM = db.LOAI_SAN_PHAM.Find(id);
            if (lOAI_SAN_PHAM == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_SAN_PHAM);
        }

        // GET: admin/LOAI_SAN_PHAM/Create
        public ActionResult Create()
        {

            this.SetSelectOption();
            return View();
        }

        public void SetSelectOption(int? selected = null)
        {
            var ListSelectOption = new DeQuySelectOption().ListSelectOption().Where(c=>c.IS_REMOVE ==false).ToList() ;
            var lsp = new SelectOptionLoaiSanPham();
            lsp.ID_CHA = 0;
            lsp.TEN_LOAI_SP = " --- Không thuộc loại nào ---";
            ListSelectOption.Insert(0, lsp);
            ViewBag.MovieType = new SelectList(ListSelectOption, "ID_LOAI_SP", "TEN_LOAI_SP", selected);
        }

        // POST: admin/LOAI_SAN_PHAM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LOAI_SAN_PHAM lOAI_SAN_PHAM)
        {
            var DB = new DAL_LoaiSanPham();
            if (ModelState.IsValid)
            {
                var check = DB.Check_isset_danh_muc_insert(lOAI_SAN_PHAM.SLUG);
                if (check)
                {
                    lOAI_SAN_PHAM.IS_REMOVE = false;
                    bool insert = DB.insertLoaiSanPham(lOAI_SAN_PHAM);
                    if (check)
                    {
                        ViewBag.Success = "Thêm Thành Công !!! ";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Err = "Không chèn được ";
                        SetSelectOption();
                        return View(lOAI_SAN_PHAM);
                    }
                }
                else
                {
                    ViewBag.Err = "Loại sản phẩm đã tồn tại";
                    SetSelectOption();
                    return View(lOAI_SAN_PHAM);
                }
                
            }
            SetSelectOption();
            return View(lOAI_SAN_PHAM);
        }

        // GET: admin/LOAI_SAN_PHAM/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.SetSelectOption(id);

            LOAI_SAN_PHAM lOAI_SAN_PHAM = db.LOAI_SAN_PHAM.Find(id);

            if (lOAI_SAN_PHAM == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_SAN_PHAM);
        }

        // POST: admin/LOAI_SAN_PHAM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LOAI_SAN_PHAM lOAI_SAN_PHAM)
        {
            if (ModelState.IsValid)
            {
                var DB = new DAL_LoaiSanPham();
                var check = DB.Check_isset_danh_muc_update(lOAI_SAN_PHAM.ID_LOAI_SP ,lOAI_SAN_PHAM.SLUG);
                if (check)
                {
                    bool insert = DB.updateLoaiSanPham(lOAI_SAN_PHAM.ID_LOAI_SP , lOAI_SAN_PHAM);
                    if (insert)
                    {
                        ViewBag.Success = "Sửa Thành Công !!! ";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Err = "Không sửa được ";
                        SetSelectOption((int)lOAI_SAN_PHAM.ID_CHA);
                        return View(lOAI_SAN_PHAM);
                    }
                }
                else
                {
                    ViewBag.Err = "Loại sản phẩm đã tồn tại";
                    SetSelectOption((int)lOAI_SAN_PHAM.ID_CHA);
                    return View(lOAI_SAN_PHAM);
                }

                return RedirectToAction("Index");
            }
            SetSelectOption(lOAI_SAN_PHAM.ID_CHA);
            return View(lOAI_SAN_PHAM);
        }

        // GET: admin/LOAI_SAN_PHAM/Delete/5
       /* public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_SAN_PHAM lOAI_SAN_PHAM = db.LOAI_SAN_PHAM.Find(id);
            if (lOAI_SAN_PHAM == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_SAN_PHAM);
        }
*/

        public JsonResult Delete(int id)
        {
            var result = new DAL_LoaiSanPham().DeleteLoaiSanPham(id);

            return Json(new
            {
                status = (result) ? "success" : "error",
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int id)
        {
            var result = new DAL_LoaiSanPham().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }


        // POST: admin/LOAI_SAN_PHAM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LOAI_SAN_PHAM lOAI_SAN_PHAM = db.LOAI_SAN_PHAM.Find(id);
            db.LOAI_SAN_PHAM.Remove(lOAI_SAN_PHAM);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
