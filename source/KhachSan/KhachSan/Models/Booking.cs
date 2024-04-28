using System.ComponentModel.DataAnnotations;

namespace KhachSan.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime createdDate { get; set; }
        public double toTalPrice { get; set; }  
        public int userId { get; set; }

        public User User;
        public ICollection<BookingRoomDetail> BookingRoomDetails { get; set;}
        public ICollection<BookingServiceDetail> BookingServiceDetails { get; set;}
    }
}
