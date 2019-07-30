using Microsoft.Extensions.Logging;
using Shared_CS;
using System.Threading.Tasks;

namespace server
{
    public class Calculator : ICalculator
    {
        ValueTask<MultiplyResult> ICalculator.MultiplyAsync(MultiplyRequest request)
        {
            var result = new MultiplyResult { Result = request.X * request.Y };
            return new ValueTask<MultiplyResult>(result);
        }
    }
}
