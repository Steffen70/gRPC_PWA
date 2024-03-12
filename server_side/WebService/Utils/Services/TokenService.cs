using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Seventy.Common.Model;
using Seventy.WebService.Middleware;
using Seventy.WebService.Model;
using Seventy.WebService.Utils.Extensions;

namespace Seventy.WebService.Utils.Services;

public class TokenService
{
    internal const int TokenExpirationDays = 7;

    private readonly RefGuidService _dataReference;

    public TokenService(RefGuidService dataReference) => _dataReference = dataReference;

    //internal static readonly SymmetricSecurityKey TokenKey = new(UTF8.GetBytes(Guid.NewGuid().ToString()));

    // TODO: Remove the hardcoded key and use a key that is generated once when the application starts
    internal static readonly SymmetricSecurityKey TokenKey = new("12345678901234567890123456789012"u8.ToArray());

    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(ClaimTypes.Role, user.UserRole.ToString()),

            new(SessionMiddleware<SessionData>.GuidIdentifier, _dataReference.Value.ToString()!),

            // Add salt to make the token unique
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var creds = new SigningCredentials(TokenKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            // TODO: Make the token expiration time configurable
            Expires = DateTime.Now.AddDays(TokenExpirationDays),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public AppUser CheckTokenChecksum(HttpContext context, LoginContext loginContext)
    {
        var appUser = context.User.GetAppUser(loginContext);

        if (appUser.SessionTokens.Count == 0)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            throw new Exception("Token is invalid!");
        }

        context.Request.Headers.TryGetValue("Authorization", out var bearerValue);

        // Check if the token is in the Authorization header
        // If not, check if the token is in the query string (used for SignalR hubs)
        var bearerString = bearerValue.FirstOrDefault() ?? $"Bearer {context.Request.Query["access_token"]}";

        var checksum = bearerString.GetChecksum();

        if (appUser.SessionTokens.Any(s => s.TokenChecksum == checksum)) return appUser;

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        throw new Exception("Token is invalid!");
    }
}