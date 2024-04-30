using KhachSan.Models;
using Microsoft.EntityFrameworkCore;

namespace KhachSan
{
    public class DatabaseConnection
    {
        private static QLKSContext instance;
        private static readonly object lockObject = new object();

        public static QLKSContext GetInstance(string connectionString)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<QLKSContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    instance = new QLKSContext(optionsBuilder.Options);
                }
            }
            return instance;
        }
    }
}
