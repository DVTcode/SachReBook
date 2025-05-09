using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;

namespace SachOnline.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private string connection;
        private dbSachOnlineDataContext db;
        public UserController()
        {
            // Khởi tạo chuỗi kết nối
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
            db = new dbSachOnlineDataContext(connection);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTen"];

            var sTenDN = collection["TenDN"];

            var sMatkhau = collection["Matkhau"];

            var sMatkhauNhapLai = collection["MatKhauNL"];

            var sDiaChi = collection["DiaChi"];

            var sEmail = collection["Email"];

            var sDienThoai = collection["DienThoai"];

            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatkhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }

            else if (String.IsNullOrEmpty(sMatkhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatkhau != sMatkhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
       
                //Gần giá trị cho đối tượng được tạo mới (kh)

                kh.HoTen = sHoTen;

                kh.TaiKhoan = sTenDN;

                kh.MatKhau = sMatkhau;

                kh.Email = sEmail;

                kh.DiaChi = sDiaChi;

                kh.DienThoai = sDienThoai;

                kh.NgaySinh = DateTime.Parse(dNgaySinh);

                db.KHACHHANGs.InsertOnSubmit(kh);

                db.SubmitChanges();

                return RedirectToAction("DangNhap");

            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];

            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    Session["MaKH"] = kh.MaKH; // 🛠 Lưu MaKH vào Session
                    return RedirectToAction("Index", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult LoginLogoutPartial()
        {
            return PartialView("LoginLogoutPartial");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index2", "SachOnline");
        }
        [AllowAnonymous] // Đảm bảo người dùng đã đăng nhập
        public ActionResult UserProfile()
        {
            var user = (KHACHHANG)Session["TaiKhoan"]; // Lấy thông tin người dùng từ Session

            if (user != null)
            {
                var userModel = new UserModel
                {
                    HoTen = user.HoTen,
                    TaiKhoan = user.TaiKhoan,
                    Matkhau = user.MatKhau,
                    Email = user.Email,
                    DiaChi = user.DiaChi,
                    // Thêm các thuộc tính khác của người dùng
                };

                return View(userModel); // Truyền thông tin người dùng đến View
            }
            else
            {
                return RedirectToAction("DangNhap", "User"); // Chuyển hướng nếu không đăng nhập
            }
        }
    }
}