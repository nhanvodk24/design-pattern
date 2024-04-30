using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace KhachSan.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string password { get; set; }    
        public string role { get; set; }
        public ICollection<Booking> Bookings { get; set; }  
        public User(string name, string address, string username, string password, string role)
        {
            this.name = name;
            this.address = address;
            this.username = username;
            this.password = password;
            this.role = role;
        }
        public User() { }   
    }
}
