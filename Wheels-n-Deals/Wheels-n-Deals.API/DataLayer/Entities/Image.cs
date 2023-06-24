using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.Entities
{
    public class Image : BaseEntity
    {
        [MaxLength(255)] public string Url { get; set; } = string.Empty;
    }
}