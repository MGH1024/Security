using AutoMapper;
using Security.Domain;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.OperationClaims.Rules;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.OperationClaims.Commands.Delete;

public class DeleteOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    IOperationClaimBusinessRules operationClaimBusinessRules) : ICommandHandler<DeleteOperationClaimCommand, DeletedOperationClaimResponse>
{
    public async Task<DeletedOperationClaimResponse> Handle(DeleteOperationClaimCommand request, CancellationToken cancellationToken)
    {
        var operationClaim = await uow.OperationClaim.GetAsync(
            new GetModel<OperationClaim>
            {
                Predicate = oc => oc.Id == request.Id,
                Include = q => q.Include(oc => oc.UserOperationClaims),
            });

        await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);
        await uow.OperationClaim.DeleteAsync(entity: operationClaim!);

        return mapper.Map<DeletedOperationClaimResponse>(operationClaim);
    }
}