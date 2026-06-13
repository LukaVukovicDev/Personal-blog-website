using System.Net;
using System.Text.Json;
using Blog.Application.Common.Exceptions;
using FluentValidation;

namespace Blog.Api.Middleware;

/// <summary>
/// Hvata sve neuhvaćene izuzetke i pretvara ih u konzistentan <c>application/problem+json</c> odgovor.
/// Domenski izuzeci se mapiraju na odgovarajuće HTTP statuse.
/// </summary>
public class GlobalExceptionHandler
{
    private readonly RequestDelegate next;
    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleAsync(context, exception);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            await WriteValidationProblemAsync(context, validationException);
            return;
        }

        var (status, title) = exception switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Niste autorizovani."),
            ForbiddenAccessException => (HttpStatusCode.Forbidden, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Došlo je do neočekivane greške."),
        };

        if (status == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "Neuhvaćena greška pri obradi {Path}", context.Request.Path);
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)status;

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)status}",
            title,
            status = (int)status,
            traceId = context.TraceIdentifier,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    private async Task WriteValidationProblemAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)HttpStatusCode.BadRequest}",
            title = "Uneti podaci nisu validni.",
            status = (int)HttpStatusCode.BadRequest,
            traceId = context.TraceIdentifier,
            errors,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
