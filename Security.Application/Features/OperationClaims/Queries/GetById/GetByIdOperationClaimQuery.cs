using MediatR;
using AutoMapper;
using Security.Domain;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Application.Pipelines.Authorization;
using Security.Application.Features.OperationClaims.Rules;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.OperationClaims.Constants;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.OperationClaims.Queries.GetById;

[Roles(OperationClaimOperationClaims.GetOperationClaims)]
public class GetByIdOperationClaimQuery : IRequest<GetByIdOperationClaimResponse>
{
    public int Id { get; set; }
}

public class GetByIdOperationClaimQueryHandler(
    IUow uow,
    IMapper mapper,
    IOperationClaimBusinessRules operationClaimBusinessRules)
    : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimResponse>
{
    public async Task<GetByIdOperationClaimResponse> Handle(GetByIdOperationClaimQuery request,
        CancellationToken cancellationToken)
    {
        var operationClaim = await uow.OperationClaim.GetAsync(
            new GetModel<OperationClaim>
            {
                Predicate = b => b.Id == request.Id,
                Include = q => q.Include(oc => oc.UserOperationClaims),
            });
        await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);
        var response = mapper.Map<GetByIdOperationClaimResponse>(operationClaim);
        return response;
    }
}