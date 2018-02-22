using System.Threading.Tasks;
using Starwars.Service.Contracts;

namespace Starwars.Service.DataSource
{
    public interface IStarwarsPersonDatasource
    {
        Task<StarwarsPerson> GetPerson(string name);
    }
}