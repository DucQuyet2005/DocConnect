using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DocConnect.Web.Controllers
{
    [Route("TinNhan")]
    public class TinNhanController : Controller
    {
        
        private readonly DocConnectDbContext _context;

        public TinNhanController(DocConnectDbContext context)
        {
            _context = context;
        }

        [HttpPost("KhoiTaoPhien")]
        [Authorize]
        public async Task<IActionResult> KhoiTaoPhien(string bacSiId)
        {
            var benhNhanId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(benhNhanId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(bacSiId))
            {
                return NotFound();
            }

            var hoSoBacSi = await _context.HoSoBacSis.FirstOrDefaultAsync(h => h.NguoiDungId == bacSiId);
            if (hoSoBacSi == null)
            {
                return NotFound();
            }

            var phien = await _context.PhienTuVans
                .FirstOrDefaultAsync(p => p.BacSiId == bacSiId && p.BenhNhanId == benhNhanId && (p.TrangThai == "Active" || p.TrangThai == "ChoTraLoi"));

            if (phien == null)
            {
                var lichHen = new LichHen
                {
                    NguoiDungId = benhNhanId,
                    BacSiId = hoSoBacSi.Id,
                    ThoiGianBatDau = DateTime.Now,
                    ThoiGianKetThuc = DateTime.Now.AddHours(1),
                    TrangThai = "Approved"
                };

                _context.LichHens.Add(lichHen);
                await _context.SaveChangesAsync();

                phien = new PhienTuVan
                {
                    LichHenId = lichHen.Id,
                    BacSiId = bacSiId,
                    BenhNhanId = benhNhanId,
                    ThoiGianBatDauThucTe = DateTime.Now,
                    TrangThai = "Active"
                };

                _context.PhienTuVans.Add(phien);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CuocTroChuyen), new { id = phien.Id });
        }

        [HttpGet("TaoPhienTuVan/{id}")]
        [Authorize]
        public async Task<IActionResult> TaoPhienTuVan(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id)) return NotFound();

            var hoSoBacSi = await _context.HoSoBacSis.FirstOrDefaultAsync(h => h.NguoiDungId == id);
            if (hoSoBacSi == null)
            {
                return Content($"LỖI DỮ LIỆU: Route hoạt động đúng! Nhưng bảng [HoSoBacSi] của bạn chưa có dòng nào mang NguoiDungId bằng '{id}'!");
            }

            int bacSiIdInt = hoSoBacSi.Id;

            var phien = await _context.PhienTuVans
                .FirstOrDefaultAsync(p => p.BacSiId == id && p.BenhNhanId == userId && (p.TrangThai == "ChoTraLoi" || p.TrangThai == "Active"));

            if (phien == null)
            {
                var lichHen = new LichHen
                {
                    NguoiDungId = userId,
                    BacSiId = bacSiIdInt,
                    ThoiGianBatDau = DateTime.Now,
                    ThoiGianKetThuc = DateTime.Now.AddHours(1),
                    TrangThai = "Approved"
                };

                _context.LichHens.Add(lichHen);
                await _context.SaveChangesAsync();

                phien = new PhienTuVan
                {
                    LichHenId = lichHen.Id,
                    BacSiId = id,
                    BenhNhanId = userId,
                    ThoiGianBatDauThucTe = DateTime.Now,
                    TrangThai = "ChoTraLoi"
                };
                _context.PhienTuVans.Add(phien);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CuocTroChuyen), new { id = phien.Id });
        }

        [HttpGet("CuocTroChuyen/{id?}")]
        [Authorize]
        public async Task<IActionResult> CuocTroChuyen(string? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            PhienTuVan? phien;

            if (int.TryParse(id, out int phienId))
            {
                phien = await _context.PhienTuVans
                    .Include(p => p.BenhNhan)
                    .Include(p => p.BacSi)
                    .FirstOrDefaultAsync(p => p.Id == phienId);
            }
            else
            {
                var hoSoBacSi = await _context.HoSoBacSis.FirstOrDefaultAsync(h => h.NguoiDungId == id);
                if (hoSoBacSi == null)
                {
                    return Content($"LỖI DỮ LIỆU: Không tìm thấy hồ sơ bác sĩ có NguoiDungId = '{id}'!");
                }

                phien = await _context.PhienTuVans
                    .Include(p => p.BenhNhan)
                    .Include(p => p.BacSi)
                    .FirstOrDefaultAsync(p => p.BacSiId == id && p.BenhNhanId == userId && (p.TrangThai == "ChoTraLoi" || p.TrangThai == "Active"));

                if (phien == null)
                {
                    var lichHen = new LichHen
                    {
                        NguoiDungId = userId,
                        BacSiId = hoSoBacSi.Id,
                        ThoiGianBatDau = DateTime.Now,
                        ThoiGianKetThuc = DateTime.Now.AddHours(1),
                        TrangThai = "Approved"
                    };

                    _context.LichHens.Add(lichHen);
                    await _context.SaveChangesAsync();

                    phien = new PhienTuVan
                    {
                        LichHenId = lichHen.Id,
                        BacSiId = id,
                        BenhNhanId = userId,
                        ThoiGianBatDauThucTe = DateTime.Now,
                        TrangThai = "ChoTraLoi"
                    };

                    _context.PhienTuVans.Add(phien);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(CuocTroChuyen), new { id = phien.Id });
            }

            if (phien == null)
            {
                return NotFound();
            }

            if (phien.BenhNhanId != userId && phien.BacSiId != userId)
            {
                return Forbid();
            }

            var lichSuTinNhan = await _context.TinNhans
                .Where(t => t.PhienTuVanId == phien.Id)
                .OrderBy(t => t.ThoiGianGui)
                .ToListAsync();

            ViewBag.CurrentUserId = userId;
            ViewBag.LichSuTinNhan = lichSuTinNhan;
            ViewBag.BacSiId = phien.BacSiId;

            ViewBag.TenDoiPhuong = (phien.BacSiId == userId) 
                       ? (phien.BenhNhan?.HoTen ?? "Bệnh nhân") 
                       : (phien.BacSi?.HoTen ?? "Bác sĩ");

            return View(phien);
        }

        [HttpGet("DanhSachPhien")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DanhSachPhien()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Lấy các phiên tư vấn mà bác sĩ này phụ trách và đang cần trả lời
            var danhSachPhien = await _context.PhienTuVans
                .Include(p => p.BenhNhan) // Lấy thông tin bệnh nhân để hiển thị tên
                .Where(p => p.BacSiId == userId && (p.TrangThai == "Active" || p.TrangThai == "ChoTraLoi"))
                .OrderByDescending(p => p.ThoiGianBatDauThucTe)
                .ToListAsync();

            return View(danhSachPhien);
        }
    }
}