﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MGH.Core.Infrastructure.Securities.Security.Encryption;

public static class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string securityKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
}
