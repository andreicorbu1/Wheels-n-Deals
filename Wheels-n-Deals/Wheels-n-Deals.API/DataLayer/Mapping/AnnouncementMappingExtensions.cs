using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class AnnouncementMappingExtensions
{
    public static AnnouncementDto ToAnnouncementDto(this Announcement announcement)
    {
        var announcementDto = new AnnouncementDto()
        {
            Id = announcement.Id.ToString(),
            User = announcement.User?.ToUserDto(),
            Vehicle = announcement.Vehicle?.ToVehicleDto(),
            Title = announcement.Title,
            Description = announcement.Description,
            County = announcement.County,
            City = announcement.City,
            ImagesUrl = announcement.ImagesUrl
        };
        return announcementDto;
    }

    public static List<AnnouncementDto> ToAnnouncementDtos(this List<Announcement> announcements)
    {
        var announcementDtos = new List<AnnouncementDto>();
        foreach (var announcement in announcements)
        {
            announcementDtos.Add(announcement.ToAnnouncementDto());
        }
        return announcementDtos;
    }
}