
using Microsoft.EntityFrameworkCore;

namespace KhachSan.Models
{
    public class QLKSContext : DbContext
    {
        public QLKSContext(DbContextOptions<QLKSContext> options) : base(options) { } 
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingRoomDetail> BookingsRoomDetails { get; set;}
        public DbSet<BookingServiceDetail> BookingsServiceDetails { get; set;}

    
    }
}
