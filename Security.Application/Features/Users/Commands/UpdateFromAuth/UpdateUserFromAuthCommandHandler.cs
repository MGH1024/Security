using AutoMapper;
using Security.Domain.Repositories;
using MGH.Core.Application.Buses.Commands;
using Security.Application.Features.Users.Rules;
using Security.Application.Features.Auth.Services;
using Security.Application.Features.Users.Extensions;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Security.Application.Features.Users.Commands.UpdateFromAuth;

public class UpdateUserFromAuthCommandHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IUserBusinessRules userBusinessRules,
    IAuthService authService)
    : ICommandHandler<UpdateUserFromAuthCommand, UpdatedUserFromAuthResponse>
{
    public async Task<UpdatedUserFromAuthResponse> Handle(UpdateUserFromAuthCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.Id,cancellationToken);

        await userBusinessRules.UserShouldBeExistsWhenSelected(user);
        await userBusinessRules.UserPasswordShouldBeMatched(user, request.OldPassword);
        await userBusinessRules.UserEmailShouldNotExists(user.Email,cancellationToken);

        user = mapper.Map(request, user);
        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
        user.SetHashPassword(hashingHelperModel);

        var updatedUser = await userRepository.UpdateAsync(user,true, cancellationToken);

        var response = mapper.Map<UpdatedUserFromAuthResponse>(updatedUser);
        response.AccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);
        return response;
    }
}