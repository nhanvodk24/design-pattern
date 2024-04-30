using KhachSan.Extension;
using KhachSan.ViewModel;
using Microsoft.AspNetCore.Http;

namespace KhachSan.Decorator
{
    public class TotalPrice : ITotalPrice
    {
        public double calculateTotalPrice(HttpContext httpContext)
        {
            var cartRoom = httpContext.Session.Get<List<CartRoomViewModel>>("CartRoom");
            double totalPrice = 0.0;
            foreach (var crvm in cartRoom)
            {
                totalPrice += crvm.totalPrice;
            }
            return totalPrice;
        }
    }
}
