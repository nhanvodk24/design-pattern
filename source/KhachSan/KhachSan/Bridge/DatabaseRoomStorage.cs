using KhachSan.Models;
using Microsoft.AspNetCore.Hosting;

namespace KhachSan.Bridge
{
    public class DatabaseRoomStorage : IRoomStorage
    {
        private readonly QLKSContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DatabaseRoomStorage(QLKSContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddRoom(Room room, IFormFile Phongimg)
        {
            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/room");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Phongimg.FileName;
            string filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                Phongimg.CopyTo(fileStream);
            }

            room.image = "/img/room/" + uniqueFileName;
            _context.Rooms.Add(room);
            _context.SaveChanges();
            
        }

        public void EditRoom(String id,Room room2, IFormFile Phongimg)
        {
            int idPhong = Convert.ToInt32(id);

            var room = _context.Rooms.FirstOrDefault(r => r.Id == idPhong);

            if (room != null && Phongimg == null)
            {
                room.name = room2.name;
                room.price = room2.price;
                room.numPeople = room2.numPeople;
                _context.SaveChanges();

            }
            else if (room != null && Phongimg != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/room");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Phongimg.FileName;
                string filePath = Path.Combine(uploadsDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Phongimg.CopyTo(fileStream);
                }
                room.name = room2.name;
                room.price = room2.price;
                room.numPeople = room2.numPeople;
                room.image = "/img/room/" + uniqueFileName;
                _context.SaveChanges();

            }
            
        }

        public bool DeleteRoom(String roomId)
        {
            int idRooom = (int)Convert.ToInt32(roomId);
            var hdPhong = _context.BookingsRoomDetails.Where(r => r.roomId == idRooom).ToList();
            if (hdPhong.Count() > 0)
            {
                return false;
            }
            else
            {
                var room = _context.Rooms.FirstOrDefault(r => r.Id == idRooom);
                _context.Rooms.Remove(room);
                _context.SaveChanges();
                return true;
            }
        }
    }
}
