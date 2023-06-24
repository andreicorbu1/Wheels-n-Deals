using System.ComponentModel.DataAnnotations;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class UpdateAnnouncementDto
{
    [MaxLength(50)] public Vehicle? Vehicle { get; set; }

    [MaxLength(150)] public string Title { get; set; } = string.Empty;

    [MaxLength(1000)] public string Description { get; set; } = string.Empty;

    [MaxLength(50)] public string County { get; set; } = string.Empty;

    [MaxLength(50)] public string City { get; set; } = string.Empty;

    public List<Image>? ImagesUrl { get; set; }
}