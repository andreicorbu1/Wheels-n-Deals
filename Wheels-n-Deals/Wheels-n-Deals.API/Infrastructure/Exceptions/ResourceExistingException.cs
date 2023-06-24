namespace Wheels_n_Deals.API.Infrastructure.Exceptions;

public class ResourceExistingException : Exception
{
    public ResourceExistingException(string message) : base(message) 
    {
    }
}