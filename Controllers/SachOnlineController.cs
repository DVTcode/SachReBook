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
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        private string connection;
        private dbSachOnlineDataContext data;
        public SachOnlineController()
        {
            // Khởi tạo chuỗi kết nối
            connection = "Data Source=DESKTOP-1Q9MS11\\SQLEXPRESS06;Initial Catalog=SachOnline;Integrated Security=True";
            data = new dbSachOnlineDataContext(connection);
        }
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        private List<SACH> LayTenChuDe(int iMaCD)
        {
            return data.SACHes.Where(s => s.MaCD == iMaCD).ToList();
        }
        private List<SACH> LayTenNXB(int iMaNXB)
        {
            return data.SACHes.Where(s => s.MaNXB == iMaNXB).ToList();
        }
        public ActionResult Index()
        {
                //Lay 6 quyen sach moi
                var listSachMoi = LaySachMoi(8);
                return View(listSachMoi);
        }
        public ActionResult Index2()
        {
            //Lay 6 quyen sach moi
            var listSachMoi = LaySachMoi(8);
            return View(listSachMoi);
        }
        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in data.CHUDEs select cd; 
            return PartialView(listChuDe);
        }
        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(listNhaXuatBan);
        }
        public ActionResult SachTheoChuDe(int id, int ? page)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;

            var sach = data.SACHes.Where(s => s.MaCD == id)
                                  .OrderByDescending(s => s.NgayCapNhat)
                                  .ToPagedList(pageNumber, pageSize);

            ViewBag.Id = id;
            ViewBag.ten = LayTenChuDe(id);

            return View(sach);
        }
        public ActionResult SachTheoNhaXuatBan(int id, int ? page)
        {
            int pageSize = 3;
            int pageNumber = page ?? 1;

            var sach = data.SACHes.Where(s => s.MaNXB == id)
                                  .OrderByDescending(s => s.NgayCapNhat)
                                  .ToPagedList(pageNumber, pageSize);

            ViewBag.Id = id;
            ViewBag.ten = LayTenNXB(id);

            return View(sach);
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = data.SACHes.Where(s => s.MaSach == id).SingleOrDefault();

            if (sach != null)
            {
                var binhLuans = data.Comments.Where(b => b.MaSach == id).ToList();
                var binhLuansModel = binhLuans.Select(c => new CommentModel
                {
                    IdBinhLuan = c.idBinhLuan,
                    UserName = c.UserName,
                    NoiDung = c.NoiDung,
                    NgayTao = (DateTime)c.NgayTao
                }).ToList();

                // Chuyển thông tin sách và bình luận đến View
                var chiTietModel = new ChiTietSachModel
                {
                    Sach = sach,
                    BinhLuans = binhLuansModel
                };

                return View(chiTietModel);
            }
            else
            {
                // Xử lý sách không tồn tại
                return HttpNotFound();
            }
        }
        
        public ActionResult Contact()
        {
          
            return View();
        }
        public ActionResult Details(int id)
        {
            var khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }
        public ActionResult BinhLuan(int id)
        {
            var comments = data.Comments.Where(c => c.MaSach == id).ToList();
            return View(comments);
        }
        [HttpPost]
        public ActionResult ThemBinhLuan(int MaSach, string NoiDung)
        {
            // Kiểm tra xem nội dung bình luận có rỗng không, nếu rỗng thì không thêm vào cơ sở dữ liệu
            if (string.IsNullOrEmpty(NoiDung))
            {
                // Xử lý thông báo hoặc chuyển hướng về trang chi tiết sách với thông báo lỗi
                return RedirectToAction("ChiTietSach", new { id = MaSach, loi = "Nội dung bình luận không được để trống." });
            }

            // Tạo một đối tượng BinhLuan mới
            var newComment = new Comment
            {
                UserName = "Khách", // Gán tên người dùng là "Khách" hoặc tùy chọn khác
                NoiDung = NoiDung,
                NgayTao = DateTime.Now,
                MaSach = MaSach // ID của sách đang được xem chi tiết
            };

            // Thêm bình luận vào cơ sở dữ liệu
            data.Comments.InsertOnSubmit(newComment);
            data.SubmitChanges();

            // Chuyển hướng về trang chi tiết sách
            return RedirectToAction("ChiTietSach", new { id = MaSach });
        }
    }
}



