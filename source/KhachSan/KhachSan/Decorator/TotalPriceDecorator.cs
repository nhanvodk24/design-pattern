using KhachSan.ViewModel;

namespace KhachSan.Decorator
{
    public abstract class TotalPriceDecorator : ITotalPrice
    {
        protected ITotalPrice total;
        public TotalPriceDecorator(ITotalPrice total)   
        {
            this.total = total;
        }
        public ITotalPrice GetTotalPrice()
        {
            return this.total;  
        }
        public void setTotalPrice(ITotalPrice total)
        {
            this.total = total;
        }
        public abstract double calculateTotalPrice(HttpContext httpContext);
    }
}
