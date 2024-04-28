using System.ComponentModel.DataAnnotations;

namespace KhachSan.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }    
        public string image { get; set; }

        public double price { get; set; }

        public int numPeople { get; set; }
        public ICollection<BookingRoomDetail> BookingRoomDetails { get; set; }
        public Room(string name, double price, int numPeople)
        {
            this.name = name;
            this.price = price;
            this.numPeople = numPeople;
        }
    }
}
