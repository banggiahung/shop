namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COLOR")]
    public partial class COLOR
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string TEN_MAU { get; set; }

        [StringLength(250)]
        public string SLUG { get; set; }

        [StringLength(250)]
        public string MA_MAU { get; set; }

        [StringLength(20)]
        public string MA_SP { get; set; }

        [StringLength(100)]
        public string IMAGES { get; set; }

        public bool? TRANG_THAI { get; set; }
    }
}
