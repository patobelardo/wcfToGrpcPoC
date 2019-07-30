using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf;

namespace Contracts
{
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        Task<DoResponse> DoAsync(DoRequest request);
    }


    [DataContract]
    public class DoRequest
    {
        [DataMember(Order = 1)]
        public List<Shape> Shapes { get; set; }
    }

    [DataContract]
    public class DoResponse
    {
        [DataMember(Order = 1)]
        public bool IsSuccess { get; set; }
    }

    //Protoinclude is part of protobuf-net implementation and will allow to include derived classes
    [DataContract, ProtoInclude(4, typeof(Circle))]
    public abstract class Shape
    {
        public int S1 { get; set; }
        public int S2 { get; set; }
    }
    [DataContract]
    public class Circle : Shape
    {
        public int C1 { get; set; }
        public int C2 { get; set; }
    }
}