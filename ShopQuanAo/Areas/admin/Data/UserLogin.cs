using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Areas.admin.Data
{
    public class UserLogin
    {
        [Required]
        public string TaiKhoan { get; set; }
        [Required]
        [MinLength(6)]
        public string MatKhau { get; set; }
        public bool? Remember { get; set; }
    }
}