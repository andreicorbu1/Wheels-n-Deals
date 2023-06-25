using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class ImageDto
{
    [Required][MaxLength(255)] public string Url { get; set; } = string.Empty;
}