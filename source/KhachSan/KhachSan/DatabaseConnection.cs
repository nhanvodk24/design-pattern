using KhachSan.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KhachSan
{
    public class DatabaseConnection
    {
        private static QLKSContext instance;

        public static QLKSContext GetInstance(string connectionString)
        {
            if (instance == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<QLKSContext>();
                optionsBuilder.UseSqlServer(connectionString);
                instance = new QLKSContext(optionsBuilder.Options);
            }
            return instance;
        }
    }
}
