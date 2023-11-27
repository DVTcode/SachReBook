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
            connection = "Data Source=DESKTOP-1Q9MS11\\SQLEXPRESS06;Initial Catalog=SachOnline;Integrated Security=True";
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
    }

}