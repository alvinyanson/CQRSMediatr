
using CQRSMediatr.Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace CQRSMediatr.DataService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Achievement> Achievements { get; set; }
    }
}
