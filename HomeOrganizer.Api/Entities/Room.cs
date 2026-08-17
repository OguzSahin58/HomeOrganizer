namespace HomeOrganizer.Api.Entities;

public class Room
{
    public int Id { get; set; }
    public int HomeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
