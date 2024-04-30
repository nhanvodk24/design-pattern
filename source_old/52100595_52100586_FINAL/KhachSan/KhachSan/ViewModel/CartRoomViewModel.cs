using JetBrains.Annotations;

namespace KhachSan.ViewModel
{
    public class CartRoomViewModel
    {
        public int roomId { get; set; }
        public string roomName { get; set; }
        public double price { get; set; }
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public double totalPrice => (int)(((TimeSpan)(checkOut - checkIn)).Days) * price;
         
    }
}
