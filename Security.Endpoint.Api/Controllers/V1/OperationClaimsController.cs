using MediatR;
using Asp.Versioning;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.Requests;
using Security.Application.Features.OperationClaims.Commands.Create;
using Security.Application.Features.OperationClaims.Commands.Delete;
using Security.Application.Features.OperationClaims.Commands.Update;
using Security.Application.Features.OperationClaims.Queries.GetById;
using Security.Application.Features.OperationClaims.Queries.GetList;

namespace Security.Endpoint.Api.Controllers.V1;

/// <summary>
/// Provides API endpoints for managing operation claims,
/// including creation, retrieval, updates, and deletion.
/// </summary>
/// <remarks>
/// Operation claims define the permissions and actions available to users in the system.
/// </remarks>
[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class OperationClaimsController(ISender sender) : AppController(sender)
{
    /// <summary>
    /// Retrieves an operation claim by its unique identifier.
    /// </summary>
    /// <param name="getByIdOperationClaimQuery">The query containing the operation claim ID.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Returns details of the specified operation claim.</returns>
    /// <response code="200">Operation claim successfully retrieved.</response>
    /// <response code="404">Operation claim not found.</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdOperationClaimQuery getByIdOperationClaimQuery,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(getByIdOperationClaimQuery, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of all operation claims.
    /// </summary>
    /// <param name="pageRequest">Pagination parameters such as page number and page size.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Returns a paginated list of operation claims.</returns>
    /// <response code="200">List of operation claims successfully retrieved.</response>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest, CancellationToken cancellationToken)
    {
        var getListOperationClaimQuery = new GetListOperationClaimQuery { PageRequest = pageRequest };
        var result = await Sender.Send(getListOperationClaimQuery, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new operation claim.
    /// </summary>
    /// <param name="createOperationClaimCommand">The command containing details of the new operation claim.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Returns the created operation claim information.</returns>
    /// <response code="201">Operation claim successfully created.</response>
    /// <response code="400">Invalid operation claim data provided.</response>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommand createOperationClaimCommand,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(createOperationClaimCommand, cancellationToken);
        return Created(uri: "", response);
    }

    /// <summary>
    /// Updates an existing operation claim.
    /// </summary>
    /// <param name="updateOperationClaimCommand">The command containing updated operation claim details.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Returns the updated operation claim information.</returns>
    /// <response code="200">Operation claim successfully updated.</response>
    /// <response code="400">Invalid update data provided.</response>
    /// <response code="404">Operation claim not found.</response>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand updateOperationClaimCommand,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(updateOperationClaimCommand, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Deletes an operation claim.
    /// </summary>
    /// <param name="deleteOperationClaimCommand">The command specifying which operation claim to delete.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Returns confirmation of deletion.</returns>
    /// <response code="200">Operation claim successfully deleted.</response>
    /// <response code="404">Operation claim not found.</response>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteOperationClaimCommand deleteOperationClaimCommand,
        CancellationToken cancellationToken)
    {
        var respnse = await Sender.Send(deleteOperationClaimCommand, cancellationToken);
        return Ok(respnse);
    }
}
