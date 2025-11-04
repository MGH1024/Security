using AutoMapper;
using Security.Domain.Repositories;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Users.Rules;
using Security.Application.Features.Users.Extensions;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Users.Commands.Create;

public class CreateUserCommandHandler(IMapper mapper, IUserBusinessRules userBusinessRules, IUserRepository userRepository)
    : ICommandHandler<CreateUserCommand, CreatedUserResponse>
{
    public async Task<CreatedUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await userBusinessRules.UserEmailShouldNotExists(request.Email, cancellationToken);
        var user = mapper.Map<User>(request);

        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
        user.SetHashPassword(hashingHelperModel);

        var createdUser = await userRepository.AddAsync(user,true, cancellationToken);
        return mapper.Map<CreatedUserResponse>(createdUser);
    }
}