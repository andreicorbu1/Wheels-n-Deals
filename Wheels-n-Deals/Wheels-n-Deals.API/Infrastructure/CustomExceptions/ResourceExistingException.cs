namespace Wheels_n_Deals.API.Infrastructure.CustomExceptions;

public class ResourceExistingException : Exception
{
    public ResourceExistingException(string message) : base(message)
    {
    }
}
