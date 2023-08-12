using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.DTO;

public class UpdateAnnouncementDto
{
    [Required][MaxLength(50)] public string VinNumber { get; set; } = string.Empty;

    [Required][MaxLength(150)] public string Title { get; set; } = string.Empty;

    [Required][MaxLength(1000)] public string Description { get; set; } = string.Empty;

    [Required][MaxLength(50)] public string County { get; set; } = string.Empty;

    [Required][MaxLength(50)] public string City { get; set; } = string.Empty;

    public List<ImageDto>? ImagesUrl { get; set; }
}