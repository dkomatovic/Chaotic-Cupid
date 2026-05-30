

using System;
using System.ServiceModel;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(CupidService)))
            {
                host.Open();
                Console.WriteLine("--- Cupid service ---");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
