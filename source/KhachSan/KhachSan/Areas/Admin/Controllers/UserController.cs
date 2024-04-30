using KhachSan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KhachSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly QLKSContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(QLKSContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int? page)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                int currentPage = page ?? 1;
                int pageSize = 5;
                int totalUsers = _context.Users.Count();
                int totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
                if (currentPage < 1 || currentPage > totalPages)
                {
                    currentPage = 1;
                }
                int skip = (currentPage - 1) * pageSize;
                var taikhoan = _context.Users
                    .OrderBy(r => r.Id)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                ViewBag.CurrentPage = currentPage;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                return View(taikhoan);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult AddUser()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        [HttpPost]
        public IActionResult AddUser(string name, string address, string username, string password, string role)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
            {
                User user = new User(name, address, username, password, role);
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddAccount", "Account", new { area = "Admin" });

            }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult EditUser(string id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
            {
                int idUser = (int)Convert.ToInt32(id);
                var user = _context.Users.FirstOrDefault(r => r.Id == idUser);
                return View(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        [HttpPost]
        public IActionResult EditUser(string Id, string name, string address, string username, string password, string role)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
            {
                int idUser = Convert.ToInt32(Id);
                var user = _context.Users.FirstOrDefault(r => r.Id == idUser);
                user.name = name;
                user.address = address;
                user.username = username;
                user.password = password;
                user.role = role;
                _context.SaveChanges();
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("EditUser", "Account", new { area = "Admin", id = Id });
            }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult RemoveUser(string id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                TempData["Message"] = "";
            int idUser = (int)Convert.ToInt32(id);
            var hdUser = _context.Bookings.Where(r => r.userId == idUser).ToList();
            if (hdUser.Count() > 0)
            {
                TempData["Message"] = "Không xóa được người dùng này!";
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            else
            {
                var user = _context.Users.FirstOrDefault(r => r.Id == idUser);
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Message"] = "Người dùng đã được xóa!";
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
    }
}
