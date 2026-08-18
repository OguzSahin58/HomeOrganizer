using HomeOrganizer.Api.Enums;

namespace HomeOrganizer.Api;

public record class StorageUnitDto
(
    int Id,
    int RoomId,
    string Name,
    StorageUnitType Type,
    int PositionX,
    int PositionY,
    int Width,
    int Height
);