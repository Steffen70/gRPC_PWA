using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Seventy.Common;
using Seventy.Common.Model;

namespace Seventy.WebService.Services;

[Authorize]
public class GreeterService : Greeter.GreeterBase
{
    private readonly SessionData _sessionData;

    public GreeterService(SessionData sessionData) => _sessionData = sessionData;

    public override Task<HelloResponse> HelloWorld(Empty request, ServerCallContext context)
    {
        var response = new HelloResponse { Message = _sessionData.Data };

        return Task.FromResult(response);
    }
}