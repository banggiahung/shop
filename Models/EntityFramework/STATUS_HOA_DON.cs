namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STATUS_HOA_DON
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string STATUS_ORDER { get; set; }
    }
}
