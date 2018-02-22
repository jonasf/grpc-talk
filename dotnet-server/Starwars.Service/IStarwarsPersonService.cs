using System.Threading.Tasks;
using Starwars.Service.Contracts;

namespace Starwars.Service
{
    public interface IStarwarsPersonService
    {
        Task<StarwarsPerson> GetPerson(string name);
    }
}