using KhachSan.Models;

namespace KhachSan.ViewModel
{
    public class SearchRoomViewModel
    {
        public DateTime? DateFrom { get; set; } = null;
        public DateTime? DateTo { get; set; } = null;
        public int NoOfPeople { get; set; }
        public IList<Room> Room { get; set; } = new List<Room>();
    }
}
