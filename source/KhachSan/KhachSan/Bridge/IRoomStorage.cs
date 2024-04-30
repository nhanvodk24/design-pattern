using KhachSan.Models;

namespace KhachSan.Bridge
{
    public interface IRoomStorage
    {
        void AddRoom(Room room, IFormFile Phongimg);
        void EditRoom(string id,Room room, IFormFile Phongimg);
        bool DeleteRoom(String roomId);
    }
}
