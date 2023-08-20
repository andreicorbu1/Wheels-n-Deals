using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnnouncementService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> AddAnnouncementAsync(AddAnnouncementDto addAnnouncementDto)
    {
        if (addAnnouncementDto is null) throw new ArgumentNullException(nameof(addAnnouncementDto));

        var user = await _unitOfWork.Users.GetByIdAsync(addAnnouncementDto.UserId);
        var vehicle = await _unitOfWork.Vehicles.GetVehicleAsync(addAnnouncementDto.VinNumber);

        if (user is null || vehicle is null || vehicle.Owner is null)
            throw new ResourceMissingException("User or vehicle not found!");

        if (vehicle.Owner.Id != user.Id && user.Role != Role.Admin)
            throw new ForbiddenAccessException("You are not allowed to do this operation!");

        var images = new List<Image>();
        if (addAnnouncementDto.ImagesUrl is not null)
            images = await AddImagesToAnnouncement(addAnnouncementDto.ImagesUrl);

        var newAnnouncement = new Announcement
        {
            Owner = user,
            Vehicle = vehicle,
            VehicleId = vehicle.Id,
            Title = addAnnouncementDto.Title,
            Description = addAnnouncementDto.Description,
            County = addAnnouncementDto.County,
            City = addAnnouncementDto.City,
            Images = new List<Image>(),
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
        newAnnouncement.Images = images;
        newAnnouncement = await _unitOfWork.Announcements.AddAsync(newAnnouncement);
        await _unitOfWork.SaveChangesAsync();

        if (newAnnouncement is null) return Guid.Empty;

        return newAnnouncement.Id;
    }

    public async Task<Announcement?> DeleteAnnouncementAsync(Guid id)
    {
        var result = await _unitOfWork.Announcements.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return result is null
            ? throw new ResourceMissingException($"Announcement with id {id} does not exist")
            : result;
    }

    public async Task<Announcement?> GetAnnouncementAsync(Guid id)
    {
        return await _unitOfWork.Announcements.GetByIdAsync(id);
    }

    public async Task<List<Announcement>> GetAnnouncementsAsync(List<Vehicle> vehicles)
    {
        var announcements = await _unitOfWork.Announcements.GetManyAsync(a => (a.Vehicle != null) && vehicles.Contains(a.Vehicle));

        return announcements;
    }

    public async Task<Announcement?> RenewAnnouncementAsync(Guid id)
    {
        var announcement = await GetAnnouncementAsync(id) ??
                           throw new ResourceMissingException($"Announcement with id {id} does not exist");
        if (DateTime.UtcNow.Day - announcement.DateModified.Day < 1)
            throw new RenewTimeNotElapsedException("An announcement can be renewed once every 24h");
        announcement.DateModified = DateTime.UtcNow;
        await _unitOfWork.Announcements.UpdateAsync(announcement);
        await _unitOfWork.SaveChangesAsync();
        return announcement;
    }

    public async Task<Announcement?> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementDto updatedAnnouncement)
    {
        var existingAnnouncement = await GetAnnouncementAsync(id) ??
                                   throw new ResourceMissingException($"Announcement with id {id} does not exist!");
        await ClearExistingImagesAsync(existingAnnouncement);
        if (updatedAnnouncement is null || updatedAnnouncement.ImagesUrl is null)
            throw new ArgumentNullException(nameof(updatedAnnouncement));
        var newImages = await AddImagesToAnnouncement(updatedAnnouncement.ImagesUrl);

        UpdateAnnouncementProperties(existingAnnouncement, updatedAnnouncement, newImages);

        return await SaveUpdatedAnnouncementAsync(existingAnnouncement);
    }

    private async Task<List<Image>> AddImagesToAnnouncement(IEnumerable<ImageDto> images)
    {
        if (images is null) throw new ArgumentNullException(nameof(images));

        var imagesForAnnouncement = new List<Image>();

        foreach (var image in images)
        {
            var dbImage = await _unitOfWork.Images.GetImageAsync(image.ImageUrl);
            if (dbImage is null)
            {
                dbImage = new Image
                {
                    ImageUrl = image.ImageUrl
                };
                await _unitOfWork.Images.AddAsync(dbImage);
            }

            imagesForAnnouncement.Add(dbImage);
        }

        return imagesForAnnouncement;
    }

    private async Task ClearExistingImagesAsync(Announcement announcement)
    {
        foreach (var image in announcement.Images.ToList())
        {
            announcement.Images.Remove(image);
            image.Announcements.Remove(announcement);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private void UpdateAnnouncementProperties(Announcement announcement, UpdateAnnouncementDto updatedAnnouncement,
        List<Image> newImages)
    {
        announcement.Title = updatedAnnouncement.Title;
        announcement.City = updatedAnnouncement.City;
        announcement.County = updatedAnnouncement.County;
        announcement.Description = updatedAnnouncement.Description;
        announcement.Images = newImages;
        var vehicle = GetVehicle(updatedAnnouncement.VinNumber);
        announcement.Vehicle = vehicle;
        announcement.VehicleId = vehicle.Id;
    }

    private Vehicle GetVehicle(string vinNumber)
    {
        var vehicle = _unitOfWork.Vehicles.GetVehicleAsync(vinNumber).Result;
        return vehicle is null
            ? throw new ResourceMissingException("You need to add the vehicle with that VIN to the database first")
            : vehicle;
    }

    private async Task<Announcement?> SaveUpdatedAnnouncementAsync(Announcement announcement)
    {
        return await _unitOfWork.Announcements.UpdateAsync(announcement);
    }
}