using System.Text.Json.Serialization;

namespace Wheels_n_Deals.API.DataLayer.Models
{
    public class AnnouncementImage
    {
        public Guid AnnouncementId { get; set; }
        public Guid ImageId { get; set; }

        [JsonIgnore]
        public Announcement? Announcement { get; set; }
        public Image? Image { get; set; }
    }
}
