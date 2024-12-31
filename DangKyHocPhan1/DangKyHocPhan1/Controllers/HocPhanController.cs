using Microsoft.AspNetCore.Mvc;
using DangKyHocPhan1.Models;
using DangKyHocPhan1;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using DangKyHocPhan;

namespace DangKyHocPhan1.Controllers
{
    public class HocPhanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HocPhanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HocPhan/ListHP
        public async Task<IActionResult> ListHP()
        {
            var hocPhans = await _context.HocPhans.ToListAsync();
            return View(hocPhans);
        }

        // POST: HocPhan/DangKy
        [HttpPost]
        public async Task<IActionResult> DangKy(string maHP)
        {
            // Logic đăng ký học phần
            //  Lấy mã sinh viên từ session hoặc cookie, bạn cần tùy chỉnh theo cách bạn xác thực
            string maSV = HttpContext.Session.GetString("MaSV");

            if (string.IsNullOrEmpty(maSV))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Account");
            }

            if (!ModelState.IsValid)
            {
                //     return View(hocPhan);
                return BadRequest("Lỗi dữ liệu");
            }

            // kiểm tra sự tồn tại của học phần
            var hocPhan = await _context.HocPhans.FindAsync(maHP);
            if (hocPhan == null)
            {
                return NotFound();
            }
            // Kiểm tra xem sinh viên đã đăng ký học phần này chưa
            var dangKy = await _context.DangKys
              .Include(d => d.SinhVien)
               .Where(d => d.MaSV == maSV)
                .FirstOrDefaultAsync();
            if (dangKy != null && await _context.ChiTietDangKys.AnyAsync(ct => ct.MaDK == dangKy.MaDK && ct.MaHP == maHP))
            {
                return BadRequest("Học phần đã đăng ký");
            }
            // Tạo một đăng ký mới nếu chưa tồn tại
            if (dangKy == null)
            {
                dangKy = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Now
                };
                _context.DangKys.Add(dangKy);
                await _context.SaveChangesAsync();
            }
            //Tạo chi tiết đăng ký mới
            var chiTietDangKy = new ChiTietDangKy
            {
                MaDK = dangKy.MaDK,
                MaHP = maHP
            };
            _context.ChiTietDangKys.Add(chiTietDangKy);


            //  Giảm số lượng học phần
            hocPhan.SoLuong--;

            await _context.SaveChangesAsync();
            //  return RedirectToAction("GioHang","GioHang");
            return Ok("Đã thêm học phần vào giỏ hàng");
        }
    }
}