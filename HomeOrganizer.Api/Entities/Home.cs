namespace HomeOrganizer.Api.Entities;

public class Home
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<Room> Rooms { get; set; } = [];
}
