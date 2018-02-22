using System.Threading.Tasks;
using Starwars.Service.Contracts;
using Starwars.Service.DataSource;

namespace Starwars.Service
{
    public class StarwarsPersonService : IStarwarsPersonService
    {
        private readonly IStarwarsPersonDatasource _starwarsPersonDatasource;

        public StarwarsPersonService(IStarwarsPersonDatasource starwarsPersonDatasource)
        {
            _starwarsPersonDatasource = starwarsPersonDatasource;
        }

        public async Task<StarwarsPerson> GetPerson(string name)
        {
            return await _starwarsPersonDatasource.GetPerson(name);
        }
    }
}