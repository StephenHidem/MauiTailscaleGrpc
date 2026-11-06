using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AntPlusMauiClient.GrpcServices
{
    /// <summary>
    /// This interceptor adds a custom header to gRPC requests to identify the client by name. The client name
    /// is obtained from the device information and included in the request headers for server-side logging
    /// and tracking purposes.
    /// </summary>
    internal class ClientNameInterceptor : Interceptor
    {
        private readonly Metadata _headers;

        /// <summary>
        /// Creates a new instance of the <see cref="ClientNameInterceptor"/> class and initializes
        /// the custom header with the client name.
        /// </summary>
        public ClientNameInterceptor()
        {
            _headers = new Metadata
            {
                { "x-client-name", DeviceInfo.Name }
            };
        }

        /// <summary>
        /// Intercepts unary gRPC calls to add the custom client name header to the request.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(_headers));
            return continuation(request, newContext);
        }

        /// <summary>
        /// Intercepts server streaming gRPC calls to add the custom client name header to the request.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(_headers));
            return continuation(request, newContext);
        }
    }
}
