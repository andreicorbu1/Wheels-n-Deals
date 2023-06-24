using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Mapping;
using Wheels_n_Deals.API.Infrastructure.Exceptions;

namespace Wheels_n_Deals.API.Services;

public class AnnouncementService
{
    private readonly UnitOfWork _unitOfWork;

    public AnnouncementService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Announcement> GetAnnouncementById(Guid id)
    {
        return await _unitOfWork.Announcements.GetById(id) ?? throw new ResourceMissingException($"Announcement with id '{id}' doesn't exist");
    }

    public async Task<Guid> CreateAnnouncement(AddAnnouncementDto addAnnouncementDto)
    {
        var user = await _unitOfWork.Users.GetById(addAnnouncementDto.UserId);
        var vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(addAnnouncementDto.VinNumber);

        if(user is null)
        {
            throw new ResourceMissingException($"User with id '{addAnnouncementDto.UserId}' doesn't exist");
        }

        if(vehicle is null || (vehicle?.Owner?.Id != user.Id && vehicle?.Owner?.RoleType != DataLayer.Enums.Role.Administrator))

        {
            throw new ResourceMissingException($"Vehicle with id '{addAnnouncementDto.VinNumber}' doesn't exist");
        }

        if(vehicle?.Owner?.Id != user.Id && vehicle?.Owner?.RoleType != DataLayer.Enums.Role.Administrator)
        {
            throw new ForbiddenException($"User with id '{user.Id}' doesn't have acces to this operation!");
        }

        var newAnnouncement = new Announcement
        {
            User = user,
            Vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(addAnnouncementDto.VinNumber),
            Title = addAnnouncementDto.Title,
            Description = addAnnouncementDto.Description,
            County = addAnnouncementDto.County,
            City = addAnnouncementDto.City,
            ImagesUrl = addAnnouncementDto.ImagesUrl
        };

        var id = await _unitOfWork.Announcements.Insert(newAnnouncement) ?? Guid.Empty;
        await _unitOfWork.SaveChanges();

        return id;
    }

    public async Task<Announcement> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementDto updateDto)
    {
        var existingAnnouncement = await _unitOfWork.Announcements.GetById(announcementId);

        if (existingAnnouncement is null)
        {
            throw new ResourceMissingException($"Announcement with id '{announcementId}' doesn't exist");
        }

        if (existingAnnouncement.User?.Id != userId && existingAnnouncement.User?.RoleType != DataLayer.Enums.Role.Administrator)
        {
            throw new ForbiddenException($"User with id '{userId}' doesn't have acces to this operation!");
        }


        existingAnnouncement.Vehicle = updateDto.Vehicle;
        existingAnnouncement.Title = updateDto.Title;
        existingAnnouncement.Description = updateDto.Description;
        existingAnnouncement.County = updateDto.County;
        existingAnnouncement.City = updateDto.City;
        existingAnnouncement.ImagesUrl = updateDto.ImagesUrl;

        return await _unitOfWork.Announcements.Update(existingAnnouncement);
    }

    public async Task<bool> DeleteAnnouncement(Guid userId, Guid announcementId)
    {
        var existingAnnouncement = await _unitOfWork.Announcements.GetById(announcementId);

        if (existingAnnouncement is null)
        {
            throw new ResourceMissingException($"Announcement with id '{announcementId}' doesn't exist");
        }

        if (existingAnnouncement.User?.Id != userId && existingAnnouncement.User?.RoleType != DataLayer.Enums.Role.Administrator)
        {
            throw new ForbiddenException($"User with id '{userId}' doesn't have acces to this operation!");
        }

        var announcement = await _unitOfWork.Announcements.Remove(existingAnnouncement.Id);
        await _unitOfWork.SaveChanges();

        return announcement is not null;
    }

    public async Task<List<AnnouncementDto>> GetAllAnnouncements(List<Vehicle> vehicles)
    {
        var announcements = await _unitOfWork.Announcements.GetAll();
        announcements = announcements
            .Where(ann => vehicles.Contains(ann.Vehicle))
            .ToList();

        return announcements.ToAnnouncementDtos();
    }
}