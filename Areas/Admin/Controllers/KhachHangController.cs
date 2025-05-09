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
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
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
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var khachHang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Hiển thị các thông tin cần thiết cho dropdownlist hoặc các phần tử khác nếu cần
            // Ví dụ: ViewBag.MaLoaiKH = new SelectList(data.LOAIKHACHHANGs.ToList(), "MaLoaiKH", "TenLoaiKH", khachHang.MaLoaiKH);

            return View(khachHang);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(KHACHHANG khachHang)
        {
            var existingKhachHang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == khachHang.MaKH);
            if (existingKhachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Cập nhật thông tin từ form
            existingKhachHang.HoTen = khachHang.HoTen;
            existingKhachHang.TaiKhoan = khachHang.TaiKhoan;
            existingKhachHang.MatKhau = khachHang.MatKhau;
            existingKhachHang.Email = khachHang.Email;
            existingKhachHang.DiaChi = khachHang.DiaChi;
            existingKhachHang.DienThoai = khachHang.DienThoai;
            existingKhachHang.NgaySinh = khachHang.NgaySinh;

            // Cập nhật các trường khác nếu cần
            // Ví dụ: existingKhachHang.MaLoaiKH = khachHang.MaLoaiKH;

            if (ModelState.IsValid)
            {
                data.SubmitChanges();
                return RedirectToAction("Index"); // Chuyển hướng đến trang danh sách sau khi cập nhật thành công
            }

            // Hiển thị lại trang edit nếu có lỗi
            return View(existingKhachHang);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var khachHang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachHang);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var khachHang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra xem khách hàng có trong các bảng liên quan không trước khi xóa
            var donDatHangs = data.DONDATHANGs.Where(ddh => ddh.MaKH == id).ToList();
            if (donDatHangs.Count > 0)
            {
                // Nếu khách hàng có đơn đặt hàng, bạn có thể hiển thị thông báo hoặc xử lý khác
                ViewBag.ThongBao = "Khách hàng này đang có trong bảng Đơn đặt hàng. Hãy xóa hết đơn đặt hàng liên quan trước khi xóa khách hàng.";
                return View(khachHang);
            }

            // Xóa khách hàng nếu không có đơn đặt hàng liên quan
            data.KHACHHANGs.DeleteOnSubmit(khachHang);
            data.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}