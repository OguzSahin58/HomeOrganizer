namespace HomeOrganizer.Api;

// DTO is a cotract between the API and the client since it represents 
// a shared agreement about how data will be transferred and used. 

public record class ItemDto
(
    int Id,
    int StorageUnitId,
    string Name,
    string Description,
    int Quantity
);
