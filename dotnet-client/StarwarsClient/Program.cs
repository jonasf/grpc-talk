using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StarwarsClient
{
    class Program
    {
        private const int Port = 50051;
        private const string ServiceUrl = "localhost";

        private const int NumberOfCharactersToFetch = 10;
        private const bool PrintResult = true;

        static void Main(string[] args)
        {
            var characters = CreateCharacterList(NumberOfCharactersToFetch);

            using (var client = new StarwarsClient(ServiceUrl, Port, PrintResult))
            {
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
            }

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
