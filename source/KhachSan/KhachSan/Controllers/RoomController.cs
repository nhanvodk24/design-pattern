using KhachSan.Models;
using KhachSan.Extension;
using KhachSan.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace KhachSan.Controllers
{
    public class RoomController : Controller
    {
        private readonly QLKSContext _context;
        public RoomController(QLKSContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber=page==null || page<0 ? 1 : page.Value;
            var listRooms = _context.Rooms.AsNoTracking().OrderBy(x => x.Id);
            PagedList<Room> list = new PagedList<Room>(listRooms, pageNumber, pageSize);
            return View(list);
        }
        [HttpGet]
        public IActionResult SearchResult(SearchRoomViewModel vm)
        {
            TempData["Message"] = "";
            if(vm.DateFrom == null || vm.DateTo == null )
            {
                TempData["Message"] = "Không được để check in và check out trống!";
                return View();
            }
            if(vm.DateTo <= vm.DateFrom)
            {
                TempData["Message"] = "check in phải nhỏ hơn check out!";
                return View();
            }    
            HttpContext.Session.Set("checkIn", vm.DateFrom);
            HttpContext.Session.Set("checkOut", vm.DateTo);
            var roomBooked = from b in _context.BookingsRoomDetails
                             where 
                             ((vm.DateFrom >= b.checkIn) && (vm.DateFrom <= b.checkOut)) ||
                             ((vm.DateTo >= b.checkIn) && (vm.DateTo <= b.checkOut)) ||
                             ((vm.DateFrom <= b.checkIn) && (vm.DateTo >= b.checkIn) && (vm.DateTo <= b.checkOut)) ||
                             ((vm.DateFrom >= b.checkIn) && (vm.DateFrom <= b.checkOut) && (vm.DateTo >= b.checkOut)) ||
                             ((vm.DateFrom <= b.checkIn) && (vm.DateTo >= b.checkOut))
                             select b;
            var availableRooms = _context.Rooms.Where(r => !roomBooked.Any(b => b.roomId == r.Id)).ToList();
            foreach(var item in availableRooms)
            {
                if(item.numPeople >= vm.NoOfPeople)
                {
                    vm.Room.Add(item);
                }
            }
            return View(vm);
        }
    }
}
