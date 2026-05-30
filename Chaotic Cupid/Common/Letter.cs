using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Letter
    {
        [DataMember] public string FromUsername { get; set; }
        [DataMember] public string FromCity { get; set; }
        [DataMember] public int FromAge { get; set; }
        [DataMember] public string FromPhone { get; set; }
        [DataMember] public string Message { get; set; }
    }
}
