using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Starwars.Service.Contracts;

namespace Starwars.Service.DataSource
{
    public class StarwarsPersonDatasource : IStarwarsPersonDatasource
    {
        private readonly IDictionary<string, StarwarsPerson> _starwarsPersons;

        public StarwarsPersonDatasource()
        {
            var datafile = string.Format("{0}/{1}", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "starwars-people.json");
            IList<StarwarsPerson> data = JsonConvert.DeserializeObject<IList<StarwarsPerson>>(File.ReadAllText(datafile));

            _starwarsPersons = new ConcurrentDictionary<string, StarwarsPerson>();
            foreach (var starwarsPerson in data)
            {
                _starwarsPersons[starwarsPerson.Name] = starwarsPerson;
            }
        }

        public async Task<StarwarsPerson> GetPerson(string name)
        {
            _starwarsPersons.TryGetValue(name, out var person);
            return person;
        }
    }
}
