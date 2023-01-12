namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SAN_PHAM_CHI_TIET
    {
        public long ID { get; set; }

        public int ID_SIZE { get; set; }

        public int ID_COLOR { get; set; }

        [Required]
        [StringLength(20)]
        public string MA_SP { get; set; }

        [Required]
        [StringLength(80)]
        public string SLUG { get; set; }

        public int? SO_LUONG { get; set; }

        public bool? TRANG_THAI { get; set; }

        public DateTime? NGAY_TAO { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
