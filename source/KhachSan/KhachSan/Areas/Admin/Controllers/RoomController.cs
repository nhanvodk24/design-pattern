using KhachSan.Bridge;
using KhachSan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KhachSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : Controller
    {
        private readonly QLKSContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IRoomStorage roomStorage;

        private RoomManager _roomManager ;
        public RoomController(QLKSContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            roomStorage = new DatabaseRoomStorage(_context, _webHostEnvironment);
            _roomManager = new RoomManager(roomStorage);
        }
        public IActionResult Index(int? page)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                int currentPage = page ?? 1;
                int pageSize = 3;
                int totalRooms = _context.Rooms.Count();
                int totalPages = (int)Math.Ceiling(totalRooms / (double)pageSize);
                if (currentPage < 1 || currentPage > totalPages)
                {
                    currentPage = 1;
                }
                int skip = (currentPage - 1) * pageSize;
                var rooms = _context.Rooms
                    .OrderBy(r => r.Id)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                ViewBag.CurrentPage = currentPage;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                return View(rooms);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult AddRoom()
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
        public IActionResult AddRoom(string PhongTen, string PhongGia, string PhongSoLuongToiDa, IFormFile Phongimg)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
                {
                    double Gia = Convert.ToDouble(PhongGia);
                    int SoLuong = Convert.ToInt32(PhongSoLuongToiDa);
                    Room room = new Room(PhongTen, Gia, SoLuong);
                    if (Phongimg != null)
                    {
                        _roomManager.AddRoom(room, Phongimg);
                        return RedirectToAction("Index", "Room", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("AddRoom", "Room", new { area = "Admin" });
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("AddRoom", "Room", new { area = "Admin" });
                }
            }
            else 
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public IActionResult EditRoom(string id)
        {
            try
            {
                int idPhong = (int)Convert.ToInt32(id);
                var room = _context.Rooms.FirstOrDefault(r => r.Id == idPhong);
                return View(room);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Room", new { area = "Admin" });

            }
        }
        [HttpPost]
        public IActionResult EditRoom(String PhongID, string PhongTen, string PhongGia, string PhongSoLuongToiDa, IFormFile Phongimg)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                try
                {
                    var room = new Room(PhongTen, Convert.ToDouble(PhongGia), Convert.ToInt32(PhongSoLuongToiDa));
                    if (room != null && Phongimg == null)
                    {
                        _roomManager.EditRoom(PhongID, room, Phongimg);
                        return RedirectToAction("Index", "Room", new { area = "Admin" });

                    }
                    else if (room != null && Phongimg != null)
                    {
                        _roomManager.EditRoom(PhongID, room, Phongimg);
                        return RedirectToAction("Index", "Room", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("EditRoom", "Room", new { area = "Admin", id = PhongID });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("EditRoom", "Room", new { area = "Admin", id = PhongID });

                }
            }
            else 
            {
                return RedirectToAction("Index", "Room", new { area = "Admin" });
            }
        }
        public IActionResult RemoveRoom(string id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                TempData["Message"] = "";
                if (_roomManager.DeleteRoom(id))
                {
                    TempData["Message"] = "Phòng đã được xóa!";
                    return RedirectToAction("Index", "Room", new { area = "Admin" });
                }
                else
                {
                    TempData["Message"] = "Không xóa được phòng này!";
                    return RedirectToAction("Index", "Room", new { area = "Admin" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Room", new { area = "Admin" });
            }
        }
    }
}
