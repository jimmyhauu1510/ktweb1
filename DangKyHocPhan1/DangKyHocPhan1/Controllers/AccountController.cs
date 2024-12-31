using Microsoft.AspNetCore.Mvc;
using DangKyHocPhan1.Models;
using DangKyHocPhan1;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DangKyHocPhan;

namespace DangKyHocPhan1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/DangNhap
        public IActionResult DangNhap()
        {
            return View();
        }

        // POST: Account/DangNhap
        [HttpPost]
        public async Task<IActionResult> DangNhap(string maSV)
        {
            if (string.IsNullOrEmpty(maSV))
            {
                ModelState.AddModelError("MaSV", "Mã sinh viên không được để trống.");
                return View();
            }
            var sinhVien = await _context.SinhViens.FirstOrDefaultAsync(sv => sv.MaSV == maSV);
            if (sinhVien == null)
            {
                ModelState.AddModelError("MaSV", "Mã sinh viên không tồn tại.");
                return View();
            }
            // Lưu mã sinh viên vào session
            HttpContext.Session.SetString("MaSV", maSV);

            return RedirectToAction("ListHP", "HocPhan"); // Chuyển hướng đến trang danh sách học phần
        }
        // GET: Account/DangXuat
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap", "Account");
        }

    }
}