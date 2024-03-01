
namespace Seventy.Common.Model;

public class SessionToken
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }

    public string? TokenChecksum { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Todo: Change hashing algorithm to SHA256 and add salt
}