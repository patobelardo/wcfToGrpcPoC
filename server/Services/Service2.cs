using Microsoft.Extensions.Logging;
using Shared_CS;
using System.Threading.Tasks;
using System;
using Contracts;

namespace server
{
    public class Service2 : Contracts.IService2
    {
        public Service2()
        {
        }

        public Task AddAsync(intParam p1)
        {
            Console.WriteLine("We are in Service2-AddAsync");
            return Task.CompletedTask;
        }
        public Task<intParam> MultiplyAsync(intParam p)
        {
             Console.WriteLine("We are in Service2-MultiplyAsync");
            //throw new Exception("This method is not implemented yet!");
            return Task.FromResult(new intParam{ p1= p.p1 * p.p1});
        }
    }
}
