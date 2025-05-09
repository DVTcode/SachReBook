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
    public class ChuDeController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public ChuDeController()
        {
            // Khởi tạo chuỗi kết nối
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Admin/ChuDe
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(data.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CHUDE chude)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên chủ đề đã tồn tại chưa
                if (data.CHUDEs.Any(c => c.TenChuDe == chude.TenChuDe))
                {
                    ModelState.AddModelError("TenChuDe", "Chủ đề đã tồn tại.");
                    return View(chude);
                }

                data.CHUDEs.InsertOnSubmit(chude);
                data.SubmitChanges();

                // Chuyển hướng người dùng về trang danh sách chủ đề hoặc trang khác tùy vào yêu cầu của bạn
                return RedirectToAction("Index", "ChuDe"); // "ChuDe" là tên Controller quản lý chủ đề
            }

            return View(chude);
        }
        [HttpGet]
        public ActionResult EditChuDe(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(chuDe);
        }

        [HttpPost]
        public ActionResult EditChuDe(CHUDE chuDe)
        {
            var existingChuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == chuDe.MaCD);
            if (existingChuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            if (ModelState.IsValid)
            {
                existingChuDe.TenChuDe = chuDe.TenChuDe;

                data.SubmitChanges();

                return RedirectToAction("Index"); // Chuyển hướng đến trang danh sách CHUDE sau khi chỉnh sửa
            }

            return View(existingChuDe);
        }
        [HttpGet]
        public ActionResult DeleteChuDe(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chuDe);
        }

        [HttpPost, ActionName("DeleteChuDe")]
        public ActionResult DeleteChuDeConfirm(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra xem CHUDE có liên quan đến bảng nào khác không
            // Nếu có, bạn có thể thêm điều kiện kiểm tra tại đây

            data.CHUDEs.DeleteOnSubmit(chuDe);
            data.SubmitChanges();

            return RedirectToAction("Index"); // Chuyển hướng về trang danh sách CHUDE sau khi xóa
        }
    }

}