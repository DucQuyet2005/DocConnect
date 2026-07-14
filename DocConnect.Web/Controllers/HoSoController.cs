using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DocConnect.Web.Controllers
{
    [Route("HoSo")]
    [Authorize] // Bắt buộc đăng nhập
    public class HoSoController : Controller
    {
        private readonly DocConnectDbContext _context;

        public HoSoController(DocConnectDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (User.IsInRole("Doctor"))
            {
                return RedirectToAction(nameof(QuanLyBenhAn));
            }
            return RedirectToAction(nameof(LichSuBenhAn));
        }

        public class TaoHoSoRequest
        {
            public int PhienTuVanId { get; set; }
            public string BenhNhanId { get; set; } = string.Empty;
            public string? TrieuChung { get; set; }
            public string? KetLuan { get; set; }
        }

        [HttpPost("KetThucCaKham")]
        [Authorize(Roles = "Doctor")] // BẢO MẬT: Chỉ Bác Sĩ mới được phép gọi API này
        public async Task<IActionResult> KetThucCaKham([FromBody] TaoHoSoRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.KetLuan))
                return BadRequest("Dữ liệu không hợp lệ.");

            // Lấy Tên Bác sĩ hiện tại từ Cookie
            var tenBacSi = User.FindFirstValue(ClaimTypes.Name) ?? "Bác sĩ ẩn danh";
            var bacSiId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Cập nhật trạng thái Phiên Tư Vấn thành "Closed"
            var phien = await _context.PhienTuVans.FindAsync(req.PhienTuVanId);
            if (phien != null)
            {
                if (phien.BacSiId != bacSiId) return Forbid("Bạn không có quyền kết thúc ca khám của người khác.");
                phien.TrangThai = "Closed";
                phien.ThoiGianKetThucThucTe = DateTime.Now;
                _context.PhienTuVans.Update(phien);
            }

            // 2. Tạo Hồ sơ sức khỏe mới
            var hoSo = new HoSoSucKhoe
            {
                PhienTuVanId = req.PhienTuVanId,
                NguoiDungId = req.BenhNhanId,
                TrieuChung = req.TrieuChung,
                KetLuan = req.KetLuan,
                TenBacSi = tenBacSi,
                ThoiGian = DateTime.Now
            };

            _context.HoSoSucKhoes.Add(hoSo);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        // Bệnh nhân xem danh sách bệnh án của mình
        [HttpGet("LichSuBenhAn")]
        [Authorize(Roles = "Customer")] // BẢO MẬT: Chỉ Bệnh Nhân
        public IActionResult LichSuBenhAn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dsHoSo = _context.HoSoSucKhoes
                                 .Where(h => h.NguoiDungId == userId)
                                 .OrderByDescending(h => h.ThoiGian)
                                 .ToList();
            
            return View(dsHoSo);
        }

        // Bác sĩ xem và quản lý danh sách bệnh án mình đã lập
        [HttpGet("QuanLyBenhAn")]
        [Authorize(Roles = "Doctor")]
        public IActionResult QuanLyBenhAn()
        {
            var bacSiId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dsHoSo = (from h in _context.HoSoSucKhoes
                          join p in _context.PhienTuVans on h.PhienTuVanId equals p.Id
                          where p.BacSiId == bacSiId
                          orderby h.ThoiGian descending
                          select h).ToList();
                          
            return View(dsHoSo);
        }

        // Bác sĩ sửa bệnh án
        [HttpGet("Edit/{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Edit(int id)
        {
            var hoSo = await _context.HoSoSucKhoes.FindAsync(id);
            if (hoSo == null) return NotFound();

            var bacSiId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var phien = await _context.PhienTuVans.FindAsync(hoSo.PhienTuVanId);
            if (phien == null || phien.BacSiId != bacSiId) return Forbid();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    id = hoSo.Id, 
                    phienTuVanId = hoSo.PhienTuVanId,
                    nguoiDungId = hoSo.NguoiDungId,
                    thoiGian = hoSo.ThoiGian.ToString("yyyy-MM-ddTHH:mm:ss"),
                    tenBacSi = hoSo.TenBacSi,
                    trieuChung = hoSo.TrieuChung, 
                    ketLuan = hoSo.KetLuan 
                });
            }

            return View(hoSo);
        }

        [HttpPost("Edit/{id}")]
        [Authorize(Roles = "Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HoSoSucKhoe model)
        {
            if (id != model.Id) return BadRequest();

            var hoSo = await _context.HoSoSucKhoes.FindAsync(id);
            if (hoSo == null) return NotFound();

            var bacSiId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var phien = await _context.PhienTuVans.FindAsync(hoSo.PhienTuVanId);
            if (phien == null || phien.BacSiId != bacSiId) return Forbid();

            if (ModelState.IsValid)
            {
                hoSo.TrieuChung = model.TrieuChung;
                hoSo.KetLuan = model.KetLuan;
                _context.HoSoSucKhoes.Update(hoSo);
                await _context.SaveChangesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cập nhật hồ sơ bệnh án thành công!", data = new { id = hoSo.Id, trieuChung = hoSo.TrieuChung, ketLuan = hoSo.KetLuan } });
                }

                TempData["SuccessMessage"] = "Cập nhật hồ sơ bệnh án thành công!";
                return RedirectToAction(nameof(QuanLyBenhAn));
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }
            return View(model);
        }

        // Bác sĩ xóa bệnh án
        [HttpPost("Delete/{id}")]
        [Authorize(Roles = "Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var hoSo = await _context.HoSoSucKhoes.FindAsync(id);
            if (hoSo == null) return NotFound();

            var bacSiId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var phien = await _context.PhienTuVans.FindAsync(hoSo.PhienTuVanId);
            if (phien == null || phien.BacSiId != bacSiId) return Forbid();

            _context.HoSoSucKhoes.Remove(hoSo);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Đã xóa hồ sơ bệnh án thành công." });
            }

            TempData["SuccessMessage"] = "Đã xóa hồ sơ bệnh án.";
            return RedirectToAction(nameof(QuanLyBenhAn));
        }
    }
}
