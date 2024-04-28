using KhachSan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KhachSan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LienHe()
        {
            return View();
        }
        public IActionResult GioiThieu()
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