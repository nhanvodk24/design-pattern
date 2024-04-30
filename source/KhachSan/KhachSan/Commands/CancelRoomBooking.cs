using KhachSan.Extension;
using KhachSan.Models;
using KhachSan.ViewModel;

namespace KhachSan.Commands
{
    public class CancelBookingRoom : IBookingCommand
    {
        private readonly QLKSContext _context;
        private readonly HttpContext _httpContext;
        private DateTime checkIn;
        private DateTime checkOut;
        private int id;

        public CancelBookingRoom(QLKSContext context, HttpContext httpContext,int id,DateTime checkIn,DateTime checkOut)
        {
            _context = context;
            _httpContext = httpContext;
            this.id = id;
            this.checkIn = checkIn;
            this.checkOut = checkOut;
        }


        public void execute()
        {
            var data = _httpContext.Session.Get<List<CartRoomViewModel>>("CartRoom");
            if (data == null)
            {
                data = new List<CartRoomViewModel>();
            }
            if (_httpContext.Session.GetString("username") != null)
            {
                var cartRoom = data;
                for (int i = 0; i < cartRoom.Count; i++)
                {
                    if (cartRoom[i].roomId == id && cartRoom[i].checkIn == checkIn && cartRoom[i].checkOut == checkOut)
                    {
                        cartRoom.Remove(cartRoom[i]);
                    }
                }
                _httpContext.Session.Set("CartRoom", cartRoom);
            }
        }
    }
}
