namespace HomeOrganizer.Api;

using System.ComponentModel.DataAnnotations;

public record class UpdateHomeDto
(
    [Required] string Name,
    string Description
);
