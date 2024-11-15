using Examples.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace Examples.Data
{
    public class ExampleDbContext(DbContextOptions<ExampleDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ExDbParticipant> Participants { get; set; }
        public DbSet<ExDbHistory> Histories { get; set; }
    }
}
