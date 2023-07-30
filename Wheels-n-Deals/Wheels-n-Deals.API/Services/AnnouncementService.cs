using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
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
        var user = await _unitOfWork.Users.GetUserAsync(addAnnouncementDto.UserId);
        var vehicle = await _unitOfWork.Vehicles.GetVehicleAsync(addAnnouncementDto.VinNumber);

        if (user is null || vehicle is null)
        {
            throw new Exception("User or vehicle not found!");
        }

        if (vehicle.Owner.Id != user.Id && user.Role != Role.Admin)
        {
            throw new Exception("You are not allowed to do this operation!");
        }

        if (addAnnouncementDto is null)
        {
            throw new ArgumentNullException(nameof(addAnnouncementDto));
        }

        var images = new List<Image>();
        if (addAnnouncementDto.ImagesUrl is not null)
        {
            images = await AddImagesToAnnouncement(addAnnouncementDto.ImagesUrl);
        }

        var newAnnouncement = new Announcement()
        {
            Owner = user,
            Vehicle = vehicle,
            VehicleId = vehicle.Id,
            Title = addAnnouncementDto.Title,
            Description = addAnnouncementDto.Description,
            County = addAnnouncementDto.County,
            City = addAnnouncementDto.City,
            Images = images
        };

        return await _unitOfWork.Announcements.InsertAsync(newAnnouncement);
    }

    private async Task<List<Image>> AddImagesToAnnouncement(IEnumerable<ImageDto> images)
    {
        if (images == null) { throw new ArgumentNullException(nameof(images)); }

        var imagesForAnnouncement = new List<Image>();

        foreach (var image in images)
        {
            var dbImage = await _unitOfWork.Images.GetImageAsync(image.ImageUrl);
            if (dbImage == null)
            {
                dbImage = new Image
                {
                    ImageUrl = image.ImageUrl,
                };
                dbImage.Id = await _unitOfWork.Images.InsertAsync(dbImage);
            }
            imagesForAnnouncement.Add(dbImage);
        }

        return imagesForAnnouncement;
    }

    public async Task<Announcement?> DeleteAnnouncementAsync(Guid id)
    {
        var result = await _unitOfWork.Announcements.RemoveAsync(id);
        if (result == null) return null;
        return result;
    }

    public async Task<Announcement?> GetAnnouncementAsync(Guid id)
    {
        return await _unitOfWork.Announcements.GetAnnouncementAsync(id);
    }

    public async Task<List<Announcement>> GetAnnouncementsAsync(List<Vehicle> vehicles)
    {
        var announcements = await _unitOfWork.Announcements.GetAnnouncementsAsync(vehicles);

        return announcements;
    }


    public async Task<Announcement?> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementDto updatedAnnouncement)
    {
        var existingAnnouncement = await GetAnnouncementAsync(id);

        if (existingAnnouncement != null)
        {
            existingAnnouncement.Title = updatedAnnouncement.Title;
            existingAnnouncement.City = updatedAnnouncement.City;
            existingAnnouncement.County = updatedAnnouncement.County;
            existingAnnouncement.Description = updatedAnnouncement.Description;
            var vehicle = await _unitOfWork.Vehicles.GetVehicleAsync(updatedAnnouncement.VinNumber) ?? throw new Exception("You need to add the vehicle with that vin to the database first");
            existingAnnouncement.Vehicle = vehicle;
            existingAnnouncement.VehicleId = vehicle.Id;
            if (updatedAnnouncement.ImagesUrl != null && updatedAnnouncement.ImagesUrl.Count > 0)
            {
                existingAnnouncement.Images = await AddImagesToAnnouncement(updatedAnnouncement.ImagesUrl);
            }
            foreach(var image in (await _unitOfWork.Images.GetImagesAsync()))
            {
                if(!existingAnnouncement.Images.Contains(image))
                {
                    var ann = image.Announcements.FirstOrDefault(an => an.Id == id);
                    if (ann != null)
                    {
                        image.Announcements.Remove(ann);
                    }
                }
            }
            existingAnnouncement = await _unitOfWork.Announcements.UpdateAsync(existingAnnouncement);
            return existingAnnouncement;
        }

        throw new Exception($"Announcement with id {id} does not exist!");
    }
}
