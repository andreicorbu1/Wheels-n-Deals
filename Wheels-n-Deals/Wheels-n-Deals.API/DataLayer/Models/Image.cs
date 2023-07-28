namespace Wheels_n_Deals.API.DataLayer.Models;

public class Image
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }

    // Relationships
    public ICollection<Announcement> Announcements { get; set; }
}
