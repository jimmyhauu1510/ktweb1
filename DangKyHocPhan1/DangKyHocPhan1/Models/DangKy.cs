using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangKyHocPhan1.Models
{
    public class DangKy
    {
        [Key]
        public int MaDK { get; set; }
        public DateTime NgayDK { get; set; }
        public string MaSV { get; set; }

        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }
    }
}