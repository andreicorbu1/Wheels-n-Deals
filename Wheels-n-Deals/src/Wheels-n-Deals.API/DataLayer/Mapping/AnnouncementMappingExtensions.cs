using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class AnnouncementMappingExtensions
{
    public static AnnouncementDto ToAnnouncementDto(this Announcement announcement)
    {
        var annDto = new AnnouncementDto
        {
            Id = announcement.Id,
            DateCreated = announcement.DateCreated,
            DateModified = announcement.DateModified,
            City = announcement.City,
            County = announcement.County,
            Description = announcement.Description,
            Images = announcement.Images.ToStringList(),
            Title = announcement.Title,
            User = announcement.Owner.ToUserDto(),
            Vehicle = announcement.Vehicle.ToVehicleDto(),
        };
        return annDto;
    }

    public static List<AnnouncementDto> ToAnnouncementDtos(this List<Announcement> announcements)
    {
        return (from announcement in announcements
                let announcementDto = announcement.ToAnnouncementDto()
                select announcementDto).ToList();
    }
}
