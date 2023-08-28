using Wheels_n_Deals.API.DataLayer.Interfaces;

namespace Wheels_n_Deals.API.DataLayer;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly AppDbContext _context;

    public UnitOfWork(IUserRepository userRepository,
        AppDbContext context,
        IVehicleRepository vehicles,
        IFeatureRepository features,
        IAnnouncementRepository announcements,
        IImageRepository images)
    {
        Users = userRepository;
        _context = context;
        Vehicles = vehicles;
        Features = features;
        Announcements = announcements;
        Images = images;
    }

    public IUserRepository Users { get; }

    public IVehicleRepository Vehicles { get; }

    public IFeatureRepository Features { get; }
    public IAnnouncementRepository Announcements { get; }
    public IImageRepository Images { get; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);

        // Take this object off the finalization queue to prevent 
        // finalization code for this object from executing a second time.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Cleans up any resources being used.
    /// </summary>
    /// <param name="disposing">Whether or not we are disposing</param> 
    /// <returns><see cref="ValueTask"/></returns>
    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                await _context.DisposeAsync();
            }

            _disposed = true;
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
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