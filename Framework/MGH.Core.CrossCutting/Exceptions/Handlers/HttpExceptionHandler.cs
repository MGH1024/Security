﻿using MGH.Core.CrossCutting.Exceptions.Extensions;
using MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;
using MGH.Core.CrossCutting.Exceptions.Types;
using Microsoft.AspNetCore.Http;

namespace MGH.Core.CrossCutting.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }

    private HttpResponse _response;

    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var details = new BusinessProblemDetails(businessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(BadRequestException badRequestException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var details = new BusinessProblemDetails(badRequestException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(ValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var details = new ValidationProblemDetails(validationException.Errors).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(AuthorizationException authorizationException)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        var details = new AuthorizationProblemDetails(authorizationException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(NotFoundException notFoundException)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        string details = new NotFoundProblemDetails(notFoundException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        string details = new InternalServerErrorProblemDetails(exception.Message).AsJson();
        return Response.WriteAsync(details);
    }
}
