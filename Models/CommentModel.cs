using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SachOnline.Models
{
    public class CommentModel
    {
        public int IdBinhLuan { get; set; }
        public string UserName { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaSach { get; set; }
    }
}