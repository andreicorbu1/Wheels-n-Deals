namespace Wheels_n_Deals.API.Infrastructure.Exceptions;

public class ResourceMissingException : Exception
{
    public ResourceMissingException(string message) : base(message) 
    {
    }
}