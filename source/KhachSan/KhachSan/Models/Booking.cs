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
        public interface IBookingBuilder
        {
            IBookingBuilder WithCreatedDate(DateTime createdDate);
            IBookingBuilder WithTotalPrice(double totalPrice);
            IBookingBuilder WithUserId(int userId);
            IBookingBuilder WithUser(User user);
            IBookingBuilder WithBookingRoomDetails(ICollection<BookingRoomDetail> bookingRoomDetails);
            IBookingBuilder WithBookingServiceDetails(ICollection<BookingServiceDetail> bookingServiceDetails);
            Booking Build();
        }
        public class BookingBuilder : IBookingBuilder
        {
            private Booking _booking;

            public BookingBuilder()
            {
                _booking = new Booking();
            }

            public IBookingBuilder WithCreatedDate(DateTime createdDate)
            {
                _booking.createdDate = createdDate;
                return this;
            }

            public IBookingBuilder WithTotalPrice(double totalPrice)
            {
                _booking.toTalPrice = totalPrice;
                return this;
            }

            public IBookingBuilder WithUserId(int userId)
            {
                _booking.userId = userId;
                return this;
            }

            public IBookingBuilder WithUser(User user)
            {
                _booking.User = user;
                return this;
            }

            public IBookingBuilder WithBookingRoomDetails(ICollection<BookingRoomDetail> bookingRoomDetails)
            {
                _booking.BookingRoomDetails = bookingRoomDetails;
                return this;
            }

            public IBookingBuilder WithBookingServiceDetails(ICollection<BookingServiceDetail> bookingServiceDetails)
            {
                _booking.BookingServiceDetails = bookingServiceDetails;
                return this;
            }

            public Booking Build()
            {
                return _booking;
            }
        }
    }
}
