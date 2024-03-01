using Microsoft.AspNetCore.Authorization;
using Seventy.Common.Model;

namespace Seventy.WebService.Utils.Extensions;

public static class AuthorizationOptionsExtensions
{
    public static void AddAppUserRoles(this AuthorizationOptions options)
    {
        // loop through all AppUserRoles and add a policy for each role
        foreach (var role in Enum.GetValues<AppUserRole>())
            options.AddPolicy(role.ToString(), policy => policy.RequireRole(role.ToString()));
    }
}
