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
    public class KhachHangController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public KhachHangController()
        {
            // Khởi tạo chuỗi kết nối
            connection = "Data Source=DESKTOP-1Q9MS11\\SQLEXPRESS06;Initial Catalog=SachOnline;Integrated Security=True";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(data.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // Đoạn code POST
        [HttpPost]
        [ValidateInput(false)] // Có thể bỏ đi nếu không cần thiết
        public ActionResult Create(KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                if (data.KHACHHANGs.Any(kh => kh.TaiKhoan == khachHang.TaiKhoan))
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại.");
                    return View(khachHang);
                }

                // Thêm khách hàng vào CSDL
                data.KHACHHANGs.InsertOnSubmit(new KHACHHANG
                {
                    HoTen = khachHang.HoTen,
                    TaiKhoan = khachHang.TaiKhoan,
                    MatKhau = khachHang.MatKhau,
                    Email = khachHang.Email,
                    DiaChi = khachHang.DiaChi,
                    DienThoai = khachHang.DienThoai,
                    NgaySinh = khachHang.NgaySinh
                });

                data.SubmitChanges();

                // Chuyển hướng người dùng về trang danh sách khách hàng hoặc trang khác tùy vào yêu cầu của bạn
                return RedirectToAction("Index", "KhachHang"); // "KhachHang" là tên Controller quản lý khách hàng
            }
            return View();
        }
    }
}