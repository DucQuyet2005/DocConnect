using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using DocConnect.Web.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DocConnect.Web.Controllers
{
    public class HoiDapController : Controller
    {
        private readonly DocConnectDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HoiDapController(DocConnectDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /HoiDap
        [HttpGet]
        public async Task<IActionResult> HoiDap()
        {
            // Lấy toàn bộ danh sách (gồm cả câu hỏi gốc và các câu trả lời "RE: ...") đã được duyệt
           ViewBag.ChuyenKhoas = await _context.ChuyenKhoas
                .OrderBy(x => x.TenChuyenKhoa)
                .ToListAsync();

            var tatCaDuLieu = await _context.HoiDaps
                .Include(x => x.ChuyenKhoa)
                .Include(x => x.NguoiDung)
                .Where(x => x.DaDuyet)
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();
                return View(tatCaDuLieu);
        }

        // POST: /HoiDap/DangCauHoi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangCauHoi(HoiDapViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? duongDanAnhId = null;

                if (model.FileAnh != null && model.FileAnh.Length > 0)
                {
                    // 1. Kiểm tra đuôi file an toàn
                    var extension = Path.GetExtension(model.FileAnh.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("FileAnh", "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif)");
                        return await PrepareViewWithError(model);
                    }

                    // 2. Định nghĩa thư mục lưu trữ bằng WebHostEnvironment
                    var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var tenFile = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(folderPath, tenFile);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FileAnh.CopyToAsync(stream);
                    }

                    duongDanAnhId = "/uploads/" + tenFile;
                }

                // Lấy Id người dùng đặt câu hỏi nếu có đăng nhập (tùy chọn)
                var nguoiDungId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var entityHoiDap = new HoiDap
                {
                    TieuDe = model.TieuDe,
                    NoiDung = model.NoiDung,
                    Tuoi = model.Tuoi,
                    GioiTinh = model.GioiTinh,
                    DuongDanAnh = duongDanAnhId,
                    ChuyenKhoaId = model.ChuyenKhoaId,
                    NguoiDungId = nguoiDungId,
                    NgayTao = DateTime.Now,
                    DaDuyet = true, // Mặc định duyệt luôn hoặc sửa thành false nếu muốn kiểm duyệt
                    AnDanh = true
                };

                _context.HoiDaps.Add(entityHoiDap);
                await _context.SaveChangesAsync();

                return RedirectToAction("HoiDap");
            }

            // Nếu dữ liệu không hợp lệ
            return await PrepareViewWithError(model);
        }

        private async Task<IActionResult> PrepareViewWithError(HoiDapViewModel model)
        {
            ViewBag.ChuyenKhoas = await _context.ChuyenKhoas
                .OrderBy(x => x.TenChuyenKhoa)
                .ToListAsync();

            var danhSachCu = await _context.HoiDaps
                .Include(x => x.ChuyenKhoa)
                .Include(x => x.NguoiDung)
                .Where(x => x.DaDuyet)
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();

            return View("HoiDap", danhSachCu);
        }

        // POST: /HoiDap/TraLoiAjax
        [HttpPost]
public async Task<IActionResult> TraLoiAjax(int id, string noiDungTraLoi)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var user = await _context.NguoiDungs
        .FirstOrDefaultAsync(x => x.Id == userId);

    // Tạo câu trả lời
    var traLoi = new HoiDap
    {
        TieuDe = $"RE: {id}",
        NoiDung = noiDungTraLoi,
        NgayTao = DateTime.Now,
        NguoiDungId = userId,
        DaDuyet = true
    };

    _context.HoiDaps.Add(traLoi);
    await _context.SaveChangesAsync();

    return Json(new
    {
        success = true,
        message = "Đã trả lời thành công",
        tenBacSi = user?.HoTen
    });
}
        // POST: /HoiDap/ThaTimAjax
        [HttpPost]
        public IActionResult ThaTimAjax(int id)
        {
            // Code xử lý tính năng tăng lượt thích/thả tim (nếu cần thiết)
            return Json(new { success = true, message = "Cảm ơn bạn đã bày tỏ lòng biết ơn với bài viết!" });
        }
    }
}