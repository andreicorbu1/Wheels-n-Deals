namespace Wheels_n_Deals.API.Infrastructure.CustomExceptions;

public class ResourceMissingException : Exception
{
    public ResourceMissingException()
    {
    }

    public ResourceMissingException(string message) : base(message)
    {
    }
}