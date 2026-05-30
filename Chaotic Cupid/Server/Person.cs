using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Person
    {
        public string Username { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public ICupidCallback Callback { get; set; }
        public bool HasPendingLetter { get; set; }
        public HashSet<string> BlockedUsers { get; set; } = new HashSet<string>();
    }
}
