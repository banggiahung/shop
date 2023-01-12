using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Object
{
    public class CartItem
    {
        public int DetailsProductID { set; get; }

        public string productID { set; get; }

        public int quantity { set; get; }

        public decimal GIA_BAN { get; set; }

        public string TEN_SP { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Image { get; set; }

        public string SLUG { get; set; }
    }
}