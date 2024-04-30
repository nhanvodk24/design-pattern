using KhachSan.ViewModel;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using KhachSan.Extension;

namespace KhachSan.Decorator
{
    public class totalWService : TotalPriceDecorator
    {
        public totalWService(ITotalPrice total) : base(total) { }
        public override double calculateTotalPrice(HttpContext httpContext)
        {
            double price = total.calculateTotalPrice(httpContext);
            return price + addServicePrice(httpContext);
        }
        public double addServicePrice(HttpContext httpContext)
        {
            var cartService = httpContext.Session.Get<List<CartServiceViewModel>>("CartService");
            double totalPrice = 0.0;
            foreach (var csvm in cartService)
            {
                totalPrice += csvm.servicePrice;
            }
            return totalPrice;
        }
    }

}
