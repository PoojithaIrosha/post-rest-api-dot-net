using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PostCrud.Middleware;

public class CustomUnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public CustomUnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalStatusCode = context.Response.StatusCode;

        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            var endpoint = context.GetEndpoint();
            var authorizeAttributes = endpoint?.Metadata.GetOrderedMetadata<AuthorizeAttribute>();

            if (authorizeAttributes != null && authorizeAttributes.Any(a => a.Roles.Contains("USER")))
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "You are not authorized to access this resource.",
                    Instance = context.Request.Path,
                    Status = (int)HttpStatusCode.Unauthorized
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}