using Security.Domain;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using Security.Application.Features.Auth.Constants;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Users.Rules;

public class UserBusinessRules(IUow uow) : BaseBusinessRules, IUserBusinessRules
{
    public Task UserShouldBeExistsWhenSelected(User user)
    {
        if (user is null)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
        return Task.CompletedTask;
    }

    public async Task UserIdShouldBeExistsWhenSelected(int id, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(id, cancellationToken);
        if (user is null)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
    }

    public Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDoesNotMatch);
        return Task.CompletedTask;
    }

    public async Task UserEmailShouldNotExists(string email, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetByEmailAsync(email, cancellationToken);
        if (user is not null)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
}