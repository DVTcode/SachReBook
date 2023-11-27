using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList.Mvc;
using PagedList;
using System.IO;

namespace SachOnline.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public DonHangController()
        {
            // Khởi tạo chuỗi kết nối
            connection = "Data Source=DESKTOP-1Q9MS11\\SQLEXPRESS06;Initial Catalog=SachOnline;Integrated Security=True";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Admin/DonHang
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(data.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
    }
}