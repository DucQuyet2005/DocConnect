using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DocConnect.Web.Controllers
{
    [Route("BacSi")] // Định nghĩa Route gốc cho toàn bộ Controller
    public class BacSiController : Controller
    {
        private readonly DocConnect.Web.Repositories.IBacSiRepository _bacSiRepository;
        public BacSiController(DocConnect.Web.Repositories.IBacSiRepository bacSiRepository)
        {
            _bacSiRepository = bacSiRepository;
        }

        // Nhận diện cả: /BacSi và /BacSi/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(int? chuyenKhoaId, string? ngayKham)
        {
            int p_ChuyenKhoaId = chuyenKhoaId ?? 0;

            var danhSachBacSi = await _bacSiRepository.GetDanhSachBacSiAsync(p_ChuyenKhoaId);

            ViewBag.ChuyenKhoas = await _bacSiRepository.GetChuyenKhoasAsync();
            ViewBag.SelectedChuyenKhoa = chuyenKhoaId;
            ViewBag.SelectedNgayKham = ngayKham;

            return View(danhSachBacSi);
        }

        // Nhận diện: /BacSi/ChiTiet/DR_USER_001
        [HttpGet("ChiTiet/{id}")]
        public async Task<IActionResult> ChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var chiTietBacSi = await _bacSiRepository.GetChiTietBacSiAsync(id);

            if (chiTietBacSi == null || string.IsNullOrEmpty(chiTietBacSi.Id))
            {
                return NotFound();
            }

            return View(chiTietBacSi);
        }
    }
}