namespace HomeOrganizer.Api;

using System.ComponentModel.DataAnnotations;

// DTO is a cotract between the API and the client since it represents 
// a shared agreement about how data will be transferred and used. 

public record class HomeDto
(
    int Id,
    [Required] string Name,
    string Description
);
