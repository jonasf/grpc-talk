using System;
using System.Threading.Tasks;
using Grpc.Core;
using Starwars.Service;

namespace Starwars.Server
{
    public class StarwarsServiceImpl : StarwarsService.StarwarsServiceBase
    {
        private readonly IStarwarsPersonService _starwarsPersonService;
        public StarwarsServiceImpl(IStarwarsPersonService starwarsPersonService)
        {
            _starwarsPersonService = starwarsPersonService;
        }

        public override async Task<CharacterResponse> GetCharacter(CharacterRequest request, ServerCallContext context)
        {
            try
            {
                var starwarsPerson = await _starwarsPersonService.GetPerson(request.Name);
                return new CharacterResponse
                {
                    Name = starwarsPerson.Name,
                    Gender = starwarsPerson.Gender,
                    Height = starwarsPerson.Height
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public override async Task GetCharacterStream(IAsyncStreamReader<CharacterRequest> requestStream, IServerStreamWriter<CharacterResponse> responseStream, ServerCallContext context)
        {
            try
            {
                while (await requestStream.MoveNext(context.CancellationToken))
                {
                    var req = requestStream.Current;
                    var starwarsPerson = await _starwarsPersonService.GetPerson(req.Name);
                    await responseStream.WriteAsync(new CharacterResponse
                    {
                        Name = starwarsPerson.Name,
                        Gender = starwarsPerson.Gender,
                        Height = starwarsPerson.Height
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
