namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SAN_PHAM
    {
        [Key]
        [StringLength(20)]
        public string MA_SP { get; set; }

        public int ID_LSP { get; set; }

        [Required]
        [StringLength(250)]
        public string TEN_SP { get; set; }

        [Required]
        [StringLength(250)]
        public string SLUG { get; set; }

        [StringLength(255)]
        public string MO_TA { get; set; }

        [Column(TypeName = "ntext")]
        public string MO_TA_CHI_TIET { get; set; }

        [StringLength(250)]
        public string LINK_ANH_CHINH { get; set; }

        [Column(TypeName = "text")]
        public string LIST_ANH_KEM { get; set; }

        public int? SO_LUONG_TONG { get; set; }

        public decimal GIA_NHAP { get; set; }

        public decimal GIA_BAN { get; set; }

        public int? GIAM_GIA { get; set; }

        [StringLength(30)]
        public string DON_VI_TINH { get; set; }

        public int? LUOT_XEM { get; set; }

        public bool? TRANG_THAI { get; set; }

        public bool? NOI_BAT { get; set; }

        public DateTime? NGAY_TAO { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
