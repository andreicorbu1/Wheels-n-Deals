namespace Wheels_n_Deals.API.Infrastructure.CustomExceptions;

public class RenewTimeNotElapsedException : Exception
{
    public RenewTimeNotElapsedException(string message) : base(message)
    {
    }
}