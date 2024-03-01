using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Seventy.Common.Model;

namespace Seventy.WebService.Utils;

public class RoleBasedAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public RoleBasedAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policyBuilder = new AuthorizationPolicyBuilder();

        policyBuilder.RequireAuthenticatedUser();

        var tuples = Enum.GetValues(typeof(AppUserRole))
            .Cast<AppUserRole>()
            .Select(role => (Role: role, Name: role.ToString()))
            .ToList();

        if (Enum.TryParse<AppUserRole>(policyName, out var requiredRole))
        {
            // Filter roles up to and including the required role
            var matching = tuples
                .Where(tuple => tuple.Role <= requiredRole)
                .ToList();

            if (matching.Any())
                tuples = matching;

            // If no roles match, fallback to all roles
        }

        var roles = tuples.Select(tuple => tuple.Name).ToList();

        policyBuilder.RequireAssertion(context => roles.Any(context.User.IsInRole));

        return await Task.FromResult(policyBuilder.Build());
    }
}