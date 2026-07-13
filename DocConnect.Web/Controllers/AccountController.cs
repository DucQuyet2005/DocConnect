using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using DocConnect.Web.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DocConnect.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DocConnect.Web.Repositories.IAccountRepository _accountRepository;
        private readonly DocConnectDbContext _context;
        private readonly PasswordHasher<NguoiDung> _passwordHasher;

        public AccountController(DocConnect.Web.Repositories.IAccountRepository accountRepository, DocConnectDbContext context)
        {
            _accountRepository = accountRepository;
            _context = context;
            _passwordHasher = new PasswordHasher<NguoiDung>();
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ChuyenKhoas = _context.ChuyenKhoas.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.VaiTro == "Doctor" && !model.ChuyenKhoaId.HasValue)
                {
                    ModelState.AddModelError("ChuyenKhoaId", "Bác sĩ vui lòng chọn chuyên khoa.");
                    ViewBag.ChuyenKhoas = _context.ChuyenKhoas.ToList();
                    return View(model);
                }
                var emailExists = await _accountRepository.EmailExistsAsync(model.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký sử dụng.");
                    return View(model);
                }

                var user = new NguoiDung
                {
                    Id = Guid.NewGuid().ToString(),
                    HoTen = model.HoTen,
                    Email = model.Email,
                    SoDienThoai = model.SoDienThoai ?? "",
                    VaiTro = model.VaiTro,
                    TrangThai = model.VaiTro != "Doctor", 
                    NgayTao = DateTime.Now,
                    AnhDaiDien = "/images/default-avatar.png"
                };
                user.MatKhauHash = _passwordHasher.HashPassword(user, model.MatKhau);

                await _accountRepository.AddUserAsync(user);

                // Nếu là bác sĩ, tạo thêm HoSoBacSi
                if (model.VaiTro == "Doctor" && model.ChuyenKhoaId.HasValue)
                {
                    var hoSo = new HoSoBacSi
                    {
                        NguoiDungId = user.Id,
                        ChuyenKhoaId = model.ChuyenKhoaId.Value,
                        KinhNghiem = "Chưa cập nhật",
                        GioiThieu = "Bác sĩ mới",
                        TrangThaiXacThuc = "Pending"
                    };
                    _context.HoSoBacSis.Add(hoSo);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
                return RedirectToAction("Login");
            }
            ViewBag.ChuyenKhoas = _context.ChuyenKhoas.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    //if (user.TrangThai == false || !user.TrangThai.HasValue)
                    //{
                    //    ModelState.AddModelError("", "Tài khoản của bạn hiện đang bị khóa hoặc đang chờ ban quản trị phê duyệt.");
                    //    return View(model);
                    //}
                    var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.MatKhauHash, model.MatKhau);
                    if (verifyResult == PasswordVerificationResult.Success)
                    {
                        user.TrangThai = true;
                        await _accountRepository.UpdateUserAsync(user);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Name, user.HoTen),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.VaiTro)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties 
                        {
                            IsPersistent = model.GhiNho,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _accountRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    user.TrangThai = false;
                    await _accountRepository.UpdateUserAsync(user);
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}