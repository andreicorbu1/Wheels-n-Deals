using Wheels_n_Deals.API.DataLayer.Interfaces;

namespace Wheels_n_Deals.API.DataLayer;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository Users { get; }

    public IVehicleRepository Vehicles { get; }

    public IFeatureRepository Features { get; }
    public IAnnouncementRepository Announcements { get; }
    public IImageRepository Images { get; }

    public AppDbContext Context { get; }

    public UnitOfWork(IUserRepository userRepository,
        AppDbContext context,
        IVehicleRepository vehicles,
        IFeatureRepository features,
        IAnnouncementRepository announcements,
        IImageRepository images)
    {
        Users = userRepository;
        Context = context;
        Vehicles = vehicles;
        Features = features;
        Announcements = announcements;
        Images = images;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var errorMessage = "Error when saving to the database: "
                               + $"{ex.Message}\n\n"
                               + $"{ex.InnerException}\n\n"
                               + $"{ex.StackTrace}\n\n";

            Console.WriteLine(errorMessage);
        }
    }
}
