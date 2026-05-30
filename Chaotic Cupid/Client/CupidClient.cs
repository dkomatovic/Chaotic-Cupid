using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    public class CupidClient : DuplexClientBase<ICupidService>, ICupidService
    {
        public CupidClient(InstanceContext context, string endpointName)
        : base(context, endpointName) { }

        public bool InitSinglePerson(string username, string city, int age, string phoneNumber)
            => Channel.InitSinglePerson(username, city, age, phoneNumber);

        public void AcknowledgeLetter() => Channel.AcknowledgeLetter();

        public bool BlockUser(string usernameToBlock) => Channel.BlockUser(usernameToBlock);

    }
}
