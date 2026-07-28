namespace HomeOrganizer.Api;

public record  CreateItemDto
(
    string Name,
    string Description, 
    DateOnly LastModifiedDate

);