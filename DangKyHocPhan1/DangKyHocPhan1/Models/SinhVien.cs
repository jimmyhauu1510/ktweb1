using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangKyHocPhan1.Models
{
    public class SinhVien
    {
        [Key]
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Hinh { get; set; }
        public string MaNganh { get; set; }

        [ForeignKey("MaNganh")]
        public NganhHoc NganhHoc { get; set; }
    }
}