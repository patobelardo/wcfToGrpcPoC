using System;
using System.Runtime.InteropServices;
using ProtoBuf.Grpc.Client;
using System.Net.Http;

namespace Client.Library
{
    public class ServiceAgent<TContract>: IDisposable where TContract : class
    {
        private readonly HttpClient _client;
        private static HttpClientHandler clientHandler = new HttpClientHandler();
        public ServiceAgent()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ){
                HttpClientExtensions.AllowUnencryptedHttp2 = true;
            };

            var server = "localhost";
            if (Environment.GetEnvironmentVariable("serverlocation") != null)
                server = Environment.GetEnvironmentVariable("serverlocation");

            _client = new HttpClient (clientHandler);
            _client.BaseAddress = new Uri($"http://{server}:8000");
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