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
            
            // 1. TÌM KIẾM phiên hiện có trước (quan trọng nhất)
            var phienTonTai = await _context.PhienTuVans
                .FirstOrDefaultAsync(p => p.BacSiId == id && p.BenhNhanId == userId 
                                    && (p.TrangThai == "ChoTraLoi" || p.TrangThai == "Active"));

            // 2. Nếu đã có phiên rồi, trả về luôn, không tạo mới
            if (phienTonTai != null)
            {
                return RedirectToAction(nameof(CuocTroChuyen), new { id = phienTonTai.Id });
            }

            // 3. Nếu chưa có, mới tiến hành tạo mới
            var hoSoBacSi = await _context.HoSoBacSis.FirstOrDefaultAsync(h => h.NguoiDungId == id);
            if (hoSoBacSi == null) return NotFound();

            var lichHen = new LichHen { /* ... dữ liệu lịch hẹn ... */ };
            _context.LichHens.Add(lichHen);
            await _context.SaveChangesAsync();

            var phienMoi = new PhienTuVan {
                LichHenId = lichHen.Id,
                BacSiId = id,
                BenhNhanId = userId,
                ThoiGianBatDauThucTe = DateTime.Now,
                TrangThai = "ChoTraLoi"
            };
            _context.PhienTuVans.Add(phienMoi);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CuocTroChuyen), new { id = phienMoi.Id });
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
            
            // 1. Lấy tất cả phiên của bác sĩ
            var danhSachPhien = await _context.PhienTuVans
                .Include(p => p.BenhNhan)
                .Where(p => p.BacSiId == userId)
                .ToListAsync();

            // 2. Gán thông tin tin nhắn cuối để có thời gian so sánh
            foreach (var phien in danhSachPhien)
            {
                var tinNhanCuoi = await _context.TinNhans
                    .Where(t => t.PhienTuVanId == phien.Id)
                    .OrderByDescending(t => t.ThoiGianGui)
                    .FirstOrDefaultAsync();

                if (tinNhanCuoi != null)
                {
                    phien.TinNhanCuoi = tinNhanCuoi.NoiDung;
                    phien.ThoiGianCuoiCung = tinNhanCuoi.ThoiGianGui; // Dùng thời gian này để sắp xếp
                    phien.NguoiGuiCuoiId = tinNhanCuoi.NguoiGuiId;
                    phien.IsUnread = (tinNhanCuoi.NguoiGuiId != userId) && !tinNhanCuoi.DaDoc;
                }
                else
                {
                    // Nếu chưa có tin nhắn, dùng thời gian bắt đầu phiên
                    phien.ThoiGianCuoiCung = phien.ThoiGianBatDauThucTe; 
                }
            }

            // 3. SẮP XẾP LẠI (Tin mới nhất lên đầu)
            var danhSachDaSapXep = danhSachPhien
                .OrderByDescending(p => p.ThoiGianCuoiCung)
                .ToList();

            return View(danhSachDaSapXep);
        }


        [HttpGet("GetDanhSachPhienJson")]
        public async Task<IActionResult> GetDanhSachPhienJson()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = await _context.PhienTuVans
                .Include(p => p.BenhNhan)
                .Where(p => p.BacSiId == userId)
                .Select(p => new {
                    phienId = p.Id,
                    tenBenhNhan = p.BenhNhan!.HoTen
                })
                .ToListAsync();
            return Json(data);
        }

        [HttpPost]
        public IActionResult DanhDauDaDoc(int phienId)
        {
            // Ở đây, vì DaDoc không lưu vào DB, bạn có thể thực hiện một logic khác
            // ví dụ: lưu vào một bảng riêng hoặc đơn giản là xử lý trên giao diện (JS)
            return Ok();
        }
        }
}