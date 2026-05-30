using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Chaotic Cupid - Client ---\n");

            var callbackHandler = new CupidCallbackHandler();
            var context = new InstanceContext(callbackHandler);
            var client = new CupidClient(context, "NetTcpBinding_ICupidService");
            callbackHandler.SetClient(client);

            // Registration
            Console.WriteLine("- Regster -");
            while (true)
            {
                try
                {
                    Console.Write("Username: ");
                    string username = Console.ReadLine();
                    Console.Write("City: ");
                    string city = Console.ReadLine();
                    Console.Write("Age: ");
                    int age = int.Parse(Console.ReadLine());
                    Console.Write("Phone: ");
                    string phone = Console.ReadLine();

                    bool success = client.InitSinglePerson(username, city, age, phone);
                    if (success)
                    {
                        Console.WriteLine("Registration successfull! Awaiting letters...\n");
                        Console.WriteLine("Actions:\n/block username\n/exit\n Any key to acknowledge the letter\n");
                        break;
                    }

                    Console.WriteLine("Registration failed. Enter valid data and try again");
                }
                catch (EndpointNotFoundException)
                {
                    Console.WriteLine("Service not found. Shutting down...");
                    try { client.Close(); } catch { client.Abort(); }
                    Environment.Exit(1);
                }
                catch {
                    Console.WriteLine("Enter data in the right format");
                }

            }
            

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "/exit")
                    break;
                else if (input.StartsWith("/block"))
                {
                    var parts = input.Split(' ');
                    if (parts.Length == 2)
                    {
                        bool blocked = client.BlockUser(parts[1]);
                        Console.WriteLine(blocked ? $"Blocked {parts[1]}" : $"Failed blocking {parts[1]}");
                    }
                    else
                        Console.WriteLine("Syntax: /block username");
                }
                else
                    try
                    {
                        client.AcknowledgeLetter();
                    }
                    catch
                    {
                        Console.WriteLine("Failed acknowledging letter");
                    }
                    
            }

        }
    }
}
