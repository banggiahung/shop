namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHI_TIET_HOA_DON
    {
        public int ID { get; set; }

        public int ID_HD { get; set; }

        public int ID_SP_CT { get; set; }

        [StringLength(80)]
        public string SP_SIZE_MAU { get; set; }

        [StringLength(30)]
        public string DON_VI_TINH { get; set; }

        public int? SL_MUA { get; set; }

        public decimal? GIA_BAN { get; set; }

        public bool? IS_REMOVE { get; set; }
    }
}
