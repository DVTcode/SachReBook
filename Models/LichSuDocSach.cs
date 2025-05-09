using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SachOnline.Models;
using System.Collections.Generic;
using System.Web;

namespace SachOnline.Models
{
    public partial class LichSuDocSach
    {
        [Key]
        public int IdLS { get; set; }


        /* [Required]: Thu?c tính KhachHangId b?t bu?c ph?i có giá tr? (không đư?c null). 
         * public int KhachHangId { get; set; }: Đ?i di?n cho ID c?a khách hàng đ? đ?c sách.
        */
                [Required]
        public int KhachHangId { get; set; }

        [Required]
        public int SachId { get; set; }

        public DateTime ThoiGianDoc { get; set; } = DateTime.Now; //public DateTime ThoiGianDoc { get; set; }: Đ?i di?n cho th?i đi?m khách hàng đ?c sách.= DateTime.Now;: M?c đ?nh s? l?y th?i gian hi?n t?i khi t?o m?t b?n ghi m?i.
        public string TinhTrangDoc { get; set; } = "Chưa đọc";

        [ForeignKey("KhachHangId")]
        public virtual KHACHHANG KhachHang { get; set; } // Cho phép truy c?p thông tin chi ti?t v? khách hàng có ID tương ?ng.

        [ForeignKey("SachId")] //[ForeignKey("SachId")]: Xác đ?nh SachId là khóa ngo?i liên k?t v?i b?ng Sach.
        public virtual Sach Sach { get; set; } //Cho phép truy c?p thông tin chi ti?t v? sách có ID tương ?ng.
    }
}