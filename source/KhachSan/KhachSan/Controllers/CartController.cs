using KhachSan.Extension;
using KhachSan.Models;
using KhachSan.ViewModel;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication.ExtendedProtection;

namespace KhachSan.Controllers
{
    public class CartController : Controller
    {
        private readonly QLKSContext _context;
        public CartController(QLKSContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<CartRoomViewModel> getCartRooms
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartRoomViewModel>>("CartRoom");
                if (data == null)
                {
                    data = new List<CartRoomViewModel>();
                }
                return data;
            }

        }
        public List<CartServiceViewModel> getCartServices
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartServiceViewModel>>("CartService");
                if (data == null)
                {
                    data = new List<CartServiceViewModel>();
                }
                return data;
            }

        }
        public IActionResult AddRoom(int roomId)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var cartRoom = getCartRooms;
                var room = cartRoom.SingleOrDefault(s => s.roomId == roomId);
                if (room == null)
                {
                    var r = _context.Rooms.SingleOrDefault(r => r.Id == roomId);
                    room = new CartRoomViewModel
                    {
                        roomId = roomId,
                        roomName = r.name,
                        price = r.price,
                        checkIn = HttpContext.Session.Get<DateTime>("checkIn"),
                        checkOut = HttpContext.Session.Get<DateTime>("checkOut")
                    };
                    cartRoom.Add(room);
                }
                else
                {
                    DateTime checkIn = HttpContext.Session.Get<DateTime>("checkIn");
                    DateTime checkOut = HttpContext.Session.Get<DateTime>("checkOut");
                    if ((checkIn < room.checkIn && checkOut < room.checkIn) || (checkIn > room.checkOut && checkOut > room.checkOut))
                    {
                        var r = _context.Rooms.SingleOrDefault(r => r.Id == roomId);
                        var newRoom = new CartRoomViewModel
                        {
                            roomId = roomId,
                            roomName = r.name,
                            price = r.price,
                            checkIn = HttpContext.Session.Get<DateTime>("checkIn"),
                            checkOut = checkOut = HttpContext.Session.Get<DateTime>("checkOut")
                        };
                        cartRoom.Add(newRoom);
                    }
                }
                HttpContext.Session.Set("CartRoom", cartRoom);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult AddService(int serviceId)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var cartService = getCartServices;
                var service = cartService.SingleOrDefault(s => s.serviceId == serviceId);
                if (service == null)
                {
                    var s = _context.Services.SingleOrDefault(s => s.Id == serviceId);
                    service = new CartServiceViewModel
                    {
                        serviceId = serviceId,
                        serviceName = s.name,
                        servicePrice = s.price
                    };
                    cartService.Add(service);
                }
                HttpContext.Session.Set("CartService", cartService);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult RemoveFromCartService(int id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var cartService = getCartServices;
                for (int i = 0; i < cartService.Count; i++)
                {
                    if (cartService[i].serviceId == id)
                    {
                        cartService.Remove(cartService[i]);
                    }
                }
                HttpContext.Session.Set("CartService", cartService);
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult RemoveFromCartRoom(int id, DateTime checkIn, DateTime checkOut)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var cartRoom = getCartRooms;
                for (int i = 0; i < cartRoom.Count; i++)
                {
                    if (cartRoom[i].roomId == id && cartRoom[i].checkIn == checkIn && cartRoom[i].checkOut == checkOut)
                    {
                        cartRoom.Remove(cartRoom[i]);
                    }
                }
                HttpContext.Session.Set("CartRoom", cartRoom);
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}
