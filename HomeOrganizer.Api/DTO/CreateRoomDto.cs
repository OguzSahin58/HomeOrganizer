namespace HomeOrganizer.Api;

using System.ComponentModel.DataAnnotations;

public record CreateRoomDto
(
    [Required] int HomeId, 
    [Required] string Name,
    [Required] int PositionX,
    [Required] int PositionY,
    [Required] int Width,
    [Required] int Height
);