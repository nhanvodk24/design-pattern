using KhachSan.Controllers;

namespace KhachSan.Observer
{
    public class CartObserver : ICartObserver
    {
        private readonly CartController _cartController;

        public CartObserver(CartController cartController)
        {
            _cartController = cartController;
        }

        public void UpdateCart()
        {
            var cartServices = _cartController.getCartServices;
            var cartRooms = _cartController.getCartRooms;

            int totalRooms = cartRooms.Count;
            int totalServices = cartServices.Count;

            var httpContext = _cartController.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.SetInt32("TotalRooms", totalRooms);
                httpContext.Session.SetInt32("TotalServices", totalServices);
            }
        }
    }
}
