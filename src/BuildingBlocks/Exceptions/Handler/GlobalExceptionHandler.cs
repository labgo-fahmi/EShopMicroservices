using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Exceptions.Handler;

public sealed class GlobalExceptionHandler : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int statusCode, string message, string title, Dictionary<string, object?>? Extensions) = exception switch
        {
            CustomException => (StatusCodes.Status400BadRequest, exception.Message, exception.GetType().Name, null),
            ValidationException => HandleValidationException(exception),
            _ => (StatusCodes.Status500InternalServerError, exception.Message, exception.GetType().Name, null)
        };
        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = title,
            Detail = message,
            Instance = httpContext.Request.Path
        };
        if (Extensions != null)
        {
            problemDetails.Extensions = Extensions;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int statusCode, string message, string title, Dictionary<string, object?>? Extensions) HandleValidationException(Exception exception)
    {
        var errors = (exception as ValidationException)!.Errors.Select(e => new KeyValuePair<string, object>(e.PropertyName, e.ErrorMessage)).ToDictionary();
        var fieldErrors = new Dictionary<string, object?>() { { "fields", errors } };
        return (StatusCodes.Status400BadRequest, "One or more invalid fields", exception.GetType().Name, fieldErrors);
    }
}