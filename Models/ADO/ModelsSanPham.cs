using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ADO
{
    public class ModelsSanPham
    {

        [Display(Name = "id")]
        [StringLength(20)]
        public string MA_SP { get; set; }

        public int ID_LSP { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "name product")]
        public string TEN_SP { get; set; }

        [Required]
        [Display(Name = "slug")]
        [StringLength(70)]
        public string SLUG { get; set; }

        [StringLength(255)]
        [Display(Name = "describe")]
        public string MO_TA { get; set; }

        [Required]
        [Display(Name = "detailed description")]
        public string MO_TA_CHI_TIET { get; set; }

       
        public string LINK_ANH_CHINH { get; set; }

        [StringLength(250)]
        public string LIST_ANH_KEM { get; set; }


        public int? SO_LUONG_TONG
        { get; set; }

        [Required]
        [Display(Name = "import price")]
        public decimal GIA_NHAP { get; set; }

        [Required]
        [Display(Name = "price")]
        public decimal GIA_BAN { get; set; }

        public int? GIAM_GIA
        { get; set; }

        [StringLength(30)]
        [Display(Name = "unti")]
        public string DON_VI_TINH { get; set; }

        public int? LUOT_XEM { get; set; }

        public bool? TRANG_THAI
        { get; set; }

        public bool? NOI_BAT
        { get; set; }

        public string TenLoaiSP
        {
            get;set;
        }
        public DateTime? NGAY_TAO
        { get; set; }

        public List<SanPhamDetails> listSanPhamDetails { get; set; }
        public List<ColorProduct> listSanPhamColor { get; set; }
    }
    public class SanPhamDetails
    {
        public long ID { get; set; }

        public int ID_SIZE { get; set; }

        public string TenSize { get; set; }

        public int ID_COLOR { get; set; }

        public string Image { get; set; }

        public string MoTaSize { get; set; }

        public string TenColor { get; set; }

        public string MA_SP { get; set; }

        public string SLUG { get; set; }

        public int? SO_LUONG { get; set; }

        public bool? TRANG_THAI { get; set; }

        public DateTime? NGAY_TAO { get; set; }
    }




    /// <summary>
    /// Lấy thông tin sản phẩm chi tiết theo màu color
    /// </summary>
    public class ColorProduct
    {
        public int ID { get; set; }

        public string TEN_MAU { get; set; }

        public string MA_MAU { get; set; }

        public string MA_SP { get; set; }

        public string Slug { get; set; }

        public string IMAGES { get; set; }

        public bool? TRANG_THAI { get; set; }

    }

}
