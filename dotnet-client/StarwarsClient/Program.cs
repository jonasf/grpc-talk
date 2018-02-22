using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using Starwars;

namespace StarwarsClient
{
    class Program
    {
        private const int Port = 50051;
        private const int NumberOfCharactersToFetch = 100000;
        private const bool PrintResult = false;

        static void Main(string[] args)
        {
            var channel = new Channel("localhost", Port, ChannelCredentials.Insecure);

            var client = new StarwarsClient(new StarwarsService.StarwarsServiceClient(channel), PrintResult);

           var characters = CreateCharacterList(NumberOfCharactersToFetch);

            Console.WriteLine("Fetching {0} characters one at the time", characters.Count);
            MeasureTimeTaken(() =>
            {
                foreach (var character in characters)
                {
                    client.GetCharacters(character).Wait();
                }
            }, "Async");

            Console.WriteLine("Fetching {0} characters over a stream", characters.Count);
            MeasureTimeTaken(() =>
            {
                client.GetCharactersStream(characters).Wait();
            }, "Streaming"); 

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static IList<string> CreateCharacterList(int size)
        {
            var characterList = new List<string>();

            Random rnd = new Random();
            var characters = new[] {"Luke Skywalker", "Darth Vader", "C-3PO", "Obi-Wan Kenobi" };

            for (int i = 0; i < size; i++)
            {
                characterList.Add(characters[rnd.Next(0, 3)]);
            }

            return characterList;
        }

        static void MeasureTimeTaken(Action action, string description)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            action();

            stopWatch.Stop();
            Console.WriteLine("{0} operation time elapsed: {1}", description, stopWatch.Elapsed);
        }
    }
}
