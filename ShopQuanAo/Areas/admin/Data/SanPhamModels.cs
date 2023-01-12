using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Areas.admin.Data
{
    public class SanPhamModels
    {

        public string MA_SP { get; set; }

        public int ID_LSP { get; set; }


        public string TEN_SP { get; set; }


        public string SLUG { get; set; }


        public string MO_TA { get; set; }

        public string MO_TA_CHI_TIET { get; set; }

        public string LINK_ANH_CHINH { get; set; }

        public string LIST_ANH_KEM { get; set; }

        public int? SO_LUONG_TONG
        {
            set { this.SO_LUONG_TONG = 0; }
            get { return this.SO_LUONG_TONG; }
        }

        public decimal GIA_NHAP { get; set; }

        public decimal GIA_BAN { get; set; }

        public int? GIAM_GIA {
            set { this.GIAM_GIA = 0; }
            get { return this.GIAM_GIA; }
        }

        public string DON_VI_TINH { get; set; }

        public int? LUOT_XEM { get { return this.LUOT_XEM; } set { this.LUOT_XEM = 0; } }

        public bool? TRANG_THAI
        {
            set { this.TRANG_THAI = false; }
            get { return this.TRANG_THAI; }
        }

        public bool? NOI_BAT
        {
            set { this.NOI_BAT = false; }
            get { return this.NOI_BAT; }
        }

        public DateTime? NGAY_TAO
        {
            set { this.NGAY_TAO = DateTime.Now; }
            get { return this.NGAY_TAO; }
        }

        public List<SanPhamDetails> listSanPhamDetails{ get ; set;}
    }
    public class SanPhamDetails
    {
        public long ID { get; set; }

        public int ID_SIZE { get; set; }

        public string TenSize { get; set; }

        public int ID_COLOR { get; set; }

        public string Image { get; set; }

        public string TenColor { get; set; }

        public string MA_SP { get; set; }

        public string SLUG { get; set; }

        public int? SO_LUONG { get; set; }

        public bool? TRANG_THAI { get; set; }

        public DateTime? NGAY_TAO { get; set; }
    }
}