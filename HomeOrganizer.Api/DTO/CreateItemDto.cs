namespace HomeOrganizer.Api;

using System.ComponentModel.DataAnnotations;
// DTO is a cotract between the API and the client since it represents 
// a shared agreement about how data will be transferred and used. 
public record  CreateItemDto
(
    [Required] string Name,
    [Required] string Description, 
    [Required] DateOnly LastModifiedDate

);