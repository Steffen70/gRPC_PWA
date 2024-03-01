using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Seventy.Common;

public class AuthInterceptor : Interceptor
{
    private readonly string _token;

    public AuthInterceptor(string token) => _token = token;

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var newContext = CreateNewContext(context);

        return base.AsyncUnaryCall(request, newContext, continuation);
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var newContext = CreateNewContext(context);

        return base.AsyncServerStreamingCall(request, newContext, continuation);
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var newContext = CreateNewContext(context);

        return base.AsyncClientStreamingCall(newContext, continuation);
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var newContext = CreateNewContext(context);

        return base.AsyncDuplexStreamingCall(newContext, continuation);
    }

    protected ClientInterceptorContext<TRequest, TResponse> CreateNewContext<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context) where TRequest : class where TResponse : class
    {
        var metadata = new Metadata { new("Authorization", $"Bearer {_token}") };

        var newOptions = context.Options.WithHeaders(metadata);

        return new ClientInterceptorContext<TRequest, TResponse>(
                       context.Method,
                       context.Host,
                       newOptions);
    }
}
