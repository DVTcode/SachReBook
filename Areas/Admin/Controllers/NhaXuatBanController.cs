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
    public class NhaXuatBanController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public NhaXuatBanController()
        {
            // Khởi tạo chuỗi kết nối
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Admin/NhaXuatBan
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(data.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult CreateNXB()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNXB(NHAXUATBAN nhaxuatban)
        {
            if (ModelState.IsValid)
            {
                // Thêm mới NHAXUATBAN vào cơ sở dữ liệu
                data.NHAXUATBANs.InsertOnSubmit(nhaxuatban);
                data.SubmitChanges();

                // Chuyển hướng về trang Index hoặc trang khác tùy vào yêu cầu của bạn
                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, hiển thị lại trang Create với các thông tin đã nhập và thông báo lỗi
            return View(nhaxuatban);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nhaXuatBan = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nhaXuatBan == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nhaXuatBan);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NHAXUATBAN nhaXuatBan)
        {
            var existingNhaXuatBan = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == nhaXuatBan.MaNXB);
            if (existingNhaXuatBan == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            if (ModelState.IsValid)
            {
                existingNhaXuatBan.TenNXB = nhaXuatBan.TenNXB;
                existingNhaXuatBan.DiaChi = nhaXuatBan.DiaChi;
                existingNhaXuatBan.DienThoai = nhaXuatBan.DienThoai;

                data.SubmitChanges();

                return RedirectToAction("Index");
            }

            return View(existingNhaXuatBan);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nhaXuatBan = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nhaXuatBan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhaXuatBan);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nhaXuatBan = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nhaXuatBan == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra nếu có các liên kết với bảng khác (ví dụ: sách) thì hiển thị thông báo
            var relatedData = data.SACHes.Where(s => s.MaNXB == id);
            if (relatedData.Count() > 0)
            {
                // Nội dung sẽ hiển thị khi nhà xuất bản cần xóa đã có trong các bảng khác
                ViewBag.ThongBao = "Nhà xuất bản này đang có trong các bảng khác <br>" + " Nếu muốn xóa thì phải xóa hết các liên kết của nó";
                return View(nhaXuatBan);
            }

            // Xóa nhà xuất bản
            data.NHAXUATBANs.DeleteOnSubmit(nhaXuatBan);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }


    }
}