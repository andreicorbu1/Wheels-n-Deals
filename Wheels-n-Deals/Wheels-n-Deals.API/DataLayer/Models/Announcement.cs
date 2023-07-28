using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public Guid VehicleId { get; set; }

    // Relationships
    public User Owner { get; set; }
    public Vehicle Vehicle { get; set; }
    public ICollection<Image> Images { get; set; }
}
