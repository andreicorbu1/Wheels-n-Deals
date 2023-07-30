using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Wheels_n_Deals.API.DataLayer.Models;

[Index(nameof(ImageUrl), IsUnique = true)]
public class Image
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }

    // Relationships
    [JsonIgnore]
    public ICollection<Announcement> Announcements { get; set; }
}
