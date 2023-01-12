using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Areas.admin.Data
{
    public class JsonDetails
    {
        
            [JsonProperty("tenMau")]
            public string tenMau { get; set; }

            [JsonProperty("slug")]
            public string slug { get; set; }

            [JsonProperty("maMau")]
            public string maMau { get; set; }
            [JsonProperty("sizeQuantity")]
            public List<SizeQuantity> sizeQuantity{ get; set; }
        
    }

    public class SizeQuantity
    {
        [JsonProperty("size")]
        public string size { get; set; }

        [JsonProperty("quantity")]
        public string quantity { get; set; }
    }
    

}