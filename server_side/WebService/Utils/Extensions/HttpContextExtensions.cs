using Microsoft.AspNetCore.Authorization;

namespace Seventy.WebService.Utils.Extensions;

public static class HttpContextExtensions
{
    public static bool IsAuthorizedEndpoint(this HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        return endpoint is not null &&
               // Check if the endpoint has AuthorizeAttribute or a subclass of it
               endpoint.Metadata.Any(metadataItem => metadataItem is AuthorizeAttribute) &&
               // Check if none AllowAnonymousAttribute or a subclass of it is present
               endpoint.Metadata.All(metadataItem => metadataItem is not AllowAnonymousAttribute);
    }
}
