using Security.Domain;
using MGH.Core.Application.Buses.Commands;
using Security.Application.Features.Auth.Rules;
using Security.Application.Features.Auth.Services;

namespace Security.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IUow uow,
    IAuthService authService,
    IAuthBusinessRules authBusinessRules) : ICommandHandler<LoginCommand, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetByEmailAsync(request.LoginCommandDto.Email, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.LoginCommandDto.Password,
            cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(user, cancellationToken);
        var createdRefreshTkn = await authService.CreateRefreshToken(user);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(createdRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(user.Id, cancellationToken);

        return new LoginCommandResponse(createdAccessToken.Token, createdAccessToken.Expiration, addedRefreshTkn.Token,
            addedRefreshTkn.Expires, true);
    }
}