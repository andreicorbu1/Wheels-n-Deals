using System.ComponentModel.DataAnnotations;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class AddAnnouncementDto
{
    [Required]public Guid UserId { get; set; }

    [Required][MaxLength(50)] public string VinNumber { get; set; } = string.Empty;

    [Required][MaxLength(150)] public string Title { get; set; } = string.Empty;

    [Required][MaxLength(1000)] public string Description { get; set; } = string.Empty;

    [Required][MaxLength(50)] public string County { get; set; } = string.Empty;

    [Required][MaxLength(50)] public string City { get; set; } = string.Empty;

    public List<Image>? ImagesUrl { get; set; }
}