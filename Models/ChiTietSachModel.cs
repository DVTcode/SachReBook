using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SachOnline.Models
{
    public class ChiTietSachModel
    {
        public SACH Sach { get; set; }

        public List<CommentModel> BinhLuans { get; set; }
        public string TenSach { get; set; }
    }
}