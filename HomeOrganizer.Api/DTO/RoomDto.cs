namespace HomeOrganizer.Api;

public record class RoomDto
(
    int Id,
    int HomeId,
    string Name,
    int positionX,
    int positionY,
    int width,
    int height
);