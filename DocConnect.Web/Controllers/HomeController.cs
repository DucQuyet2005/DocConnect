using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DocConnect.Web.Models;
using DocConnect.Web.Data;
using DocConnect.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DocConnect.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DocConnectDbContext _context;

        // Tiêm DbContext vào thông qua Constructor
        public HomeController(DocConnectDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var top5Doctors = await _context.Database
                .SqlQueryRaw<BacSiViewModel>("EXEC GetTop5BacSi")
                .ToListAsync();
            return View(top5Doctors);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}