using HomeOrganizer.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeOrganizer.Api.Data;

public static class DataExtension
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }

    public static void AddSeedingDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
        options => options
        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding((context, _) =>
        {
            if(!context.Set<Home>().Any())
            {
                context.Set<Home>().AddRange(
                    new Home { Id = 1, Name = "My Home", Description = "123 Main St" },
                    new Home { Id = 2, Name = "Hometown", Description = "456 Oak Ave" },
                    new Home { Id = 3, Name = "Summer House", Description = "789 Pine Rd" },
                    new Home { Id = 4, Name = "Winter Retreat", Description = "123 Main St" }
                );
                context.SaveChanges();
            }
            if(!context.Set<Item>().Any())
            {
                context.Set<Item>().AddRange(
                    new Item { Id = 1, Name = "Bavul", Description = "Item container", LastModifiedDate = DateOnly.FromDateTime(DateTime.Now)},
                    new Item { Id = 2, Name = "Canta", Description = "Item container", LastModifiedDate = DateOnly.FromDateTime(DateTime.Now)},
                    new Item { Id = 3, Name = "Ayakkabi", Description = "Item container", LastModifiedDate = DateOnly.FromDateTime(DateTime.Now)},
                    new Item { Id = 4, Name = "Sapka", Description = "Item container", LastModifiedDate = DateOnly.FromDateTime(DateTime.Now)}
                );
                context.SaveChanges();
            }
            
            if(!context.Set<Room>().Any())
            {
                context.Set<Room>().AddRange(
                    new Room { Id = 1, HomeId = 1, Name = "Living Room", PositionX = 0, PositionY = 0, Width = 500, Height = 400 },
                    new Room { Id = 2, HomeId = 1, Name = "Kitchen", PositionX = 500, PositionY = 0, Width = 300, Height = 400 },
                    new Room { Id = 3, HomeId = 2, Name = "Bedroom", PositionX = 0, PositionY = 0, Width = 400, Height = 300 },
                    new Room { Id = 4, HomeId = 2, Name = "Bathroom", PositionX = 400, PositionY = 0, Width = 200, Height = 300 }
                );
                context.SaveChanges();
            }   
            
        })
);
    }
}
