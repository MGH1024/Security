using AutoMapper;
using Security.Domain;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Auth.Rules;
using Security.Application.Features.Auth.Services;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUow uow,
    IAuthService authService,
    IAuthBusinessRules authBusinessRules,
    IMapper mapper)
    : ICommandHandler<RegisterCommand, RegisterCommandResponse>
{
    public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await authBusinessRules.UserEmailShouldBeNotExists(request.RegisterCommandDto.Email, cancellationToken);

        var newUser = mapper.Map<User>(request);
        authService.SetHashPassword(request.RegisterCommandDto.Password, newUser);

        var createdUser = await uow.User.AddAsync(newUser,false,cancellationToken);
        var createdRefreshToken = await authService.CreateRefreshToken(createdUser);
        newUser.RefreshTokens.Add(createdRefreshToken);

        await uow.CompleteAsync(cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(createdUser, cancellationToken);
        return new RegisterCommandResponse(createdAccessToken.Token, createdAccessToken.Expiration,
            createdRefreshToken.Token, createdRefreshToken.Expires);
    }
}