namespace HomeOrganizer.Api;

using System.ComponentModel.DataAnnotations;

public record class UpdateItemDto
(
    [Required] string Name,
    [Required] string Description,
    [Required] int Quantity
);
