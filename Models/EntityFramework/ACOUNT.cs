namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ACOUNT")]
    public partial class ACOUNT
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string EMAIL { get; set; }

        [StringLength(250)]
        public string MAT_KHAU { get; set; }

        [StringLength(250)]
        public string LINK_ANH { get; set; }

        [Required]
        [StringLength(50)]
        public string HO_TEN { get; set; }

        [Required]
        [StringLength(12)]
        public string PHONE { get; set; }

        public DateTime? NGAY_DANG_KY { get; set; }

        [Required]
        [StringLength(250)]
        public string DIA_CHI { get; set; }

        public bool? TRANG_THAI { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
