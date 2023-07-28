namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    //IVehicleRepository Vehicles { get; }
    //IFeaturesRepository Features { get; }
    //IAnnouncementRepository Announcements { get; }
    //IImageRepository Images { get; }

    Task SaveChangesAsync();
}
