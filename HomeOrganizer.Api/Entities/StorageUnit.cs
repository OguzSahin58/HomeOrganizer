
using HomeOrganizer.Api.Enums;

namespace HomeOrganizer.Api.Entities;

public class StorageUnit
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public Room Room { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public StorageUnitType Type { get; set; }

    public int PositionX { get; set; }

    public int PositionY { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public List<Item> Items { get; set; } = [];
}
