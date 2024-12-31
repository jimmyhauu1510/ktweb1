using Microsoft.AspNetCore.Mvc;
using DangKyHocPhan1.Models;
using DangKyHocPhan1;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DangKyHocPhan;


namespace DangKyHocPhan1.Controllers
{
    public class DangKyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangKyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DangKy/GioHang
        public async Task<IActionResult> GioHang()
        {
            string maSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var dangKy = await _context.DangKys
               .Include(d => d.SinhVien)
                .Where(d => d.MaSV == maSV)
                .FirstOrDefaultAsync();

            if (dangKy == null)
            {
                return View(new List<ChiTietDangKy>());
            }

            var chiTietDangKys = await _context.ChiTietDangKys
                 .Include(ct => ct.HocPhan)
                .Where(ct => ct.MaDK == dangKy.MaDK)
                .ToListAsync();

            return View(chiTietDangKys);
        }
        // POST: DangKy/XoaKhoiGioHang
        [HttpPost]
        public async Task<IActionResult> XoaKhoiGioHang(int maDK, string maHP)
        {
            //  Lấy mã sinh viên từ session hoặc cookie
            string maSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Account");
            }
            var dangKy = await _context.DangKys
                .Where(d => d.MaDK == maDK && d.MaSV == maSV)
                .FirstOrDefaultAsync();
            if (dangKy == null)
            {
                return NotFound();
            }
            var chiTietDangKy = await _context.ChiTietDangKys
                .Where(ct => ct.MaDK == maDK && ct.MaHP == maHP)
               .FirstOrDefaultAsync();
            if (chiTietDangKy == null)
            {
                return NotFound();
            }

            _context.ChiTietDangKys.Remove(chiTietDangKy);
            //tăng lại số lượng
            var hocPhan = await _context.HocPhans.FirstOrDefaultAsync(hp => hp.MaHP == maHP);
            if (hocPhan != null)
            {
                hocPhan.SoLuong++;
                _context.HocPhans.Update(hocPhan);
            }
            await _context.SaveChangesAsync();
            return Ok("Đã xóa học phần khỏi giỏ hàng");
        }
        // POST: DangKy/XoaDangKy
        [HttpPost]
        public async Task<IActionResult> XoaDangKy(int maDK)
        {
            //  Lấy mã sinh viên từ session hoặc cookie
            string maSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Account");
            }
            var dangKy = await _context.DangKys
              .Where(d => d.MaDK == maDK && d.MaSV == maSV)
               .FirstOrDefaultAsync();
            if (dangKy == null)
            {
                return NotFound();
            }
            var chiTietDangKys = _context.ChiTietDangKys.Where(ct => ct.MaDK == maDK);
            if (chiTietDangKys == null)
            {
                return NotFound();
            }
            foreach (var ct in chiTietDangKys)
            {
                _context.ChiTietDangKys.Remove(ct);
                var hocPhan = await _context.HocPhans.FirstOrDefaultAsync(hp => hp.MaHP == ct.MaHP);
                if (hocPhan != null)
                {
                    hocPhan.SoLuong++;
                    _context.HocPhans.Update(hocPhan);
                }
            }
            _context.DangKys.Remove(dangKy);
            await _context.SaveChangesAsync();
            return Ok("Đã xóa toàn bộ đăng ký");
        }
        // GET: DangKy/DatHang
        public async Task<IActionResult> DatHang()
        {
            string maSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("DangNhap", "Account");
            }

            var dangKy = await _context.DangKys
               .Include(d => d.SinhVien)
                .Where(d => d.MaSV == maSV)
               .FirstOrDefaultAsync();
            if (dangKy == null)
            {
                return NotFound();
            }
            var chiTietDangKys = await _context.ChiTietDangKys
                  .Include(ct => ct.HocPhan)
                 .Where(ct => ct.MaDK == dangKy.MaDK)
                 .ToListAsync();
            ViewBag.ChiTietDangKys = chiTietDangKys;
            return View(dangKy);
        }
        // GET: DangKy/XacnhanDonHang
        public async Task<IActionResult> XacnhanDonHang()
        {
            return View();
        }
    }
}