using Microsoft.EntityFrameworkCore;

namespace Scheduler.Data
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options) : base(options) { }
    }
}
