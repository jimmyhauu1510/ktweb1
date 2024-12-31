using System.ComponentModel.DataAnnotations;

namespace DangKyHocPhan1.Models
{
    public class NganhHoc
    {
        [Key]
        public string MaNganh { get; set; }
        public string TenNganh { get; set; }
    }
}