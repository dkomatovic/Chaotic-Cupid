using System.ServiceModel;

namespace Common
{
    
    internal interface ICupidCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveLetter(string fromUsername, string fromCity, int fromAge, string fromPhone, string message);
    }
}
