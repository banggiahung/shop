using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ADO
{
    public class ModelBills
    {
        public int MA_HD { get; set; }

        public int ID_KH { get; set; }

        public string TEN_NHAN_HANG { get; set; }

        public DateTime? NGAY_MUA { get; set; }

        public string DIA_CHI_NHAN { get; set; }

        public string SDT_NHAN { get; set; }

        public decimal TONG_TIEN { get; set; }

        public int? TRANG_THAI { get; set; }

        public string GHI_CHU { get; set; }

        public string TenStatus { get; set; }

        public List<ModelDetailsBill> listDetalisBill { get; set; }
    }

    public class ModelDetailsBill {
        public int ID { get; set; }

        public int ID_HD { get; set; }

        public int ID_SP_CT { get; set; }

        public string SP_SIZE_MAU { get; set; }

        public string DON_VI_TINH { get; set; }

        public string Images { get; set; }
       
        public string tenColor { get; set; }
        
        public string tenSize { get; set; }

        public string tenSP { get; set; }

        public int SL_MUA { get; set; }

        public decimal GIA_BAN { get; set; }

    }

}
