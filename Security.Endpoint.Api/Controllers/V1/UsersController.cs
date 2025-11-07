using MediatR;
using AutoMapper;
using Asp.Versioning;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.Requests;
using Security.Application.Features.Users.Commands.Create;
using Security.Application.Features.Users.Commands.Delete;
using Security.Application.Features.Users.Commands.Update;
using Security.Application.Features.Users.Queries.GetById;
using Security.Application.Features.Users.Queries.GetList;
using Security.Application.Features.Users.Commands.UpdateFromAuth;

namespace Security.Endpoint.Api.Controllers.V1;

/// <summary>
/// Provides API endpoints for managing user accounts, including creation, updates, deletion,
/// and retrieving user information.
/// </summary>
/// <remarks>
/// All endpoints require appropriate authentication and authorization.
/// </remarks>
[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class UsersController(ISender sender, IMapper mapper) : AppController(sender)
{
    /// <summary>
    /// Retrieves a specific user by ID.
    /// </summary>
    /// <param name="getUserByIdQuery">The query containing the user ID to fetch.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns user details if found.</returns>
    /// <response code="200">User details successfully retrieved.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetUserByIdQuery getUserByIdQuery,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(getUserByIdQuery, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the currently authenticated user's information.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns details of the authenticated user.</returns>
    /// <response code="200">User details successfully retrieved.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpGet("GetFromAuth")]
    public async Task<IActionResult> GetFromAuth(CancellationToken cancellationToken)
    {
        var getByIdUserQuery = mapper.Map<GetUserByIdQuery>(GetUserIdFromRequest());
        var result = await Sender.Send(getByIdUserQuery, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a paginated list of all users.
    /// </summary>
    /// <param name="pageRequest">Pagination parameters such as page number and page size.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns a paginated list of users.</returns>
    /// <response code="200">List of users successfully retrieved.</response>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest, CancellationToken cancellationToken)
    {
        var getListUserQuery = mapper.Map<GetListUserQuery>(pageRequest);
        var result = await Sender.Send(getListUserQuery, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="createUserCommand">The command containing user creation details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns the created user information.</returns>
    /// <response code="201">User successfully created.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(createUserCommand, cancellationToken);
        return Created(uri: "", result);
    }

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="updateUserCommand">The command containing updated user details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns the updated user information.</returns>
    /// <response code="200">User successfully updated.</response>
    /// <response code="400">Invalid update data.</response>
    /// <response code="404">User not found.</response>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(updateUserCommand, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates the currently authenticated user's information.
    /// </summary>
    /// <param name="updateUserFromAuthCommand">The command containing updated details of the authenticated user.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns the updated user information.</returns>
    /// <response code="200">User successfully updated.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpPut("FromAuth")]
    public async Task<IActionResult> UpdateFromAuth([FromBody] UpdateUserFromAuthCommand updateUserFromAuthCommand,
        CancellationToken cancellationToken)
    {
        updateUserFromAuthCommand.Id = GetUserIdFromRequest();
        var result = await Sender.Send(updateUserFromAuthCommand, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a user account.
    /// </summary>
    /// <param name="deleteUserCommand">The command specifying which user to delete.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns confirmation of deletion.</returns>
    /// <response code="200">User successfully deleted.</response>
    /// <response code="404">User not found.</response>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand deleteUserCommand, CancellationToken cancellationToken)
    {
        DeletedUserResponse result = await Sender.Send(deleteUserCommand, cancellationToken);
        return Ok(result);
    }
}
