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
    public class HomeController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public HomeController()
        {
            // Khởi tạo chuỗi kết nối
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Admin/Home
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var stenDN = f["UserName"];
            var sMatkhau = f["Password"];
            if (String.IsNullOrEmpty(stenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatkhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gán giá trị cho đối tượng được tạo mới (ad)
                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.TenDN == stenDN && n.MatKhau == sMatkhau);
                if (ad != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TenDN"] = ad;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();

        }
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(data.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //Đưa dữ liệu vào DropDown
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fFileUpload == null)
            {
                //Nội dung thông báo yêu cầu chọn ảnh bìa ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                ViewBag.MienPhi = f["MienPhi"] == "on";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Image"), sFileName);
                    //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMoTa"];
                    sach.AnhBia = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]); sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    sach.MienPhi = f["MienPhi"] != null && f["MienPhi"].Contains("true");
                    // Kiểm tra xem checkbox được chọn hay không
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                    //Về lại trang Quản lý sách
                    return RedirectToAction ("Index");
                }
            }
            return View();
        }
        public ActionResult Details(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = data.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                // Nội dung sẽ hiển thị khi sách cần xóa đã có trong table ChiTietDonHang
                ViewBag.ThongBao = "Sách này đang có trong bảng Chi tiết đặt hàng <br>" + " Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";
            return View(sach);
            }
            //Xóa hết thông tin của cuốn sách trong table VietSach trước khi xóa sách này
            var vietsach = data.VIETSACHes. Where (vs => vs.MaSach == id).ToList(); 
            if (vietsach != null) 
            {
                data.VIETSACHes.DeleteAllOnSubmit(vietsach);
                data.SubmitChanges();
            }
            //Xóa sách
            data.SACHes.DeleteOnSubmit(sach); 
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Hiển thị danh sách chủ đề và nhà xuất bản đồng thời chọn chủ đề và nhà xuất bản của cuốn hiện tại
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SACH sach, HttpPostedFileBase fFileUpload)
        {
            var existingSach = data.SACHes.SingleOrDefault(n => n.MaSach == sach.MaSach);
            if (existingSach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", existingSach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", existingSach.MaNXB);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Image"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    existingSach.AnhBia = sFileName;
                }

                existingSach.TenSach = sach.TenSach;
                existingSach.MoTa = sach.MoTa;
                existingSach.NgayCapNhat = sach.NgayCapNhat;
                existingSach.SoLuongBan = sach.SoLuongBan;
                existingSach.GiaBan = sach.GiaBan;
                existingSach.MaCD = sach.MaCD;
                existingSach.MaNXB = sach.MaNXB;

                // Cập nhật trường MienPhi
                existingSach.MienPhi = sach.MienPhi;

                data.SubmitChanges();

                return RedirectToAction("Index");
            }

            return View(existingSach);
        }

    }
}

