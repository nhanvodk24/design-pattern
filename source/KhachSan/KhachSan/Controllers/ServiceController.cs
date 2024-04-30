using KhachSan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace KhachSan.Controllers
{
    public class ServiceController : Controller
    {
        private readonly QLKSContext _context;
        public ServiceController(QLKSContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listServices = _context.Services.AsNoTracking().OrderBy(x => x.Id);
            PagedList<Service> list = new PagedList<Service>(listServices, pageNumber, pageSize);
            return View(list);
        }
    }
}
