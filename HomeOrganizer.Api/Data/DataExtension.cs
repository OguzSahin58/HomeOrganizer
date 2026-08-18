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
            if (!context.Set<StorageUnit>().Any())
            {
                context.Set<StorageUnit>().AddRange(
                    new StorageUnit { Id = 1, RoomId = 1, Name = "Box", Type = Enums.StorageUnitType.Box, PositionX = 50, PositionY = 50, Width = 100, Height = 200 },
                    new StorageUnit { Id = 2, RoomId = 1, Name = "Cabinet", Type = Enums.StorageUnitType.Cabinet, PositionX = 200, PositionY = 50, Width = 150, Height = 100 },
                    new StorageUnit { Id = 3, RoomId = 2, Name = "DrawerUnit", Type = Enums.StorageUnitType.DrawerUnit, PositionX = 50, PositionY = 50, Width = 200, Height = 300 },
                    new StorageUnit { Id = 4, RoomId = 2, Name = "Shelf", Type = Enums.StorageUnitType.Shelf, PositionX = 300, PositionY = 50, Width = 100, Height = 200 }
                );
                context.SaveChanges();
            }
            if(!context.Set<Item>().Any())
            {
                context.Set<Item>().AddRange(
                    new Item { Id = 1, StorageUnitId = 1, Name = "Bavul", Description = "Item container", Quantity = 1 },
                    new Item { Id = 2, StorageUnitId = 1, Name = "Canta", Description = "Item container", Quantity = 1 },
                    new Item { Id = 3, StorageUnitId = 2, Name = "Ayakkabi", Description = "Item container", Quantity = 2 },
                    new Item { Id = 4, StorageUnitId = 3, Name = "Sapka", Description = "Item container", Quantity = 1 }
                );
                context.SaveChanges();
            }
            
        })
);
    }
}
