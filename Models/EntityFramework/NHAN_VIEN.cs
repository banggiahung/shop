namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NHAN_VIEN
    {
        [Key]
        public int MA_NV { get; set; }
        public string TEN_NHAN_VIEN { get; set; }
    }
}