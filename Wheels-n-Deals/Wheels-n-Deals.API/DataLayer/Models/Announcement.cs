using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Wheels_n_Deals.API.DataLayer.Models;

public class Announcement
{
    public Guid Id { get; set; }

    [MaxLength(50)] public string Title { get; set; } = string.Empty;

    [MaxLength(1000)] public string Description { get; set; } = string.Empty;

    [MaxLength(50)] public string County { get; set; } = string.Empty;

    [MaxLength(50)] public string City { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }

    [ForeignKey(nameof(Vehicle))]
    [JsonIgnore]
    public Guid VehicleId { get; set; }

    // Relationships
    [JsonIgnore] public User? Owner { get; set; }

    public Vehicle? Vehicle { get; set; }

    [JsonIgnore] public List<AnnouncementImage> AnnouncementImages { get; set; } = new();

    public List<Image> Images { get; set; } = new();
}