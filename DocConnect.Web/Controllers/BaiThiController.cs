using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Controllers
{
    public class BaiThiController : Controller
    {
        private readonly DocConnectDbContext _context;

        public BaiThiController(DocConnectDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {

            ViewBag.ChuyenKhoas = new SelectList(_context.ChuyenKhoas.ToList(), "Id", "TenChuyenKhoa");

            ViewBag.NguoiDungs = new SelectList(_context.NguoiDungs.ToList(), "Id", "HoTen");

            return View();
        }

        [HttpGet]
        public IActionResult SearchBySpecialty(int? chuyenKhoaId)
        {
            var query = _context.HoSoBacSis
                .Include(h => h.NguoiDung)
                .Include(h => h.ChuyenKhoa)
                .AsQueryable();

            if (chuyenKhoaId.HasValue && chuyenKhoaId.Value > 0)
            {
                query = query.Where(h => h.ChuyenKhoaId == chuyenKhoaId.Value);
            }

            var results = query.ToList();
            return PartialView("_DoctorListPartial", results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HoSoBacSi model)
        {
            ModelState.Remove("NguoiDung");
            ModelState.Remove("ChuyenKhoa");

            if (ModelState.IsValid)
            {
                bool isCodeExist = _context.HoSoBacSis.Any(x => x.AccessCode == model.AccessCode);
                if (isCodeExist)
                {
                    return Json(new { success = false, message = "Ma truy xuat da ton tai. Vui long nhap ma khac." });
                }

                _context.HoSoBacSis.Add(model);
                _context.SaveChanges();
                return Json(new { success = true, message = "Them moi ho so bac si thanh cong!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join(" ", errors) });
        }
    }
}
