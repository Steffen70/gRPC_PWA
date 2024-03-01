using Microsoft.AspNetCore.Authorization;
using Seventy.Common.Model;

namespace Seventy.WebService.Model;

public class RequireRoleAttribute : AuthorizeAttribute
{
    public RequireRoleAttribute(AppUserRole role) : base(role.ToString()) { }
}
