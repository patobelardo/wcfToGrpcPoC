using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        // Task AddAsync(int p1);
        Task AddAsync(intParam p1);
        Task<intParam> MultiplyAsync(intParam p1);
    }


    [DataContract]
    public class intParam
    {
        [DataMember(Order = 1)]
        public int p1 { get; set; }
    }
}