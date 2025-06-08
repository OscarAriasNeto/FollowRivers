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
    }
}
