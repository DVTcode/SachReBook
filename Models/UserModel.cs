using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SachOnline.Models
{
    public class UserModel
    {
        public string HoTen { get; set; }
        public string TaiKhoan { get; set; }
        public string Matkhau { get; set; }
        public string Email { get; set; }

        public string DiaChi { get; set; }

        public string DienThoai { get; set; }
    }
}