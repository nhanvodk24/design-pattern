using KhachSan.Models;

namespace KhachSan.Bridge
{
    public class RoomManager
    {
        private readonly IRoomStorage _roomStorage;

        public RoomManager(IRoomStorage roomStorage)
        {
            _roomStorage = roomStorage;
        }

        public void AddRoom(Room room, IFormFile Phongimg)
        {
            _roomStorage.AddRoom(room,  Phongimg);
        }

        public void EditRoom(String id,Room room, IFormFile Phongimg)
        {
            _roomStorage.EditRoom(id,room,  Phongimg);
        }

        public bool DeleteRoom(String roomId)
        {
            return _roomStorage.DeleteRoom(roomId);
        }
    }
}
