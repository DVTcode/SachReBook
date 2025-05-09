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
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Admin/DonHang
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(data.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult CreateDonDatHang()
        {
            ViewBag.MaKH = new SelectList(data.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            return View();
        }

        [HttpPost]
        public ActionResult CreateDonDatHang(DONDATHANG donDatHang, FormCollection f)
        {
            ViewBag.MaKH = new SelectList(data.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            if (ModelState.IsValid)
            {
                donDatHang.MaKH = int.Parse(f["MaKH"]);
                donDatHang.DaThanhToan = false; // Chưa thanh toán
                donDatHang.TinhTrangGiaoHang = Convert.ToInt32(f["TinhTrangGiaoHang"]);
                donDatHang.NgayDat = Convert.ToDateTime(f["NgayDat"]);
                donDatHang.NgayGiao = Convert.ToDateTime(f["NgayGiao"]);
                data.DONDATHANGs.InsertOnSubmit(donDatHang);
                data.SubmitChanges();

                // Chuyển hướng về trang Quản lý đơn đặt hàng
                return RedirectToAction("Index", "DonHang");
            }

            return View();
        }
        [HttpGet]
        public ActionResult EditDonDatHang(int id)
        {
            var donDatHang = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == id);
            if (donDatHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Hiển thị danh sách khách hàng và chọn khách hàng của đơn đặt hàng hiện tại
            ViewBag.MaKH = new SelectList(data.KHACHHANGs.ToList().OrderBy(kh => kh.HoTen), "MaKH", "HoTen", donDatHang.MaKH);

            return View(donDatHang);
        }

        [HttpPost]
        public ActionResult EditDonDatHang(DONDATHANG donDatHang, FormCollection f)
        {
            var existingDonDatHang = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == donDatHang.MaDonHang);
            if (existingDonDatHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaKH = new SelectList(data.KHACHHANGs.ToList().OrderBy(kh => kh.HoTen), "MaKH", "HoTen", existingDonDatHang.MaKH);

            if (ModelState.IsValid)
            {
                existingDonDatHang.MaKH = int.Parse(f["MaKH"]);
                existingDonDatHang.DaThanhToan = donDatHang.DaThanhToan;
                existingDonDatHang.TinhTrangGiaoHang = donDatHang.TinhTrangGiaoHang;
                existingDonDatHang.NgayDat = donDatHang.NgayDat;
                existingDonDatHang.NgayGiao = donDatHang.NgayGiao;

                data.SubmitChanges();

                return RedirectToAction("Index", "DonHang");
            }

            return View(existingDonDatHang);
        }
        [HttpGet]
        public ActionResult DeleteDonDatHang(int id)
        {
            var donDatHang = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == id);

            if (donDatHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(donDatHang);
        }

        [HttpPost, ActionName("DeleteDonDatHang")]
        public ActionResult DeleteDonDatHangConfirmed(int id)
        {
            var donDatHang = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == id);

            if (donDatHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra xem đơn đặt hàng đã được xử lý hay chưa
            if (donDatHang.DaThanhToan.HasValue && donDatHang.DaThanhToan.Value)
            {
                // Nếu đã thanh toán, hiển thị thông báo và không cho xóa
                ViewBag.ThongBao = "Đơn đặt hàng đã thanh toán, không thể xóa.";
                return View(donDatHang);
            }

            // Thực hiện xóa các chi tiết đặt hàng của đơn đặt hàng này
            var chiTietDonDatHangs = data.CHITIETDATHANGs.Where(ct => ct.MaDonHang == id).ToList();

            if (chiTietDonDatHangs.Count > 0)
            {
                data.CHITIETDATHANGs.DeleteAllOnSubmit(chiTietDonDatHangs);
                data.SubmitChanges();
            }

            // Sau đó xóa đơn đặt hàng
            data.DONDATHANGs.DeleteOnSubmit(donDatHang);
            data.SubmitChanges();

            return RedirectToAction("Index", "DonHang");
        }
    }
}