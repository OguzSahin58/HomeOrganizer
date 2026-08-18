using HomeOrganizer.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeOrganizer.Api;

public record CreateStorageUnitDto
(
    [Required] string Name,
    [Required] StorageUnitType Type,
    [Required] int PositionX,
    [Required] int PositionY,
    [Required] int Width,
    [Required] int Height
);