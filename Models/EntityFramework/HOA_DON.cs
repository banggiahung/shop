namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HOA_DON
    {
        [Key]
        public int MA_HD { get; set; }

        [StringLength(50)]
        public string TEN_NHAN_HANG { get; set; }

        public DateTime? NGAY_MUA { get; set; }

        [StringLength(250)]
        public string DIA_CHI_NHAN { get; set; }

        [StringLength(15)]
        public string SDT_NHAN { get; set; }

        public decimal? TONG_TIEN { get; set; }

        public int? TRANG_THAI { get; set; }

        [StringLength(250)]
        public string GHI_CHU { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
