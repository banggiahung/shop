namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LOAI_SAN_PHAM
    {
        [Key]
        public int ID_LOAI_SP { get; set; }

        [Required]
        [StringLength(250)]
        public string TEN_LOAI_SP { get; set; }

        [Required]
        [StringLength(250)]
        public string SLUG { get; set; }

        public bool? TRANG_THAI { get; set; }

        public int? ID_CHA { get; set; }

        public DateTime? NGAY_TAO { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
