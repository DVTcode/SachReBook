using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;

namespace SachOnline.Controllers
{
    public class ReadBookController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public ReadBookController()
        {
            // Khởi tạo chuỗi kết nối
            connection = "Data Source=DESKTOP-1Q9MS11\\SQLEXPRESS06;Initial Catalog=SachOnline;Integrated Security=True";
            data = new dbSachOnlineDataContext(connection);
        }
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes
        .Where(s => s.MienPhi.HasValue && s.MienPhi.Value) // Kiểm tra s.MienPhi có giá trị và là true
        .OrderByDescending(a => a.NgayCapNhat)
        .Take(count)
        .ToList();
        }
        // GET: ReadBook
        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in data.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }
        public ActionResult QuanKhuNamDong()
        {
           
            return View();
        }
        public ActionResult ConHoang()
        {
            

            return View();
        }
        public ActionResult ChiecNgaiVang()
        {
           

            return View();
        }
        public ActionResult TuoiThoDuDoi()
        {
           

            return View();
        }
        public ActionResult CuocDoiNgoaiCua()
        {
            

            return View();
        }
        public ActionResult ChiEmKhacMe()
        {
         
            return View();
        }
    }
}