using MediatR;
using AutoMapper;
using Asp.Versioning;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Security.Application.Features.Auth.Commands.Login;
using Security.Application.Features.Auth.Commands.Register;
using Security.Application.Features.Auth.Commands.RevokeToken;
using Security.Application.Features.Auth.Commands.RefreshToken;

namespace Security.Endpoint.Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class AuthController(ISender sender, IMapper mapper) : AppController(sender)
{
    /// <summary>
    /// Authenticates a user using the provided credentials and issues access and refresh tokens.
    /// </summary>
    /// <param name="loginCommandDto">
    /// The login credentials provided by the client, typically including username and password.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the login operation.
    /// </param>
    /// <returns>
    /// Returns <see cref="OkObjectResult"/> (HTTP 200) with a <see cref="LoginCommandResponse"/> object on success.  
    /// Returns <see cref="BadRequestObjectResult"/> (HTTP 400) if authentication fails.  
    /// Returns <see cref="StatusCodeResult"/> (HTTP 500) for unexpected server errors.
    /// </returns>
    /// <response code="200">Successfully authenticated; returns the access and refresh tokens.</response>
    /// <response code="400">Invalid credentials or failed login attempt.</response>
    /// <response code="500">An internal server error occurred during the login process.</response>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginCommandResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommandDto loginCommandDto, CancellationToken cancellationToken)
    {
        var command = mapper.Map<LoginCommand>(loginCommandDto);

        var response = await Sender.Send(command, cancellationToken);
        if (!response.IsSuccess)
            return BadRequest("Failed to login");

        SetCookie("refreshToken", response.RefreshToken, response.RefreshTokenExpiry);
        SetCookie("token", response.Token, response.TokenExpiry);
        return Ok(response);
    }

    /// <summary>
    /// Registers a new user account and issues an authentication refresh token.
    /// </summary>
    /// <remarks>
    /// This endpoint accepts a <see cref="RegisterCommandDto"/> containing the user's registration details,
    /// maps it to a <see cref="RegisterCommand"/>, and dispatches it using the MediatR pipeline.
    /// <para>
    /// Upon successful registration, a secure refresh token is stored in an HTTP-only cookie,
    /// and the response body contains a <see cref="RegisterCommandResponse"/> with user and token details.
    /// </para>
    /// </remarks>
    /// <param name="registerCommandDto">
    /// The data transfer object containing the registration details for the new user.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to observe while waiting for the task to complete, allowing the operation to be canceled.
    /// </param>
    /// <returns>
    /// Returns a <see cref="CreatedResult"/> (<c>201 Created</c>) containing the registration response if successful.
    /// If validation fails, returns <see cref="BadRequestResult"/> (<c>400 Bad Request</c>).
    /// If an internal server error occurs, returns <see cref="StatusCodeResult"/> (<c>500 Internal Server Error</c>).
    /// </returns>
    /// <response code="201">User was successfully registered.</response>
    /// <response code="400">Invalid registration data was provided.</response>
    /// <response code="500">An internal server error occurred while processing the registration.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterCommandResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommandDto registerCommandDto,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterCommand>(registerCommandDto);
        var response = await Sender.Send(command, cancellationToken);
        SetCookie("refreshToken", response.RefreshToken, response.RefreshTokenExpiry);
        SetCookie("token", response.Token, response.TokenExpiry);
        return Created(uri: "", response);
    }


    /// <summary>
    /// Retrieves the current refresh token from the request cookies, sends a command to refresh it,
    /// and updates the cookie with the new token and its expiry time.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>
    /// Returns a <see cref="CreatedResult"/> (HTTP 201) containing a <see cref="RefreshTokenResponse"/> object
    /// with the new refresh token and its expiry date. Returns 400 if the request is invalid,
    /// or 500 if an internal server error occurs.
    /// </returns>
    /// <response code="201">Successfully refreshed the token and set it in the cookie.</response>
    /// <response code="400">The request was invalid or the refresh token was missing.</response>
    /// <response code="500">An unexpected error occurred on the server.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RefreshTokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var command = mapper.Map<RefreshTokenCommand>(GetRefreshTokenFromCookies());
        var response = await Sender.Send(command, cancellationToken);
        SetCookie("refreshToken", response.RefreshToken, response.RefreshTokenExpiry);
        return Created(uri: "", response);
    }

    /// <summary>
    /// Revokes the specified refresh token, effectively invalidating the user's session.
    /// </summary>
    /// <param name="refreshToken">
    /// The refresh token to revoke.  
    /// If not provided in the request body, the token will be retrieved from the request cookies.
    /// </param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>
    /// Returns an <see cref="OkObjectResult"/> (HTTP 200) indicating the revocation result.
    /// </returns>
    /// <response code="200">The token was successfully revoked.</response>
    /// <response code="400">The provided refresh token is invalid or missing.</response>
    /// <response code="500">An unexpected error occurred while revoking the token.</response>
    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string refreshToken,
        CancellationToken cancellationToken)
    {
        var token = refreshToken ?? GetRefreshTokenFromCookies();
        var revokeTokenCommand = mapper.Map<RevokeTokenCommand>(token);
        var result = await Sender.Send(revokeTokenCommand, cancellationToken);
        return Ok(result);
    }

}