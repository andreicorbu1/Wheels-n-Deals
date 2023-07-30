namespace Wheels_n_Deals.API.DataLayer.Models;

public class AnnouncementImage
{
    public Guid AnnouncementId { get; set; }
    public Announcement Announcement { get; set; }

    public Guid ImageId { get; set; }
    public Image Image { get; set; }
}
