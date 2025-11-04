using AutoMapper;
using Security.Domain.Repositories;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Users.Rules;
using Security.Application.Features.Users.Extensions;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Security.Application.Features.Users.Commands.Update;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IUserBusinessRules userBusinessRules)
    : ICommandHandler<UpdateUserCommand, UpdatedUserResponse>
{
    public async Task<UpdatedUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.Id,cancellationToken);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);
        await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user.Id, user.Email,cancellationToken);

        user = mapper.Map(request, user);
        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
        user.SetHashPassword(hashingHelperModel);

        await userRepository.UpdateAsync(user,true, cancellationToken);
        return user.ToUpdateUserResponse();
    }
}