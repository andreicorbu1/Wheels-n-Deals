namespace Wheels_n_Deals.API.Infrastructure.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}