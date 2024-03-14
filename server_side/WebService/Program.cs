using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Seventy.Common.Model;
using Seventy.WebService.Hubs;
using Seventy.WebService.Middleware;
using Seventy.WebService.Model;
using Seventy.WebService.Utils;
using Seventy.WebService.Utils.Extensions;
using Seventy.WebService.Utils.Services;

namespace Seventy.WebService;

public class Program
{
    public static void Main(string[] args) => StartApp<SessionData>(args);

    internal static void InitSession<TSessionData>(AppUser user, TSessionData data) where TSessionData : SessionData, new()
    {
        data.Data = $"Hello {user.Username}!";
    }

    private static void StartApp<TData>(string[] args) where TData : SessionData, new()
    {
        var builder = WebApplication.CreateBuilder(args);

        // TODO: Configure with a real certificate for production
        // TODO: Store the certificate password in a secure location
        var certSettings = builder.Configuration.GetSection("Certificate").Get<CertificateSettings>()!;
        builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(5001, listenOptions =>
        {
            listenOptions.UseHttps(new X509Certificate2(certSettings.Path, certSettings.Password));
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        }));

        builder.Services.AddDbContext<LoginContext>(options
            => options.UseSqlite(builder.Configuration.GetConnectionString("LoginContext")));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<InMemorySessionPool<TData>>();

        builder.Services.AddScoped<RefGuidService>();

        builder.Services.AddScoped<SessionService<TData>>();

        builder.Services.AddScoped(ServiceProviderExtensions.GetSessionData<TData>);

        builder.Services.AddTransient<TokenService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.ConfigureJwtBearer(TokenService.TokenKey));

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, RoleBasedAuthorizationPolicyProvider>();

        builder.Services.AddAuthorization(AuthorizationOptionsExtensions.AddAppUserRoles);

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        builder.Services.AddGrpc();

        // TODO: Add the client origins to the configuration file to setup CORS
        var clients = builder.Configuration.GetSection("Clients").Get<string[]>()!;
        builder.Services.AddCors(o => o.AddPolicy("ClientPolicy", policyBuilder =>
        {
            policyBuilder.WithOrigins(clients)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
        }));

        builder.Services.AddControllers();

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.Services.SeedDataBase();

        var sessionPool = app.Services.GetRequiredService<InMemorySessionPool<TData>>();

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            // Terminate all sessions
            sessionPool.TerminateSessions();
        };

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();

        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

        app.UseCors("ClientPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMiddleware<SessionMiddleware<TData>>();

        app.MapHub<SessionPersistenceHub<TData>>("/hubs/session-hub");

        app.MapGrpcServices();

        app.MapControllers();

        app.Run();
    }
}
