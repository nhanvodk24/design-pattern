using KhachSan.Commands;
using KhachSan.Extension;
using KhachSan.Factory;
using KhachSan.Models;
using KhachSan.Observer;
using KhachSan.ViewModel;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication.ExtendedProtection;
using System.Windows.Input;

namespace KhachSan.Controllers
{
    public class CartController : Controller, ICartSubject
    {
        private readonly QLKSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IBookingCommand _command;
        private FactoryCreate factory = new FactoryCreate();
        private readonly List<ICartObserver> _cartObservers = new List<ICartObserver>();
        public CartController(QLKSContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            AttachObserver(new CartObserver(this));
        }
        public void AttachObserver(ICartObserver observer)
        {
            _cartObservers.Add(observer);
        }

        public void DetachObserver(ICartObserver observer)
        {
            _cartObservers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _cartObservers)
            {
                observer.UpdateCart();
            }
        }
        public IActionResult Index()
        {
            return View();
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
        public IActionResult AddRoom(int roomId)
        {
            DateTime checkIn = DateTime.Now; 
            DateTime checkOut = DateTime.Now; 
            if (HttpContext.Session.GetString("username") != null)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    var session = httpContext.Session;
                   
                }
                IBookingCommand temp = factory.createBookingCommnd("booking", checkIn, checkOut, _context, httpContext, roomId);
                _command = temp;
                _command.execute();
                NotifyObservers();
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
                NotifyObservers();
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
                NotifyObservers();
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
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    var session = httpContext.Session;

                }
                _command = factory.createBookingCommnd("cancel", (DateTime)checkIn, (DateTime)checkOut, _context, httpContext, id);
                _command.execute();
                NotifyObservers();
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}
