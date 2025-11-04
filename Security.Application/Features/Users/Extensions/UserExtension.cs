using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Security.Application.Features.Users.Commands.Update;
using Security.Application.Features.Users.Queries.GetList;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.Users.Extensions;

public static class UserExtension
{
    public static void SetHashPassword(this User user, HashingHelperModel hashingHelperModel)
    {
        user.PasswordHash = hashingHelperModel.PasswordHash;
        user.PasswordSalt = hashingHelperModel.PasswordSalt;
    }

    public static GetListModelAsync<User> ToGetListAsyncModel(this GetListUserQuery getListUserQuery,
        CancellationToken cancellationToken)
    {
        return new GetListModelAsync<User>
        {
            Index = getListUserQuery.PageRequest.PageIndex,
            Size = getListUserQuery.PageRequest.PageSize,
        };
    }

    public static UpdatedUserResponse ToUpdateUserResponse(this User user)
    {
        return new UpdatedUserResponse(
            Id: user.Id,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Status: false)
        ;
    }
}