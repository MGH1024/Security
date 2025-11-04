using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Users.Extensions;

public static class UserExtension
{
    public static void SetHashPassword(this User user, HashingHelperModel hashingHelperModel)
    {
        user.PasswordHash = hashingHelperModel.PasswordHash;
        user.PasswordSalt = hashingHelperModel.PasswordSalt;
    }
}