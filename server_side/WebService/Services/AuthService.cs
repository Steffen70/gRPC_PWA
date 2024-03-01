using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Seventy.Common.Model;
using Seventy.WebService.Model;
using Seventy.WebService.Utils.Extensions;
using Seventy.WebService.Utils.Services;
using SwissPension.SP7.Common;

namespace Seventy.WebService.Services;

public class AuthService : Auth.AuthBase
{
    private readonly LoginContext _loginContext;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly SessionService<SessionData> _sessionService;

    public AuthService(LoginContext loginContext, IMapper mapper, TokenService tokenService, SessionService<SessionData> sessionService)
    {
        _loginContext = loginContext;
        _mapper = mapper;
        _tokenService = tokenService;
        _sessionService = sessionService;
    }

    [AllowAnonymous]
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Username and password must be provided"));

        var user = await _loginContext.Users
            .SingleOrDefaultAsync(x => x.Username == request.Username.ToLower());

        if (user is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Login failed"));

        using (var hmac = new HMACSHA512(Convert.FromBase64String(user.PasswordSalt)))
        {
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));

            if (!user.PasswordHash.Equals(computedHash))
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Login failed"));
        }

        _sessionService.CreateData();

        var token = _tokenService.CreateToken(user);

        var loginResponseDto = new LoginResponse { Token = $"Bearer {token}" };

        _loginContext.Attach(user);

        // Load the session tokens of the user
        await _loginContext.Entry(user).Collection(u => u.SessionTokens).LoadAsync();

        // Clean up old tokens
        var oldTokens = user.SessionTokens.Where(t => t.CreatedAt < DateTime.Now.AddDays(-TokenService.TokenExpirationDays)).ToList();
        _loginContext.RemoveRange(oldTokens);

        user.SessionTokens.Add(new SessionToken { TokenChecksum = loginResponseDto.Token.GetChecksum() });

        await _loginContext.SaveChangesAsync();

        return _mapper.Map(user, loginResponseDto);
    }

    [AllowAnonymous]
    public override async Task<LoginResponse> RenewToken(Empty request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();

        var appUser = _tokenService.CheckTokenChecksum(httpContext, _loginContext);

        // CreateData generates a new Guid, sets refRefGuid.Value,
        // adds the Data object to the InMemorySessionPool
        _sessionService.CreateData();

        await _sessionService.CaptureDataAsync();

        // Create a new token with the new Guid
        var newToken = _tokenService.CreateToken(appUser);

        newToken = $"Bearer {newToken}";

        // Update the token checksum in the database to invalidate all other tokens
        _loginContext.Attach(appUser);

        appUser.SessionTokens.Add(new SessionToken { TokenChecksum = newToken.GetChecksum() });
        await _loginContext.SaveChangesAsync();

        var loginResponseDto = new LoginResponse { Token = newToken };

        return _mapper.Map(appUser, loginResponseDto);
    }

    [RequireRole(AppUserRole.Admin)]
    public override Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var (password, statusCode) = CreateUser(_loginContext, request.Username);

        if (password is null)
            throw new RpcException(new Status(statusCode, string.Empty));

        var response = new CreateResponse { Password = password };

        return Task.FromResult(response);
    }

    internal static (string? password, StatusCode statusCode) CreateUser(LoginContext loginContext, string username, AppUserRole appUserRole = AppUserRole.Member)
    {
        // Regex to match a username with 3-20 characters, including a-z, A-Z, 0-9, -, _, and .
        var regex = new Regex(@"^[a-zA-Z0-9\-\._]{3,20}$");
        if (!regex.Match(username).Success)
            return (null, StatusCode.InvalidArgument);

        if (loginContext.Users.Any(u => u.Username == username))
            return (null, StatusCode.AlreadyExists);

        using var client = new HttpClient();

        //Use first part of the namespace as User-Agent
        client.DefaultRequestHeaders.Add("User-Agent", nameof(AuthService).Split('.').First());
        var response = client.GetStringAsync($"https://www.dinopass.com/password/simple").ConfigureAwait(true);

        var password = response.GetAwaiter().GetResult();

        if (string.IsNullOrWhiteSpace(password))
            return (null, StatusCode.Unavailable);

        using var hmac = new HMACSHA512();
        loginContext.Users.Add(new AppUser
        {
            Username = username.ToLower(),
            UserRole = appUserRole,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))),
            PasswordSalt = Convert.ToBase64String(hmac.Key)
        });

        loginContext.SaveChanges();
        return (password, StatusCode.OK);
    }
}