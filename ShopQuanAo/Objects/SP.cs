using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Runtime.Serialization;
namespace ShopQuanAo.Object
{
    public class SP
    {
        public string MaSP { set; get; }

        public string TenSP { set; get; }
        public string loaiSP { set; get; }

        public string LinkAnh { set; get; }

        public int soLuong { get; set; }

        public string donViTinh { get; set; }

        public decimal giaNhap { get; set; }

        public decimal giaBan { get; set; }

        public bool trangThai { get; set; }
        public int maNhanVien { get; set; }


    }
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}