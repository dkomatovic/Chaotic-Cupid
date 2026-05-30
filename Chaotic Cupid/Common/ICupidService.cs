using System.ServiceModel;

namespace Common
{
    [ServiceContract(CallbackContract = typeof(ICupidCallback))]
    public interface ICupidService
    {
        [OperationContract]
        bool InitSinglePerson(string username, string city, int age, string phoneNumber);

        [OperationContract]
        void AcknowledgeLetter();

        [OperationContract]
        bool BlockUser(string usernameToBlock);
    }
}
