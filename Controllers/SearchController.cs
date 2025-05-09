using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using SachOnline.Models;

namespace SachOnline.Controllers
{
    public class SearchController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        public SearchController()
        {
            // Khởi tạo chuỗi kết nối
            connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                      Initial Catalog=SachOnline;
                      Integrated Security=True;
                      Encrypt=False;";
            data = new dbSachOnlineDataContext(connection);
        }
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string strSearch)
        {
            ViewBag.Search = strSearch;
            if (string.IsNullOrEmpty(strSearch))
            {
                return View();
            }
            var kq = data.SACHes.Where(s => s.TenSach.Contains(strSearch));
            //var kq = data.SACHes.Where(s => s.MaCD == int.Parse(strSearch));
            //var kq = data.SACHes.Where(s => (s.SoLuongBan >= 5 && s.SoLuongBan <= 10));
            //var kq = data.SACHes.Where(s => (
            //    s.MaSach == int.Parse(strSearch) ||
            //    s.TenSach.Contains(strSearch) ||
            //    s.MoTa.Contains(strSearch) ||
            //    s.SoLuongBan == int.Parse(strSearch) ||
            //    s.MaCD == int.Parse(strSearch) ||
            //    s.MaNXB == int.Parse(strSearch)
            //));
            //var kq = data.SACHes.Where(s => (s.SoLuongBan >= 5 && s.SoLuongBan <= 10)).OrderBy(s => s.SoLuongBan);
            //var kq = data.SACHes.Where(s => (s.SoLuongBan >= 5 && s.SoLuongBan <= 10)).OrderByDescending(s => s.SoLuongBan);

            ViewBag.Kq = kq.Count();
            return View(kq);
        }
    }
    
}