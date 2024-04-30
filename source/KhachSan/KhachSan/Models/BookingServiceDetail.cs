using System.ComponentModel.DataAnnotations;

namespace KhachSan.Models
{
    public class BookingServiceDetail
    {
        [Key]
        public int Id { get; set; }
        public int bookingId { get; set; }
        public Booking Booking { get; set; }
        public int serviceId { get; set; }  
        public Service Service { get; set; }
        public double toTalPrice { get; set; }
    }
}
