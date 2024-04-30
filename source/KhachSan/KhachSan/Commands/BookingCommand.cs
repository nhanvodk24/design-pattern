using KhachSan.Extension;
using KhachSan.Models;
using KhachSan.ViewModel;

namespace KhachSan.Commands
{
    public class BookingCommand : IBookingCommand
    {
        private readonly QLKSContext _context;
        private readonly HttpContext _httpContext;
        private int roomId;
        public BookingCommand(QLKSContext context,HttpContext httpContext, int roomId)
        {
            _context = context;
            _httpContext = httpContext;
            this.roomId = roomId;
        }


        public void execute()
        {
            var data = _httpContext.Session.Get<List<CartRoomViewModel>>("CartRoom");
            if (data == null)
            {
                data = new List<CartRoomViewModel>();
            }
            var cartRoom = data;
            var room = cartRoom.SingleOrDefault(s => s.roomId == this.roomId);
            if (room == null)
            {
                var r = _context.Rooms.SingleOrDefault(r => r.Id == this.roomId);
                room = new CartRoomViewModel
                {
                    roomId = this.roomId,
                    roomName = r.name,
                    price = r.price,
                    checkIn = _httpContext.Session.Get<DateTime>("checkIn"),
                    checkOut = _httpContext.Session.Get<DateTime>("checkOut")
                };
                cartRoom.Add(room);
            }
            else
            {
                DateTime checkIn = _httpContext.Session.Get<DateTime>("checkIn");
                DateTime checkOut = _httpContext.Session.Get<DateTime>("checkOut");
                if ((checkIn < room.checkIn && checkOut < room.checkIn) || (checkIn > room.checkOut && checkOut > room.checkOut))
                {
                    var r = _context.Rooms.SingleOrDefault(r => r.Id == this.roomId);
                    var newRoom = new CartRoomViewModel
                    {
                        roomId = this.roomId,
                        roomName = r.name,
                        price = r.price,
                        checkIn = _httpContext.Session.Get<DateTime>("checkIn"),
                        checkOut = checkOut = _httpContext.Session.Get<DateTime>("checkOut")
                    };
                    cartRoom.Add(newRoom);
                }
            }
            _httpContext.Session.Set("CartRoom", cartRoom);
        }
    }
}
