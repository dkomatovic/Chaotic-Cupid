using System.ServiceModel;

namespace Common
{
    public interface ICupidCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveLetter(Letter letter);
    }
}
