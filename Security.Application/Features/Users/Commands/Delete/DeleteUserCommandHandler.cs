using AutoMapper;
using Security.Domain.Repositories;
using MGH.Core.Application.Buses.Commands;
using Security.Application.Features.Users.Rules;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.Users.Commands.Delete;

public class DeleteUserCommandHandler(IMapper mapper, IUserRepository userRepository, IUserBusinessRules userBusinessRules)
    : ICommandHandler<DeleteUserCommand, DeletedUserResponse>
{
    public async Task<DeletedUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<User>>(request);
        var user = await userRepository.GetAsync(getUserModel);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);

        await userRepository.DeleteAsync(user,true,cancellationToken);
        return mapper.Map<DeletedUserResponse>(user);
    }
}