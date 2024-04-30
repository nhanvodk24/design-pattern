using KhachSan.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhachSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly QLKSContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceController(QLKSContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int? page)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                int currentPage = page ?? 1;
                int pageSize = 3;
                int totalServices = _context.Services.Count();
                int totalPages = (int)Math.Ceiling(totalServices / (double)pageSize);
                if (currentPage < 1 || currentPage > totalPages)
                {
                    currentPage = 1;
                }
                int skip = (currentPage - 1) * pageSize;
                var service = _context.Services
                    .OrderBy(r => r.Id)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                ViewBag.CurrentPage = currentPage;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                return View(service);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult AddService()
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
        public IActionResult AddService(string DichVuTen, string DichVuGia, string DichVuMoTa, IFormFile DichVuimg)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
                {
                    double Gia = Convert.ToDouble(DichVuGia);

                    Service service = new Service(DichVuTen, Gia, DichVuMoTa);

                    if (DichVuimg != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/services");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + DichVuimg.FileName;
                        string filePath = Path.Combine(uploadsDir, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            DichVuimg.CopyTo(fileStream);
                        }

                        service.image = "/img/services/" + uniqueFileName;
                        _context.Services.Add(service);
                        _context.SaveChanges();

                        return RedirectToAction("Index", "Service", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("AddService", "Service", new { area = "Admin" });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("AddService", "Service", new { area = "Admin" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult EditService(string id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
                {
                    int idService = (int)Convert.ToInt32(id);
                    var service = _context.Services.FirstOrDefault(r => r.Id == idService);
                    return View(service);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Service", new { area = "Admin" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
           }
        [HttpPost]
        public IActionResult EditService(String DichVuID, string DichVuTen, string DichVuGia, string DichVuMoTa, IFormFile DichVuimg)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {

                try
                {
                    int idDichVu = Convert.ToInt32(DichVuID);
                    var service = _context.Services.FirstOrDefault(r => r.Id == idDichVu);
                    if (service != null && DichVuimg == null)
                    {
                        service.name = DichVuTen;
                        service.price = Convert.ToDouble(DichVuGia);
                        service.description = DichVuMoTa;
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Service", new { area = "Admin" });

                    }
                    else if (service != null && DichVuimg != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/services");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + DichVuimg.FileName;
                        string filePath = Path.Combine(uploadsDir, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            DichVuimg.CopyTo(fileStream);
                        }
                        service.name = DichVuTen;
                        service.price = Convert.ToDouble(DichVuGia);
                        service.description = DichVuMoTa;
                        service.image = "/img/services/" + uniqueFileName;
                        _context.SaveChanges();

                        return RedirectToAction("Index", "Service", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("EditService", "Service", new { area = "Admin", id = DichVuID });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("EditService", "Service", new { area = "Admin", id = DichVuID });

                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            }
        public IActionResult RemoveService(string id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                TempData["Message"] = "";
                int idService = (int)Convert.ToInt32(id);
                var hdService = _context.BookingsServiceDetails.Where(r => r.serviceId == idService).ToList();
                if (hdService.Count() > 0)
                {
                    TempData["Message"] = "Không xóa được dịch vụ này!";
                    return RedirectToAction("Index", "Service", new { area = "Admin" });
                }
                else
                {
                    var service = _context.Services.FirstOrDefault(r => r.Id == idService);
                    _context.Services.Remove(service);
                    _context.SaveChanges();
                    TempData["Message"] = "Dịch vụ đã được xóa!";
                    return RedirectToAction("Index", "Service", new { area = "Admin" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
    }
}
