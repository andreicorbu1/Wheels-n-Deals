using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wheels_n_Deals.API.DataLayer.Entities;

public class Announcement : BaseEntity
{
    public User? User { get; set; }

    public Vehicle? Vehicle { get; set; }

    [MaxLength(150)]public string Title { get; set; } = string.Empty;

    [MaxLength(1000)] public string Description { get; set; } = string.Empty;

    [MaxLength(50)] public string County { get; set; } = string.Empty;

    [MaxLength(50)] public string City { get; set; } = string.Empty;

    public List<Image>? ImagesUrl { get; set; }
}