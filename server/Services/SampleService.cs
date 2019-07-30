using Microsoft.Extensions.Logging;
using Shared_CS;
using System.Threading.Tasks;
using System;
using Contracts;

namespace server
{
    public class SampleService : Contracts.ISampleService
    {
        public Task<DoResponse> DoAsync(DoRequest request)
        {
            Console.WriteLine("We are in SampleService-DoAsync");
            return Task.FromResult(new DoResponse{ IsSuccess = true } );
        }
    }
}
