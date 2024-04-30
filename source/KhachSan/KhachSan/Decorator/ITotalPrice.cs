using KhachSan.ViewModel;

namespace KhachSan.Decorator
{
    public interface ITotalPrice
    {
        double calculateTotalPrice(HttpContext httpContext); 
    }
}
