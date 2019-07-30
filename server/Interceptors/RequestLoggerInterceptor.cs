using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace server
{
    public class RequestInterceptor : Interceptor
    {
        private const string MessageTemplate = "{RequestMethod} responded {StatusCode} in {Elapsed:0.0000} ms";
        private static int counter = 0;

        
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response= null;
            try
            {
                var sw = Stopwatch.StartNew();
                //Request
                response = await base.UnaryServerHandler(request, context, continuation);

                //Response
                counter++;
                sw.Stop();

                Console.WriteLine($"Request: {context.Method}. Params: {request}. Responded {context.Status.StatusCode} in {sw.Elapsed.TotalMilliseconds:0.0000} ms. Counter: {counter}");
            }
            catch (System.Exception ex) 
            {
                var correlationID = Guid.NewGuid();

                Console.WriteLine("Original Exception");
                Console.WriteLine($"{ex}.CorrelationID: {correlationID}");

                

                context.ResponseTrailers.Add(new Metadata.Entry("Source", ex.Source));
                context.ResponseTrailers.Add(new Metadata.Entry("Parameters", request.ToString()));
                await context.WriteResponseHeadersAsync(new Metadata());
                throw new Exception($"Generic Error. CorrelationID: {correlationID}");
            }

            return response;
        }
    }
}


