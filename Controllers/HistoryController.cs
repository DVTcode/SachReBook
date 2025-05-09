using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;

namespace SachOnline.Controllers
{
    public class HistoryController : Controller
    {
        private dbSachOnlineDataContext data;

        public HistoryController()
        {
            string connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                                  Initial Catalog=SachOnline;
                                  Integrated Security=True;
                                  Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }

        // Lấy lịch sử đọc sách của khách hàng
        private List<LichSuDocSach> LayLichSu(int maKH)
        {
            return data.LichSuDocSaches
                       .Where(ls => ls.MaKH == maKH)
                       .AsEnumerable()
                       .OrderByDescending(ls => ls.ThoiGianDoc)
                       .ToList();
        }

        // Hiển thị danh sách lịch sử đọc sách
        public ActionResult Index()
        {
            int maKH = (int)Session["MaKH"]; // Chắc chắn có giá trị vì khách hàng đã đăng nhập
            var lichSu = LayLichSu(maKH);
            ViewBag.MaKH = maKH;
            return View(lichSu);
        }


        // Thêm sách vào lịch sử đọc khi khách hàng đọc sách
        public void ThemLichSu(int maSach)
        {
            int? maKH = Session["MaKH"] as int?;
            if (maKH == null)
            {
                return; // Nếu chưa đăng nhập, không thêm vào lịch sử
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
    }
}
