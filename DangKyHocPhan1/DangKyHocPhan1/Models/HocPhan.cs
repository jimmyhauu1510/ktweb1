using System.ComponentModel.DataAnnotations;

namespace DangKyHocPhan1.Models
{
    public class HocPhan
    {
        [Key]
        public string MaHP { get; set; }
        public string TenHP { get; set; }
        public int SoTinChi { get; set; }
        public int SoLuong { get; set; }
    }
}