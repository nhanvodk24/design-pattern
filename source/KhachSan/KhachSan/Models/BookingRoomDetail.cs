using System.ComponentModel.DataAnnotations;

namespace KhachSan.Models
{
    public class BookingRoomDetail
    {
        [Key]
        public int Id { get; set; }
        public int bookingId { get; set; }
        public Booking Booking { get; set; }
        public int roomId { get; set; } 
        public Room room { get; set; }  
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public double toTalPrice { get; set; }
    }
}
