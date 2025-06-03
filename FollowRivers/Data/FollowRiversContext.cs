using Microsoft.EntityFrameworkCore;
using FollowRivers.Models;

namespace FollowRivers.Data
{
    public class FollowRiversContext : DbContext
    {
        public FollowRiversContext(DbContextOptions<FollowRiversContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<RiverAddress> RiverAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure 1:N relationship between Person and RiverAddress
            modelBuilder.Entity<Person>()
                .HasMany(p => p.RiverAddresses)
                .WithOne(r => r.Person)
                .HasForeignKey(r => r.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
