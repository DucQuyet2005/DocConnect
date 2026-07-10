using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities; // Chứa PhienTuVan, TinNhan
using DocConnect.Models.Entities;     // Chứa LichHen (nếu hệ thống chia tách)
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocConnect.Web.Controllers
{
    [Route("TinNhan")] // 1. Định nghĩa Route gốc của Controller
    public class TinNhanController : Controller
    {
        private readonly DocConnectDbContext _context;

        public TinNhanController(DocConnectDbContext context)
        {
            _context = context;
        }

        // 2. Định nghĩa Sub-Route khớp 100% với thẻ <a href="/TinNhan/CuocTroChuyen/@dr.Id">
        [HttpGet("CuocTroChuyen/{id}")] 
        public async Task<IActionResult> CuocTroChuyen(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id)) return NotFound();

            // Tìm hồ sơ bác sĩ kiểu int tương ứng từ NguoiDungId kiểu string
            var hoSoBacSi = await _context.HoSoBacSis.FirstOrDefaultAsync(h => h.NguoiDungId == id);
            if (hoSoBacSi == null)
            {
                return Content($"LỖI DỮ LIỆU: Route hoạt động đúng! Nhưng bảng [HoSoBacSi] của bạn chưa có dòng nào mang NguoiDungId bằng '{id}'!");
            }

            int bacSiIdInt = hoSoBacSi.Id; 

            // Tìm kiếm phiên tư vấn
            var phien = await (from p in _context.PhienTuVans
                               join lh in _context.LichHens on p.LichHenId equals lh.Id
                               where lh.NguoiDungId == userId && lh.BacSiId == bacSiIdInt && p.TrangThai == "Active"
                               select p).FirstOrDefaultAsync();

            if (phien == null)
            {
                var lichHen = await _context.LichHens
                    .FirstOrDefaultAsync(lh => lh.NguoiDungId == userId && lh.BacSiId == bacSiIdInt && lh.TrangThai == "Approved");

                int lichHenId = 0;
                if (lichHen == null)
                {
                    var newLichHen = new LichHen
                    {
                        NguoiDungId = userId,
                        BacSiId = bacSiIdInt,
                        ThoiGianBatDau = DateTime.Now,
                        ThoiGianKetThuc = DateTime.Now.AddHours(1),
                        TrangThai = "Approved"
                    };
                    _context.LichHens.Add(newLichHen);
                    await _context.SaveChangesAsync();
                    lichHenId = newLichHen.Id;
                }
                else
                {
                    lichHenId = lichHen.Id;
                }

                phien = new PhienTuVan
                {
                    LichHenId = lichHenId,
                    ThoiGianBatDauThucTe = DateTime.Now,
                    TrangThai = "Active"
                };
                _context.PhienTuVans.Add(phien);
                await _context.SaveChangesAsync();
            }

            var lichSuTinNhan = await _context.TinNhans
                .Where(t => t.PhienTuVanId == phien.Id)
                .OrderBy(t => t.ThoiGianGui)
                .ToListAsync();

            ViewBag.CurrentUserId = userId;
            ViewBag.LichSuTinNhan = lichSuTinNhan;
            ViewBag.BacSiId = id; 

            return View(phien);
        }

        // 3. Định nghĩa Sub-Route cho API gửi AJAX
        [HttpPost("GuiTinNhanAjax")]
        public async Task<IActionResult> GuiTinNhanAjax(int phienId, string noiDung)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(noiDung))
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var msg = new TinNhan
            {
                PhienTuVanId = phienId,
                NguoiGuiId = senderId,
                NoiDung = noiDung.Trim(),
                LoaiTinNhan = "Text",
                ThoiGianGui = DateTime.Now
            };

            _context.TinNhans.Add(msg);
            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                senderId = senderId, 
                tenNguoiGui = User.Identity?.Name ?? "Thành viên", 
                noiDung = msg.NoiDung, 
                thoiGian = msg.ThoiGianGui.ToString("HH:mm") 
            });
        }
    }
}