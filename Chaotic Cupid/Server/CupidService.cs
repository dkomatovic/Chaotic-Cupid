using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                 ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CupidService : ICupidService
    {
        private readonly ConcurrentDictionary<string, Person> _persons = new ConcurrentDictionary<string, Person>();
        private readonly Timer _timer;
        private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        public CupidService()
        {
            // Timer se pokreće odmah, pa svakih 60 sekundi
            _timer = new Timer(SendLetters, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }
        
        private Person findPersonByCallback(ICupidCallback callback)
        {
            return _persons.Values.FirstOrDefault(p => p.Callback == callback);
        }
        public bool InitSinglePerson(string username, string city, int age, string phoneNumber)
        {
            var callback = OperationContext.Current.GetCallbackChannel<ICupidCallback>();
            var person = new Person
            {
                Username = username,
                City = city,
                Age = age,
                PhoneNumber = phoneNumber,
                Callback = callback,
                HasPendingLetter = false
            };
            return _persons.TryAdd(username, person);
        }

        public void AcknowledgeLetter()
        {
            var callback = OperationContext.Current.GetCallbackChannel<ICupidCallback>();
            var person = findPersonByCallback(callback);
            if (person != null)
                person.HasPendingLetter = false;

        }

        public bool BlockUser(string usernameToBlock)
        {
            var callback = OperationContext.Current.GetCallbackChannel<ICupidCallback>();
            var person = findPersonByCallback(callback);
            if (person == null) return false;
            return person.BlockedUsers.Add(usernameToBlock);
        }

        private void SendLetters(object state)
        {
            var personsSnapshot = _persons.Values.ToList();
            
            foreach (Person person in personsSnapshot)
            {
                if (!person.HasPendingLetter)
                {
                    try
                    {
                        Person bestMatch = FindBestMatch(person);
                        Letter letter = new Letter();

                        if (bestMatch == null)
                        {
                            letter.FromUsername = "Cupid server";
                            letter.FromCity = "Novi Sad";
                            letter.FromAge = 1;
                            letter.FromPhone = "";
                            letter.Message = "There seem to be no matches for you right now...";
                            Console.WriteLine($"Sent letter to {person.Username}, no matches available");

                        }
                        else
                        {
                            letter.FromUsername = bestMatch.Username;
                            letter.FromCity = bestMatch.City;
                            letter.FromAge = bestMatch.Age;
                            letter.Message = ChooseMessage();
                            letter.FromPhone = letter.Message == "Not interested" ? "" : bestMatch.PhoneNumber;
                            Console.WriteLine($"Sent letter to {person.Username}, from {bestMatch.Username}");
                        }

                        person.Callback.ReceiveLetter(letter);
                        person.HasPendingLetter = true;

                    }
                    catch (Exception)
                    {
                        // client process is probably terminated, remove
                        _persons.TryRemove(person.Username, out _);
                    }
                }
                
            }
        }

        private Person FindBestMatch(Person subject)
        {
            var personsSnapshot = _persons.Values.ToList();

            int bestScore = 0;
            Person bestMatch = null;
            foreach (Person match in personsSnapshot)
            {
                if (match.Username == subject.Username || 
                    subject.BlockedUsers.Contains(match.Username))
                    continue;

                int score = 0;
                if (match.City == subject.City)
                    score += 30;
                if (Math.Abs(match.Age - subject.Age) <= 2)
                    score += 20;

                byte[] randomBytes = new byte[1];
                _rng.GetBytes(randomBytes);
                score += randomBytes[0] % 100;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = match;
                }
            }

            return bestMatch;
        }


        private string ChooseMessage()
        {
            List<string> possibleMessages = new List<string>()
            {
                "Not interested", "Looking forward to meeting you!", "I want to meet you."
            };

            byte[] randomBytes = new byte[1];
            _rng.GetBytes(randomBytes);
            int index = randomBytes[0] % 3;

            return possibleMessages[index];
        }
    }
}
