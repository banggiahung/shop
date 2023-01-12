namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SLIDE")]
    public partial class SLIDE
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string TIEU_DE { get; set; }

        [Required]
        [StringLength(250)]
        public string LINK { get; set; }

        [StringLength(250)]
        public string IMAGES { get; set; }

        public byte? STT { get; set; }

        public bool? TRANG_THAI { get; set; }

        public bool? NOI_BAT { get; set; }

        public DateTime? NGAY_DANG { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
