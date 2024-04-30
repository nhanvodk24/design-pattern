using KhachSan.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhachSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : Controller
    {
        private readonly QLKSContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RoomController(QLKSContext context, IWebHostEnvironment webHostEnvironment)
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
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/room");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Phongimg.FileName;
                        string filePath = Path.Combine(uploadsDir, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Phongimg.CopyTo(fileStream);
                        }

                        room.image = "/img/room/" + uniqueFileName;
                        _context.Rooms.Add(room);
                        _context.SaveChanges();

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
                    int idPhong = Convert.ToInt32(PhongID);
                    var room = _context.Rooms.FirstOrDefault(r => r.Id == idPhong);
                    if (room != null && Phongimg == null)
                    {
                        room.name = PhongTen;
                        room.price = Convert.ToDouble(PhongGia);
                        room.numPeople = Convert.ToInt32(PhongSoLuongToiDa);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Room", new { area = "Admin" });

                    }
                    else if (room != null && Phongimg != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/room");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Phongimg.FileName;
                        string filePath = Path.Combine(uploadsDir, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Phongimg.CopyTo(fileStream);
                        }
                        room.name = PhongTen;
                        room.price = Convert.ToDouble(PhongGia);
                        room.numPeople = Convert.ToInt32(PhongSoLuongToiDa);
                        room.image = "/img/room/" + uniqueFileName;
                        _context.SaveChanges();

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
                int idRooom = (int)Convert.ToInt32(id);
                var hdPhong = _context.BookingsRoomDetails.Where(r => r.roomId == idRooom).ToList();
                if (hdPhong.Count() > 0)
                {
                    TempData["Message"] = "Không xóa được phòng này!";
                    return RedirectToAction("Index", "Room", new { area = "Admin" });
                }
                else
                {
                    var room = _context.Rooms.FirstOrDefault(r => r.Id == idRooom);
                    _context.Rooms.Remove(room);
                    _context.SaveChanges();
                    TempData["Message"] = "Phòng đã được xóa!";
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
