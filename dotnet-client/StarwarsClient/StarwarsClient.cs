using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Starwars;

namespace StarwarsClient
{
    public class StarwarsClient
    {
        private readonly StarwarsService.StarwarsServiceClient _starwarsServiceClient;
        private readonly bool _printResultToConsoleOut;
        private const int TimeOut = 120;

        public StarwarsClient(StarwarsService.StarwarsServiceClient starwarsServiceClient, bool printResultToConsoleOut)
        {
            _starwarsServiceClient = starwarsServiceClient;
            _printResultToConsoleOut = printResultToConsoleOut;
        }

        public async Task GetCharacters(string name)
        {
            try
            {
                var response = await _starwarsServiceClient.GetCharacterAsync(new CharacterRequest {Name = name}, deadline: DateTime.UtcNow.AddSeconds(TimeOut));

                if(_printResultToConsoleOut)
                    PrintResult(response);
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task GetCharactersStream(IList<string> names)
        {
            try
            {
                using (var call = _starwarsServiceClient.GetCharacterStream(deadline: DateTime.UtcNow.AddSeconds(TimeOut)))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var response = call.ResponseStream.Current;
                            if (_printResultToConsoleOut)
                                PrintResult(response);
                        }
                    });

                    foreach (var name in names)
                    {
                        await call.RequestStream.WriteAsync(new CharacterRequest { Name = name });
                    }

                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void PrintResult(CharacterResponse response)
        {
            var result = $"Name: {response.Name}, Gender: {response.Gender}, Height: {response.Height}";

            Console.WriteLine(result);
        }
    }
}