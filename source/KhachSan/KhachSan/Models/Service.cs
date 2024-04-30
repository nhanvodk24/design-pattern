using System.ComponentModel.DataAnnotations;

namespace KhachSan.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public double price { get; set; }   
        public string description { get; set; }
        public string image { get; set; }
        public ICollection<BookingServiceDetail> BookingServiceDetails { get; set; } 
        public Service(string name, double price, string description)
        {
            this.name = name;
            this.price = price;
            this.description = description;
        }
    }
}
