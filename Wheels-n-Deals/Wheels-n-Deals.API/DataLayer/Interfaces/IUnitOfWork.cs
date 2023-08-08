﻿namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IVehicleRepository Vehicles { get; }
    IFeatureRepository Features { get; }
    IAnnouncementRepository Announcements { get; }
    IImageRepository Images { get; }

    Task SaveChangesAsync();
}