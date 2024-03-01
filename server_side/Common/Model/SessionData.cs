namespace Seventy.Common.Model;

// SessionData can implement IDisposable if it needs to clean up resources.
// The Service automatically disposes of the SessionData when the session ends.
public class SessionData
{
    public string? Data { get; set; }
}
