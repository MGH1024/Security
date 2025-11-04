using System.Collections;
using Microsoft.EntityFrameworkCore;
using Security.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Metadata;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using Security.Domain.Repositories;

namespace Security.Infrastructure.Repositories.Security;

public class RefreshTokenRepository(SecurityDbContext securityDbContext)
    : Repository<RefreshToken, int>(securityDbContext), IRefreshTokenRepository
{
    private IQueryable<RefreshToken> Query() => securityDbContext.Set<RefreshToken>();

    public async Task<RefreshToken> GetByTokenAsync(string requestRefreshToken, CancellationToken cancellationToken)
    {
        return await Query().Where(a => a.Token == requestRefreshToken).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetRefreshTokenByUserId(int userId, int refreshTokenTtl,
        CancellationToken cancellationToken)
    {
        var queryable = Query();
        var refreshTokens = await queryable.Where(r =>
            r.UserId == userId
            && r.Revoked == null
            && r.Expires >= DateTime.UtcNow
            && r.CreatedAt.AddDays(refreshTokenTtl) <= DateTime.UtcNow).ToListAsync(cancellationToken);

        return refreshTokens;
    }

    public async Task DeleteRangeAsync(IEnumerable<RefreshToken> entities, bool permanent = false)
    {
        foreach (var refreshTkn in entities)
        {
            await DeleteAsync(refreshTkn);
        }
    }
}