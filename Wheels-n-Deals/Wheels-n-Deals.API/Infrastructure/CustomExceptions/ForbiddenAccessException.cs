namespace Wheels_n_Deals.API.Infrastructure.CustomExceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
