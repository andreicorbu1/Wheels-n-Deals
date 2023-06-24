using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Wheels_n_Deals.API.Infrastructure.Exceptions;

namespace Wheels_n_Deals.API.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    readonly RequestDelegate next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("request1");

        try
        {
            await next(context);
        }
        catch (ForbiddenException ex)
        {
            await RespondToExceptionAsync(context, HttpStatusCode.Forbidden, ex.Message, ex);
        }
        catch (ResourceMissingException ex)
        {
            await RespondToExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex);
        }
        catch (ResourceExistingException ex)
        {
            await RespondToExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex);
        }
        catch (Exception ex)
        {
            await RespondToExceptionAsync(context, HttpStatusCode.InternalServerError, "Internal Server Error", ex);
        }

        Console.WriteLine("response1");
    }

    private static Task RespondToExceptionAsync(HttpContext context, HttpStatusCode failureStatusCode, string message, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)failureStatusCode;

        var response = new
        {
            Message = message,
            Error = exception.GetType().Name,
            Timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
    }
}