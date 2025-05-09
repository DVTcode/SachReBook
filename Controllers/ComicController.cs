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
    public class ComicController : Controller
    {
        private dbSachOnlineDataContext data;

        public ComicController()
        {
            string connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                                  Initial Catalog=SachOnline;
                                  Integrated Security=True;
                                  Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }

        public ActionResult Index()
        {
            var list = data.TruyenTranhs.OrderByDescending(t => t.NgayCapNhat).ToList();
            return View(list);
        }

        public ActionResult ChiTiet(int id)
        {
            var truyen = data.TruyenTranhs.SingleOrDefault(t => t.MaTruyen == id);
            var chapters = data.Chapters.Where(c => c.MaTruyen == id).OrderBy(c => c.STT).ToList();
            ViewBag.Chapters = chapters;
            return View(truyen);
        }

        public ActionResult DocChapter(int id)
        {
            var chapter = data.Chapters.SingleOrDefault(c => c.MaChapter == id);

            // Lấy danh sách trang truyện, sắp xếp theo STT
            var pages = data.TrangTruyens
                .Where(p => p.MaChapter == id)
                .OrderBy(p => p.STT)
                .ToList();

            // Chuẩn hóa đường dẫn ảnh nếu cần (nếu trong DB bạn lưu chỉ tên file, không có /Image/)
            foreach (var page in pages)
            {
                if (!string.IsNullOrEmpty(page.HinhAnh) && !page.HinhAnh.StartsWith("/Image/"))
                {
                    page.HinhAnh = "/Image/" + page.HinhAnh;
                }
            }

            ViewBag.Pages = pages;

            return View(chapter);
        }

    }

}



