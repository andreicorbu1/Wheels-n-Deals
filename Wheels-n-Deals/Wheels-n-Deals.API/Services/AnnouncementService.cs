using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Enums;
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

        if (user is null)
        {
            throw new ResourceMissingException($"User with id '{addAnnouncementDto.UserId}' doesn't exist");
        }

        if (vehicle is null)
        {
            throw new ResourceMissingException($"Vehicle with id '{addAnnouncementDto.VinNumber}' doesn't exist");
        }

        if (vehicle?.Owner?.Id != user.Id && user.RoleType != DataLayer.Enums.Role.Administrator)
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
            ImagesUrl = null
        };

        var id = await _unitOfWork.Announcements.Insert(newAnnouncement) ?? Guid.Empty;
        await _unitOfWork.SaveChanges();

        if (id == Guid.Empty)
        {
            throw new Exception("Announcement could not be inserted into database");
        }

        var images = await AddImages(addAnnouncementDto.ImagesUrl, id);

        if (images != null)
        {
            var updateAnnouncementDto = new UpdateAnnouncementDto()
            {
                Vehicle = vehicle,
                Title = addAnnouncementDto.Title,
                Description = addAnnouncementDto.Description,
                County = addAnnouncementDto.County,
                City = addAnnouncementDto.City,
                ImagesUrl = images
            };
            await UpdateAnnouncement(user.Id, id, updateAnnouncementDto);
        }

        return id;
    }

    private async Task<List<Image>> AddImages(List<ImageDto> imagesDto, Guid announcementId)
    {
        List<Image> images = new();
        foreach (var image in imagesDto)
        {
            var auxImage = new Image()
            {
                Url = image.Url,
                Id = Guid.Empty,
                AnnouncementId = announcementId
            };
            var id = await _unitOfWork.Images.Insert(auxImage);
            auxImage.Id = id.Value;
            images.Add(auxImage);
        }
        await _unitOfWork.SaveChanges();
        return images;
    }

    public async Task<Announcement> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementDto updateDto)
    {
        var existingAnnouncement = await _unitOfWork.Announcements.GetById(announcementId);
        var user = await _unitOfWork.Users.GetById(userId);

        if (existingAnnouncement is null)
        {
            throw new ResourceMissingException($"Announcement with id '{announcementId}' doesn't exist");
        }

        if (existingAnnouncement.User?.Id != userId && user?.RoleType != DataLayer.Enums.Role.Administrator)
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
        var user = await _unitOfWork.Users.GetById(userId);

        if (existingAnnouncement is null)
        {
            throw new ResourceMissingException($"Announcement with id '{announcementId}' doesn't exist");
        }

        if (existingAnnouncement.User?.Id != userId && user?.RoleType != Role.Administrator)
        {
            throw new ForbiddenException($"User with id '{userId}' doesn't have acces to this operation!");
        }

        var images = await _unitOfWork.Images.GetImagesOfAnnouncement(announcementId);

        if(images is not null)
        {
            foreach (var image in images)
            {
                await _unitOfWork.Images.Remove(image.Id);
            }
        }

        var announcement = await _unitOfWork.Announcements.Remove(announcementId);
        await _unitOfWork.SaveChanges();

        return announcement is not null;
    }

    public async Task<List<AnnouncementDto>> GetAllAnnouncements(List<Vehicle> vehicles)
    {
        var announcements = await _unitOfWork.Announcements.GetAllAnnouncements(vehicles);

        return announcements.ToAnnouncementDtos();
    }
}