using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangKyHocPhan1.Models
{
    public class ChiTietDangKy
    {
        [Key, Column(Order = 0)]
        public int MaDK { get; set; }
        [Key, Column(Order = 1)]
        public string MaHP { get; set; }

        [ForeignKey("MaDK")]
        public DangKy DangKy { get; set; }

        [ForeignKey("MaHP")]
        public HocPhan HocPhan { get; set; }
    }
}