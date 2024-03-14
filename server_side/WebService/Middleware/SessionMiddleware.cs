using System.Text.RegularExpressions;
using Seventy.Common.Model;
using Seventy.WebService.Model;
using Seventy.WebService.Utils.Extensions;
using Seventy.WebService.Utils.Services;

namespace Seventy.WebService.Middleware;

public class SessionMiddleware<TSessionData> where TSessionData : SessionData, new()
{
    // The key used to store the Guid in the HttpContext.Items dictionary
    public static readonly string GuidIdentifier = nameof(SessionMiddleware<TSessionData>) + ".Guid";

    private readonly RequestDelegate _next;

    public SessionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, SessionService<TSessionData> sessionService, TokenService tokenService, LoginContext loginContext, RefGuidService dataReference)
    {
        var (path, method) = (context.Request.Path, context.Request.Method);

        if (!context.IsAuthorizedEndpoint())
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($@"Request {method} {path} anonymous endpoint");
            Console.ResetColor();

            await _next(context);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($@"Request {method} {path} authorized endpoint");
        Console.ResetColor();

        // Retrieve the data reference from the JWT token
        dataReference.Value = context.User.GetDataReference();

        try
        {
            var appUser = tokenService.CheckTokenChecksum(context, loginContext);

            // Attempt to capture data, this sets the InUse property to true
            // The capture fails if the Data object was desposed by a SignalR disconnect event
            if (!await sessionService.CaptureDataAsync())
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                throw new Exception("The session was closed, renew the token to initialize a new session");
            }

            var dataWrapper = sessionService.RetrieveDataWrapper();

            // Initialize the Data if it is not initialized yet (first request)
            if (!dataWrapper.IsInitialized)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($@"Request {method} {path} initializing session data");
                Console.ResetColor();

                // Only initialize the Data if the request is from a SignalR hub
                // This prevents the Data being initialized but not disposed e.g. when the SignalR connection is never established and therefor never closed/disposed
                if (context.Request.Path.StartsWithSegments("/hubs"))
                    dataWrapper.Init(appUser);
                else
                    throw new Exception(
                        "The Session is not fully initialized, start the session by connecting to a SignalR hub");
            }

            await _next(context);
        }
        finally
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($@"Request {method} {path} finally block reached");
            Console.ResetColor();

            if (dataReference.Value.HasValue)
                // Hub connection data reference was already released in OnConnectedAsync event
                if (!Regex.IsMatch(context.Request.Path, @"^\/hubs\/[^\/]+(?<!\/negotiate)$"))
                {
                    sessionService.ReleaseData();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($@"Session data released: {dataReference.Value}");
                    Console.ResetColor();
                }
        }
    }
}
