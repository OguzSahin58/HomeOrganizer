using HomeOrganizer.Api.Entities;

using Microsoft.EntityFrameworkCore;

namespace HomeOrganizer.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Home> Homes => Set<Home>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Room> Rooms => Set<Room>();
}
