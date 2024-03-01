using System.Security.Cryptography;
using System.Text;

namespace Seventy.Common.Model;

public class AppUser
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;

    public AppUserRole UserRole { get; set; } = AppUserRole.Member;

    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;

    public List<SessionToken> SessionTokens { get; set; } = new();

    public static AppUser Create(string username, string password, AppUserRole userRole = AppUserRole.Member)
    {
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            Username = username.ToLower(),
            UserRole = userRole,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))),
            PasswordSalt = Convert.ToBase64String(hmac.Key),
        };

        return user;
    }
}