using KhachSan.Models;
using KhachSan.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KhachSan.Extension;

namespace KhachSan.Controllers
{
    public class BookingController : Controller
    {
        private readonly QLKSContext _context;
        public BookingController(QLKSContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var userId = (int)Convert.ToInt32(HttpContext.Session.GetString("userId"));
                var listBooking = _context.Bookings.Where(x => x.userId == userId).ToList();
                return View(listBooking);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult CheckOut(double totalPrice)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                if(HttpContext.Session.Get<List<CartServiceViewModel>>("CartService") == null && HttpContext.Session.Get<List<CartServiceViewModel>>("RoomService") == null)
                {
                    return RedirectToAction("Index","Room");
                }
                var userId = (int)Convert.ToInt32(HttpContext.Session.GetString("userId"));
                Booking booking = new Booking();
                booking.createdDate = DateTime.Now;
                booking.toTalPrice = totalPrice;
                booking.userId = userId;
                _context.Add(booking);
                _context.SaveChanges();
                var cartService = HttpContext.Session.Get<List<CartServiceViewModel>>("CartService");
                if(cartService != null)
                {
                    foreach (var service in cartService)
                    {
                        BookingServiceDetail bookingServiceDetail = new BookingServiceDetail();
                        bookingServiceDetail.bookingId = booking.Id;
                        bookingServiceDetail.serviceId = service.serviceId;
                        bookingServiceDetail.toTalPrice = service.servicePrice;
                        _context.Add(bookingServiceDetail);
                    }
                    _context.SaveChanges();
                }
                var cartRoom = HttpContext.Session.Get<List<CartRoomViewModel>>("CartRoom");
                if(cartRoom != null)
                {
                    foreach (var room in cartRoom)
                    {
                        BookingRoomDetail bookingRoomDetail = new BookingRoomDetail();
                        bookingRoomDetail.bookingId = booking.Id;
                        bookingRoomDetail.roomId = room.roomId;
                        bookingRoomDetail.checkIn = room.checkIn;
                        bookingRoomDetail.checkOut = room.checkOut;
                        bookingRoomDetail.toTalPrice = room.totalPrice;
                        _context.Add(bookingRoomDetail);
                    }
                    _context.SaveChanges();
                }
                HttpContext.Session.Remove("CartService");
                HttpContext.Session.Remove("CartRoom");
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult ViewDetail(int id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewData["bookId"] = id;
                List<BookingRoomDetail> bookingRoomDetails = new List<BookingRoomDetail>();
                bookingRoomDetails = _context.BookingsRoomDetails.Where(x => x.bookingId == id).ToList();
                ViewData["bookingRoomDetails"] = bookingRoomDetails;
                List<BookingServiceDetail> bookingServiceDetails = new List<BookingServiceDetail>();
                bookingServiceDetails = _context.BookingsServiceDetails.Where(x => x.bookingId == id).ToList();
                ViewData["bookingServiceDetails"] = bookingServiceDetails;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}
