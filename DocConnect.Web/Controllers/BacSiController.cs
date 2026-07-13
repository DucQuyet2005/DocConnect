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
        private readonly DocConnectDbContext _context;
        public BacSiController(DocConnectDbContext context)
        {
            _context = context;
        }

        // Nhận diện cả: /BacSi và /BacSi/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index(int? chuyenKhoaId, string? ngayKham)
        {
            int p_ChuyenKhoaId = chuyenKhoaId ?? 0;

            var danhSachBacSi = _context.Database
                .SqlQueryRaw<BacSiViewModel>(
                    "EXEC GetDanhSachBacSi @ChuyenKhoaId = {0}",
                    p_ChuyenKhoaId)
                .ToList();

            ViewBag.ChuyenKhoas = _context.ChuyenKhoas.ToList();
            ViewBag.SelectedChuyenKhoa = chuyenKhoaId;
            ViewBag.SelectedNgayKham = ngayKham;

            return View(danhSachBacSi);
        }

        // Nhận diện: /BacSi/ChiTiet/DR_USER_001
        [HttpGet("ChiTiet/{id}")]
        public IActionResult ChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var p_BacSiId = new SqlParameter("@BacSiId", SqlDbType.NVarChar, 50) { Value = id };

            var chiTietBacSi = _context.Database
                .SqlQueryRaw<BacSiViewModel>(
                    "EXEC GetChiTietBacSi @BacSiId", 
                    p_BacSiId)
                .AsEnumerable()
                .FirstOrDefault();

            if (chiTietBacSi == null || string.IsNullOrEmpty(chiTietBacSi.Id))
            {
                return NotFound();
            }

            return View(chiTietBacSi);
        }
    }
}