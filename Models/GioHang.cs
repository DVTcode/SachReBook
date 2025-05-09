using System.Linq;
using SachOnline.Models;

public class GioHang
{
    dbSachOnlineDataContext db;

    public int iMaSach { get; set; }
    public string sTenSach { get; set; }
    public string sAnhBia { get; set; }
    public double dDonGia { get; set; }
    public int iSoLuong { get; set; }
    public double dThanhTien
    {
        get { return iSoLuong * dDonGia; }
    }

    // Constructor với chuỗi kết nối
    public GioHang(int ms)
    {
        string connection = @"Data Source=LAPTOP-7HTVH7CG\SQLEXPRESS;
                              Initial Catalog=SachOnline;
                              Integrated Security=True;
                              Encrypt=False;";
        db = new dbSachOnlineDataContext(connection);

        iMaSach = ms;
        SACH s = db.SACHes.Single(n => n.MaSach == iMaSach);
        sTenSach = s.TenSach;
        sAnhBia = s.AnhBia;
        dDonGia = double.Parse(s.GiaBan.ToString());
        iSoLuong = 1;
    }
}
