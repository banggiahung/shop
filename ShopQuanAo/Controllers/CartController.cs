using Models.EntityFramework;
using ShopQuanAo.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopQuanAo.Controllers
{
    public class CartController : Controller
    {
        //tạo tên session cho giỏ hàng
        private const string CartSession = "CartSession";
        private ShopQuanAoDBContext context;
        // GET: Cart
        public CartController()
        {
            this.context = new ShopQuanAoDBContext();
        }
        public ActionResult Index()
        {//hiển thị danh sách sản phẩm có trong giỏ hàng
            var listCartItem = new List<CartItem>();
            var cart = Session[CartSession];
            if(cart != null)
            {
                listCartItem = (List<CartItem>)cart;
            }
            return View(listCartItem);
        }

        //thêm mới sản phẩm vào trong giỏ hàng
        public ActionResult InsertCart(string id , FormCollection formCollection)
        {
            var quantity = int.Parse(formCollection["quantity"]);
            var idColor = formCollection["idColor"];
            var idSize = formCollection["idSize"];
            //lấy chi tiết của sản phẩm
            string sql = String.Format("SELECT * FROM SAN_PHAM_CHI_TIET WHERE ID_COLOR = {0} AND ID_SIZE = {1} AND MA_SP='{2}' ;", idColor , idSize , id);
            var productDetails = this.context.SAN_PHAM_CHI_TIET.SqlQuery(sql).FirstOrDefault();
            var cartItem = new CartItem();
            //thêm sản phẩm vào trong giỏ hàng
            if (productDetails != null)
            {
                var product = this.context.SAN_PHAM.Find(productDetails.MA_SP);
                cartItem.productID = product.MA_SP;
                cartItem.quantity = quantity;
                cartItem.GIA_BAN = product.GIA_BAN - (int)(product.GIAM_GIA * product.GIA_BAN)/100;
                cartItem.SLUG = product.SLUG;
                cartItem.Color = this.context.COLORs.Find(productDetails.ID_COLOR).TEN_MAU;
                cartItem.Size = this.context.SIZEs.Find(productDetails.ID_SIZE).TEN_SIZE;   
                cartItem.TEN_SP = product.TEN_SP;
                cartItem.Image = product.LINK_ANH_CHINH;
                cartItem.DetailsProductID = int.Parse( productDetails.ID.ToString());
                int soluongcon = ( productDetails.SO_LUONG ?? 0);
                if(soluongcon > 1)
                {
                    this.AddCart(cartItem, soluongcon);
                }
                else
                {
                    ViewBag.ErrorCart = "Lỗi không đủ số lượng để bán !!!";
                }
            }
            return RedirectToRoute("giohang", new { action ="Index"  });
        }
        //thêm vào cart khi có thì thay đổi số lượng, khi chưa có sản phẩm thì thêm một session
        private void AddCart(CartItem cartItem , int soluong)
        {
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.DetailsProductID == cartItem.DetailsProductID))
                {
                    var item = list.Where(c => c.DetailsProductID == cartItem.DetailsProductID).FirstOrDefault();
                    int sl_mua =  item.quantity + cartItem.quantity;
                    if(sl_mua <= soluong)
                    {
                        item.quantity = sl_mua;
                    }
                    else
                    {
                        ViewBag.ErrorCart = "Lỗi không đủ số lượng để bán !!!";
                        return;
                    }
                }
                else
                {
                    list.Add(cartItem);
                }

                Session[CartSession] = list;
            }
            else
            {
                var listItem = new List<CartItem>();
                listItem.Add(cartItem);
                Session[CartSession] = listItem;
            }
        }
        //xóa tất cả các phần tử trong giỏ hàng
        public ActionResult DeleteCart()
        {
            var cart = Session[CartSession];
            if(cart != null)
            {
                Session[CartSession] = null;
            }
            return RedirectToRoute("giohang", new { action = "Index" });
        }
        //xóa một sản phẩm trong giỏ
        public ActionResult Delete(int id)
        {
            var cart = Session[CartSession];
           
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if(list.Exists(c=>c.DetailsProductID == id))
                {
                    var cartitem = list.SingleOrDefault(c=>c.DetailsProductID == id);
                    list.Remove(cartitem);
                }
                Session[CartSession] = list;
            }
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //cập nhật thêm sản phẩm vào trogn giỏ
        public ActionResult Update(int id , int quantity)
        {
            var cart = Session[CartSession];

            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(c => c.DetailsProductID == id))
                {
                    var cartItem = list.SingleOrDefault(c => c.DetailsProductID == id);
                   
                    if (quantity == 0)
                    {
                        list.Remove(cartItem);
                    }
                    else
                    {
                        cartItem.quantity = quantity;
                    }
                }
                Session[CartSession] = list;
            }
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //thanh toán
        public ActionResult ThanhToan(FormCollection formCollection)
        {
            var cart = Session[CartSession];

            if (cart == null)
            {
                return RedirectToRoute("giohang",new { action = "Index" });
            }
            var list = (List<CartItem>)cart;
            if (list.Count == 0)
            {
                return RedirectToRoute("giohang", new { action = "Index" });
            }
            var name = formCollection["username"];
            var phone = formCollection["phone"];
            var diachi = formCollection["diachi"];
            var note = formCollection["note"];
            var order = new HOA_DON();
            order.GHI_CHU = note;
            order.IS_REMOVE = false;
            order.NGAY_MUA = DateTime.Now;
            order.SDT_NHAN = phone;
            order.TEN_NHAN_HANG = name;
            order.DIA_CHI_NHAN = diachi;
            order.TRANG_THAI = 1;
            order.TONG_TIEN = list.Sum(x => (x.GIA_BAN * x.quantity));
            var hd =  this.context.HOA_DON.Add(order);
            this.context.SaveChanges();
            foreach(var item in list)
            {
                var cthd = new CHI_TIET_HOA_DON();
                cthd.SL_MUA = item.quantity;
                cthd.SP_SIZE_MAU = item.TEN_SP + item.Size + item.Color;
                cthd.ID_SP_CT = item.DetailsProductID;
                cthd.DON_VI_TINH = (this.context.SAN_PHAM.SingleOrDefault(c => c.MA_SP == item.productID) != null) ? this.context.SAN_PHAM.SingleOrDefault(c => c.MA_SP == item.productID).DON_VI_TINH:"null";
                cthd.GIA_BAN = item.GIA_BAN;
                cthd.ID_HD = hd.MA_HD;
                cthd.IS_REMOVE = false;
                this.context.CHI_TIET_HOA_DON.Add(cthd);
                this.context.SaveChanges();
            }
            Session["ThanhToan"] = "Đặt hàng thành công";
            Session.Remove(CartSession);
            return RedirectToRoute("giohang", new { action = "Index" });
        }



    }
}