using System;
using System.Runtime.InteropServices;
using System.Net.Http;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using Grpc.Core;
using Grpc.Net.Client;
namespace Client.Library
{
    public class ServiceAgent<TContract>: IDisposable where TContract : class
    {
        // private readonly HttpClient _client;
        private readonly GrpcChannel _client;
        private static HttpClientHandler clientHandler = new HttpClientHandler();
        public ServiceAgent()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ){
                GrpcClientFactory.AllowUnencryptedHttp2 = true;
            };
    

            var server = "localhost";
            if (Environment.GetEnvironmentVariable("serverlocation") != null)
                server = Environment.GetEnvironmentVariable("serverlocation");

            _client = GrpcChannel.ForAddress($"http://{server}:8000");

            // _client = new HttpClient (clientHandler);
            // _client.BaseAddress = new Uri($"http://{server}:8000");
        }

        public object Invoke(Func<TContract, object> func)
        {
            var instance = _client.CreateGrpcService<TContract>();
            
            return func(instance);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}