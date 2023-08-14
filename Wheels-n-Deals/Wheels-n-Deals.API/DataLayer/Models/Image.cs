using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Wheels_n_Deals.API.DataLayer.Models;

[Index(nameof(ImageUrl), IsUnique = true)]
public class Image
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    // Relationships
    [JsonIgnore] public List<AnnouncementImage> AnnouncementImages { get; set; } = new();

    [JsonIgnore] public List<Announcement> Announcements { get; set; } = new();
}