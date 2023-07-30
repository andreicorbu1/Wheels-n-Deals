using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Wheels_n_Deals.API.DataLayer.Models;

public class Announcement
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Title { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }
    [MaxLength(50)]
    public string County { get; set; }
    [MaxLength(50)]
    public string City { get; set; }
    public DateTime DateCreated { get; set; }

    [ForeignKey(nameof(Vehicle))]
    [JsonIgnore]
    public Guid VehicleId { get; set; }

    // Relationships
    [JsonIgnore]
    public User Owner { get; set; }
    public Vehicle Vehicle { get; set; }
    public ICollection<AnnouncementImage> Images { get; set; }
}
