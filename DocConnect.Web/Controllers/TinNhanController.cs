using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DocConnect.Web.Controllers
{
    [Route("TinNhan")]
    public class TinNhanController : Controller
    {
        
        private readonly DocConnect.Web.Repositories.ITinNhanRepository _tinNhanRepository;

        public TinNhanController(DocConnect.Web.Repositories.ITinNhanRepository tinNhanRepository)
        {
            _tinNhanRepository = tinNhanRepository;
        }

        [HttpPost("LuuTrieuChungTamThoi")]
        [Authorize]
        public IActionResult LuuTrieuChungTamThoi(string trieuChung)
        {
            if (!string.IsNullOrEmpty(trieuChung))
            {
                HttpContext.Session.SetString("InitialSymptoms", trieuChung);
            }
            return Json(new { success = true });
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

            var hoSoBacSi = await _tinNhanRepository.GetHoSoBacSiByUserIdAsync(bacSiId);
            if (hoSoBacSi == null)
            {
                return NotFound();
            }

            var phien = await _tinNhanRepository.GetPhienTuVanActiveAsync(bacSiId, benhNhanId);

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

                await _tinNhanRepository.AddLichHenAsync(lichHen);
                await _tinNhanRepository.SaveChangesAsync();

                phien = new PhienTuVan
                {
                    LichHenId = lichHen.Id,
                    BacSiId = bacSiId,
                    BenhNhanId = benhNhanId,
                    ThoiGianBatDauThucTe = DateTime.Now,
                    TrangThai = "Active"
                };

                await _tinNhanRepository.AddPhienTuVanAsync(phien);
                await _tinNhanRepository.SaveChangesAsync();
            }

            // Đọc triệu chứng ban đầu lưu từ Session (nếu có)
            var symptoms = HttpContext.Session.GetString("InitialSymptoms");
            if (!string.IsNullOrEmpty(symptoms))
            {
                var firstMsg = new TinNhan
                {
                    PhienTuVanId = phien.Id,
                    NguoiGuiId = benhNhanId,
                    NoiDung = $"[Triệu chứng ban đầu]: {symptoms}",
                    LoaiTinNhan = "Text",
                    ThoiGianGui = DateTime.Now,
                    DaDoc = false
                };
                await _tinNhanRepository.AddTinNhanAsync(firstMsg);
                await _tinNhanRepository.SaveChangesAsync();
                HttpContext.Session.Remove("InitialSymptoms");
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
            
            // 1. TÌM KIẾM phiên hiện có trước (quan trọng nhất)
            var phien = await _tinNhanRepository.GetPhienTuVanActiveAsync(id, userId);

            // 2. Nếu chưa có, mới tiến hành tạo mới
            if (phien == null)
            {
                var hoSoBacSi = await _tinNhanRepository.GetHoSoBacSiByUserIdAsync(id);
                if (hoSoBacSi == null) return NotFound();

                var lichHen = new LichHen
                {
                    NguoiDungId = userId,
                    BacSiId = hoSoBacSi.Id,
                    ThoiGianBatDau = DateTime.Now,
                    ThoiGianKetThuc = DateTime.Now.AddHours(1),
                    TrangThai = "Approved"
                };
                await _tinNhanRepository.AddLichHenAsync(lichHen);
                await _tinNhanRepository.SaveChangesAsync();

                phien = new PhienTuVan {
                    LichHenId = lichHen.Id,
                    BacSiId = id,
                    BenhNhanId = userId,
                    ThoiGianBatDauThucTe = DateTime.Now,
                    TrangThai = "ChoTraLoi"
                };
                await _tinNhanRepository.AddPhienTuVanAsync(phien);
                await _tinNhanRepository.SaveChangesAsync();
            }

            // Đọc triệu chứng ban đầu lưu từ Session (nếu có)
            var symptoms = HttpContext.Session.GetString("InitialSymptoms");
            if (!string.IsNullOrEmpty(symptoms))
            {
                var firstMsg = new TinNhan
                {
                    PhienTuVanId = phien.Id,
                    NguoiGuiId = userId,
                    NoiDung = $"[Triệu chứng ban đầu]: {symptoms}",
                    LoaiTinNhan = "Text",
                    ThoiGianGui = DateTime.Now,
                    DaDoc = false
                };
                await _tinNhanRepository.AddTinNhanAsync(firstMsg);
                await _tinNhanRepository.SaveChangesAsync();
                HttpContext.Session.Remove("InitialSymptoms");
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
                phien = await _tinNhanRepository.GetPhienTuVanByIdAsync(phienId);
            }
            else
            {
                var hoSoBacSi = await _tinNhanRepository.GetHoSoBacSiByUserIdAsync(id);
                if (hoSoBacSi == null)
                {
                    return Content($"LỖI DỮ LIỆU: Không tìm thấy hồ sơ bác sĩ có NguoiDungId = '{id}'!");
                }

                phien = await _tinNhanRepository.GetPhienTuVanActiveAsync(id, userId);

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

                    await _tinNhanRepository.AddLichHenAsync(lichHen);
                    await _tinNhanRepository.SaveChangesAsync();

                    phien = new PhienTuVan
                    {
                        LichHenId = lichHen.Id,
                        BacSiId = id,
                        BenhNhanId = userId,
                        ThoiGianBatDauThucTe = DateTime.Now,
                        TrangThai = "ChoTraLoi"
                    };

                    await _tinNhanRepository.AddPhienTuVanAsync(phien);
                    await _tinNhanRepository.SaveChangesAsync();
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

            var lichSuTinNhan = await _tinNhanRepository.GetTinNhansByPhienIdAsync(phien.Id);

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
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            // 1. Lấy tất cả phiên của bác sĩ
            var danhSachPhien = await _tinNhanRepository.GetPhienTuVansByBacSiIdAsync(userId);

            // 2. Gán thông tin tin nhắn cuối để có thời gian so sánh
            foreach (var phien in danhSachPhien)
            {
                var tinNhanCuoi = await _tinNhanRepository.GetTinNhanCuoiAsync(phien.Id);

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
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var phienTuVans = await _tinNhanRepository.GetPhienTuVansByBacSiIdAsync(userId);
            var data = phienTuVans
                .Select(p => new {
                    phienId = p.Id,
                    tenBenhNhan = p.BenhNhan!.HoTen
                })
                .ToList();
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