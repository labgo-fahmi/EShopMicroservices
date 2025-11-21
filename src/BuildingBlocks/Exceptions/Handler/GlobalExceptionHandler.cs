using BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
        };
        (int statusCode, string message, string title) = exception switch
        {
            CustomException => (StatusCodes.Status400BadRequest, exception.Message, exception.GetType().Name),
            ValidationException => (StatusCodes.Status400BadRequest, exception.Message, exception.GetType().Name),
            _ => (StatusCodes.Status500InternalServerError, exception.Message, exception.GetType().Name)
        };
        problemDetails.Status = statusCode;
        problemDetails.Title = title;
        problemDetails.Detail = message;

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}