using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace Client
{
    public class CupidCallbackHandler : ICupidCallback
    {
        private CupidClient _client; 

        public void SetClient(CupidClient client) => _client = client;
        public void ReceiveLetter(Letter letter)
        {
            Console.WriteLine("--- A letter arrived for you ---\n");
            Console.WriteLine($"From: {letter.FromUsername}");
            Console.WriteLine($"Age: {letter.FromAge}");
            Console.WriteLine($"City: {letter.FromCity}");
            if (letter.Message != "Not interested")
                Console.WriteLine($"Phone: {letter.FromPhone}");
            Console.WriteLine($"Message: {letter.Message}\n");
            Console.WriteLine("--------------------------------");
        }
    }
}
