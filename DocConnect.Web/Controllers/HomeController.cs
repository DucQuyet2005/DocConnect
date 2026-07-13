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
        private readonly DocConnect.Web.Repositories.IBacSiRepository _bacSiRepository;

        // Tiêm Repository vào thông qua Constructor
        public HomeController(DocConnect.Web.Repositories.IBacSiRepository bacSiRepository)
        {
            _bacSiRepository = bacSiRepository;
        }
        public async Task<IActionResult> Index()
        {
            var top5Doctors = await _bacSiRepository.GetTop5BacSiAsync();
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