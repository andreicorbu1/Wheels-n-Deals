using Wheels_n_Deals.API.DataLayer.Interfaces;

namespace Wheels_n_Deals.API.DataLayer;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository Users { get; }

    private readonly AppDbContext _context;

    public UnitOfWork(IUserRepository userRepository,
        AppDbContext context)
    {
        Users = userRepository;
        _context = context;
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
