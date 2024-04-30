using KhachSan.Commands;
using KhachSan.Models;

namespace KhachSan.Factory
{
    public class FactoryCreate
    {
        public IBookingCommand createBookingCommnd(string type,DateTime checkIn,DateTime checkOut, QLKSContext context, HttpContext httpContext, int roomId)
        {
            if (type == "booking") {

                return new BookingCommand(context, httpContext, roomId);
                }
            
            else
                return new CancelBookingRoom( context, httpContext, roomId,  checkIn, checkOut);
        }
    }
}
