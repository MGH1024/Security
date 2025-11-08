using Security.Domain.Repositories;
using Security.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

namespace Security.Infrastructure.Repositories.Security;

public class OperationClaimRepository(SecurityDbContext securityDbContext) :
    Repository<OperationClaim, int>(securityDbContext),
    IOperationClaimRepository
{
    public IQueryable<OperationClaim> Query() => securityDbContext.Set<OperationClaim>();
}