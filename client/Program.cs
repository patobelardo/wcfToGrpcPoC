using System;
using System.Threading.Tasks;
using Grpc.Core;
using Contracts;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Client.Library;
using Microsoft.Extensions.DependencyInjection;
using Shared_CS;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using System.Net.Http;
using System.Diagnostics;

namespace client
{
    class Program
    {
        static bool isTest = false;
        static Stopwatch watch;
        static void Main(string[] args)
        {
            watch = new Stopwatch();
            int iterations = 1;
            
            if (args.Length != 0)
            {
                isTest = true;
                iterations = int.Parse(args[0]);
            }

            #region Without ServiceAgent
            // watch.Start();
            // if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ){
            //     HttpClientExtensions.AllowUnencryptedHttp2 = true;
            // };

            // var server = "localhost";
            // if (Environment.GetEnvironmentVariable("serverlocation") != null)
            //     server = Environment.GetEnvironmentVariable("serverlocation");

            // using var http = new HttpClient { BaseAddress = new Uri($"http://{server}:8000") };
            // watch.Stop();
            // Console.WriteLine($"Httpclient creation in {watch.ElapsedMilliseconds}ms");

            // watch.Reset();

            // _writeConsole("Calling MyCalculator...");
            // var calculator = http.CreateGrpcService<ICalculator>();
            // var result1 = calculator.MultiplyAsync(new MultiplyRequest { X = 12, Y = 4 });
            // _writeConsole($"Result: {result1.Result}");

            #endregion

            watch.Start();
            for (int pos =0; pos < iterations; pos++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                _writeConsole("Calling MyCalculator...");
                
                var c = new ServiceAgent<ICalculator>();
                var res = (ValueTask<MultiplyResult>)c.Invoke(x=> x.MultiplyAsync(new MultiplyRequest { X = 12, Y = 4 }));

                _writeConsole($"Result: {res.Result}");


                Console.ForegroundColor = ConsoleColor.Blue;
                _writeConsole("Calling Service2...");

                var service2 = new ServiceAgent<Contracts.IService2>();
                var result2 = service2.Invoke( x=> x.AddAsync(new intParam{ p1= 3 }) );
                
                _writeConsole($"Result: {result2}");


                Console.ForegroundColor = ConsoleColor.Blue;
                _writeConsole("Calling Service2-Multiply...");

                var result2multiply = (Task<intParam>)service2.Invoke( x=> x.MultiplyAsync(new intParam{ p1= 3 } ));
                
                _writeConsole($"Result: {result2multiply.Result.p1}");

                Console.ForegroundColor = ConsoleColor.Blue;


                _writeConsole("Calling SampleService-Do...");
                
                var sampleService = new ServiceAgent<ISampleService>();

                var doRequest = new DoRequest
                    {
                        Shapes = new List<Shape>
                        {
                            new Circle {S1 = 1, S2 = 2, C1 = 3, C2 = 4},
                            new Circle {S1 = 11, S2 = 12, C1 = 13, C2 = 14}
                        }
                    };

                var resultSampleService = (Task<DoResponse>)sampleService.Invoke( x=> x.DoAsync(doRequest));
                
                _writeConsole($"Result: {resultSampleService.Result.IsSuccess}");

                _writeStats(pos, iterations);
            }
        }

        private static void _writeStats(int pos, int iterations)
        {
            long sec = watch.ElapsedMilliseconds / 1000;
            if (sec != 0)
                Console.WriteLine($"Requests/sec: {pos / sec }");       
        }

        private static void _writeConsole(string message)
        {
            if (!isTest)
            {
                Console.WriteLine(message);
                Console.ReadLine();      
            }
        }
    }
}
