using Wheels_n_Deals.API.DataLayer.Repositories;

namespace Wheels_n_Deals.API.DataLayer;

public class UnitOfWork
{
    public UserRepository Users { get; }
    public VehicleRepository Vehicles { get; }
    public FeaturesRepository Features { get; }
    public AnnouncementRepository Announcements { get; }
    public ImageRepository Images { get; }

    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext, UserRepository users, VehicleRepository vehicles, FeaturesRepository features, AnnouncementRepository announcements, ImageRepository images)
    {
        Users = users;
        Vehicles = vehicles;
        Features = features;
        Announcements = announcements;
        _dbContext = dbContext;
        Images = images;
    }

    public async Task SaveChanges()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            var errorMessage = "Error when saving to the database: "
                               + $"{exception.Message}\n\n"
                               + $"{exception.InnerException}\n\n"
                               + $"{exception.StackTrace}\n\n";

            Console.WriteLine(errorMessage);
        }
    }
}