using MediatR;
using Asp.Versioning;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.Requests;
using Security.Application.Features.UserOperationClaims.Commands.Create;
using Security.Application.Features.UserOperationClaims.Commands.Delete;
using Security.Application.Features.UserOperationClaims.Commands.Update;
using Security.Application.Features.UserOperationClaims.Queries.GetById;
using Security.Application.Features.UserOperationClaims.Queries.GetList;

namespace Security.Endpoint.Api.Controllers.V1;

/// <summary>
/// Provides endpoints for managing user operation claims, 
/// including creation, retrieval, updating, and deletion of user permissions.
/// </summary>
/// <remarks>
/// User operation claims define which permissions or access rights 
/// are assigned to specific users in the system.
/// </remarks>
[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class UserOperationClaimsController(ISender sender) : AppController(sender)
{
    /// <summary>
    /// Retrieves a user operation claim by its unique identifier.
    /// </summary>
    /// <param name="getByIdUserOperationClaimQuery">The query containing the ID of the user operation claim.</param>
    /// <returns>Returns the details of the specified user operation claim.</returns>
    /// <response code="200">User operation claim successfully retrieved.</response>
    /// <response code="404">User operation claim not found.</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdUserOperationClaimQuery getByIdUserOperationClaimQuery)
    {
        var response = await Sender.Send(getByIdUserOperationClaimQuery);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of user operation claims.
    /// </summary>
    /// <param name="pageRequest">Pagination parameters such as page number and page size.</param>
    /// <returns>Returns a paginated list of user operation claims.</returns>
    /// <response code="200">List of user operation claims successfully retrieved.</response>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        var getListUserOperationClaimQuery = new GetListUserOperationClaimQuery { PageRequest = pageRequest };
        var response = await Sender.Send(getListUserOperationClaimQuery);
        return Ok(response);
    }

    /// <summary>
    /// Assigns a new operation claim to a user.
    /// </summary>
    /// <param name="createUserOperationClaimCommand">The command containing user ID and claim details.</param>
    /// <returns>Returns the newly created user operation claim.</returns>
    /// <response code="201">User operation claim successfully created.</response>
    /// <response code="400">Invalid user or claim data provided.</response>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserOperationClaimCommand createUserOperationClaimCommand)
    {
        var response = await Sender.Send(createUserOperationClaimCommand);
        return Created(uri: "", response);
    }

    /// <summary>
    /// Updates an existing user operation claim.
    /// </summary>
    /// <param name="updateUserOperationClaimCommand">The command containing updated user and claim information.</param>
    /// <returns>Returns the updated user operation claim details.</returns>
    /// <response code="200">User operation claim successfully updated.</response>
    /// <response code="400">Invalid update data provided.</response>
    /// <response code="404">User operation claim not found.</response>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserOperationClaimCommand updateUserOperationClaimCommand)
    {
        var response = await Sender.Send(updateUserOperationClaimCommand);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a user operation claim.
    /// </summary>
    /// <param name="deleteUserOperationClaimCommand">The command specifying which user operation claim to delete.</param>
    /// <returns>Returns confirmation of the deletion.</returns>
    /// <response code="200">User operation claim successfully deleted.</response>
    /// <response code="404">User operation claim not found.</response>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserOperationClaimCommand deleteUserOperationClaimCommand)
    {
        var response = await Sender.Send(deleteUserOperationClaimCommand);
        return Ok(response);
    }
}
