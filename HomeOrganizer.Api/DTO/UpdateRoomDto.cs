namespace HomeOrganizer.Api;

using System.ComponentModel.DataAnnotations;

public record class UpdateRoomDto
(
    [Required] string Name,
    [Required] int PositionX,
    [Required] int PositionY,
    [Required] int Width,
    [Required] int Height
);
