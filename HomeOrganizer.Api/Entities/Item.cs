namespace HomeOrganizer.Api.Entities;

public class Item
{
    public int Id { get; set; }

    public int StorageUnitId { get; set; }

    public StorageUnit StorageUnit { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;
}
