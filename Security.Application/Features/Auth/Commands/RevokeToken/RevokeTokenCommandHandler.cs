using AutoMapper;
using Security.Domain.Repositories;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Auth.Rules;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler(IAuthBusinessRules authBusinessRules, IRefreshTokenRepository refreshTokenRepository, IMapper mapper) : ICommandHandler<RevokeTokenCommand, RevokedTokenResponse>
{
    public async Task<RevokedTokenResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTkn = await refreshTokenRepository
            .GetAsync(new GetModel<MGH.Core.Infrastructure.Securities.Security.Entities.RefreshToken>
            { Predicate = r => r.Token == request.Token });

        await authBusinessRules.RefreshTokenShouldBeExists(refreshTkn);
        await authBusinessRules.RefreshTokenShouldBeActive(refreshTkn!);
        await refreshTokenRepository.UpdateAsync(refreshTkn, true, cancellationToken);

        return mapper.Map<RevokedTokenResponse>(refreshTkn);
    }
}