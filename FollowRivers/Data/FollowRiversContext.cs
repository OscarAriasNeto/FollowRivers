using FollowRivers.Models;
using Microsoft.EntityFrameworkCore;

namespace FollowRivers.Data;

public class FollowRiversContext : DbContext
{
    public FollowRiversContext(DbContextOptions<FollowRiversContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons => Set<Person>();
    public DbSet<RiverAddress> RiverAddresses => Set<RiverAddress>();
    public DbSet<FloodAlert> FloodAlerts => Set<FloodAlert>();
}
