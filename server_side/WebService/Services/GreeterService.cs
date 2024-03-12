using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Seventy.Common;

namespace Seventy.WebService.Services;

[Authorize]
public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloResponse> HelloWorld(HelloRequest request, ServerCallContext context)
    {
        var response = new HelloResponse { Message = "Hello " + request.Name };
        return Task.FromResult(response);
    }
}