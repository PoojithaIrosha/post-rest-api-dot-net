using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PostCrud.Exception;

namespace PostCrud.Filters;

public class ProblemDetailsExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Detail = context.Exception.Message,
            Instance = context.HttpContext.Request.Path
        };
        
        if (context.Exception is PostNotFoundException || context.Exception is UserNotFoundException)
        {
            problemDetails.Status = (int)HttpStatusCode.NotFound;
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            problemDetails.Status = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            problemDetails.Status = (int) HttpStatusCode.InternalServerError;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = context.HttpContext.Response.StatusCode
        };

        context.ExceptionHandled = true;
    }
}