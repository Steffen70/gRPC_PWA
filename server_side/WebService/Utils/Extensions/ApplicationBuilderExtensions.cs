using System.Reflection;

namespace Seventy.WebService.Utils.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MapGrpcServices(this IApplicationBuilder app)
    {
        var serviceTypeNamespace = Assembly.GetExecutingAssembly().GetName().Name + ".Services";

        var serviceTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.Namespace?.StartsWith(serviceTypeNamespace) == true)
            .Where(x => !x.IsNested);

        foreach (var type in serviceTypes)
        {
            var method = typeof(GrpcEndpointRouteBuilderExtensions)
                .GetMethod(nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService))
                !.MakeGenericMethod(type);

            _ = method.Invoke(null, new[] { app });
        }

        return app;
    }
}