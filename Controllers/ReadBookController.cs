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
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                          Initial Catalog=SachOnline;
                          Integrated Security=True;
                          Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }

        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes
                       .Where(s => s.MienPhi.HasValue && s.MienPhi.Value) // Lọc sách miễn phí
                       .OrderByDescending(a => a.NgayCapNhat)
                       .Take(count)
                       .ToList();
        }

        // Trang danh sách sách mới
        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }

        // Hiển thị chi tiết sách
        public ActionResult ChiTietSach(int id)
        {
            var sach = data.SACHes.SingleOrDefault(s => s.MaSach == id);
            if (sach == null)
            {
                return HttpNotFound();
            }

            // Ghi nhận lịch sử đọc sách
            ThemLichSu(id);

            return View(sach);
        }

        // Phương thức tự động lưu sách vào lịch sử đọc
        private void ThemLichSu(int maSach)
        {
            int? maKH = Session["MaKH"] as int?;
            if (maKH == null)
            {
                return; // Nếu chưa đăng nhập, không lưu vào lịch sử
            }

            var lichSuMoi = new LichSuDocSach
            {
                MaKH = maKH.Value,
                MaSach = maSach,
                ThoiGianDoc = DateTime.Now
            };

            data.LichSuDocSaches.InsertOnSubmit(lichSuMoi);
            data.SubmitChanges();
        }

        // Các trang nội dung riêng lẻ
        public ActionResult QuanKhuNamDong() { return View(); }
        public ActionResult ConHoang() { return View(); }
        public ActionResult ChiecNgaiVang() { return View(); }
        public ActionResult TuoiThoDuDoi() { return View(); }
        public ActionResult CuocDoiNgoaiCua() { return View(); }
        public ActionResult ChiEmKhacMe() { return View(); }
    }
}
