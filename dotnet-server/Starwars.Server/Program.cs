using System;
using Autofac;
using Grpc.Core;
using Starwars.Service;
using Starwars.Service.DataSource;

namespace Starwars.Server
{
    class Program
    {
        const int Port = 50051;

        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigureService();

            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { StarwarsService.BindService(Container.Resolve<StarwarsServiceImpl>()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Starwars server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

        private static void ConfigureService()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<StarwarsPersonDatasource>().As<IStarwarsPersonDatasource>();
            containerBuilder.RegisterType<StarwarsPersonService>().As<IStarwarsPersonService>();
            containerBuilder.RegisterType<StarwarsServiceImpl>().UsingConstructor(typeof(IStarwarsPersonService));
            Container = containerBuilder.Build();
        }
    }
}
