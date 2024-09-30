
using CQRSMediatr.Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace CQRSMediatr.DataService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
     
        // define the db entities
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Achievement> Achievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // specified the relationship between the entities
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity
                .HasOne(x => x.Driver)
                .WithMany(x => x.Achievements)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Achievements_Driver")
                ;
            });
        }
    }
}
