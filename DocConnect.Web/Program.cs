using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DocConnect.Web.Data;
using DocConnect.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình kết nối SQL Server thông qua chuỗi ConnectionStrings đã viết ở appsettings.json
builder.Services.AddDbContext<DocConnectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DocConnect.Web.Repositories.Interfaces.IAccountRepository, DocConnect.Web.Repositories.Implementations.AccountRepository>();
builder.Services.AddScoped<DocConnect.Web.Repositories.Interfaces.IBacSiRepository, DocConnect.Web.Repositories.Implementations.BacSiRepository>();
builder.Services.AddScoped<DocConnect.Web.Repositories.Interfaces.IHoiDapRepository, DocConnect.Web.Repositories.Implementations.HoiDapRepository>();
builder.Services.AddScoped<DocConnect.Web.Repositories.Interfaces.ITinNhanRepository, DocConnect.Web.Repositories.Implementations.TinNhanRepository>();

// 2. Cấu hình xác thực bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Chuyển hướng về đây nếu chưa đăng nhập
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

// Cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

var app = builder.Build();

// Thêm các Middleware cần thiết 
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Đảm bảo đặt 2 dòng này nằm ĐÚNG thứ tự và TRƯỚC UseAuthorization
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(); // Kích hoạt đọc file tĩnh như hình ảnh, CSS

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ĐẶT MAP HUB Ở ĐÂY (Sau UseAuthorization)
app.MapHub<ChatHub>("/chatHub");

app.Run();