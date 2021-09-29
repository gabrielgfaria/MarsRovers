using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Threading.Tasks;

namespace MarsRovers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IApplication, Application>()
                .AddSingleton<IPlateuService, PlateuService>()
                .AddSingleton<IRoverService, RoverService>()
                .BuildServiceProvider();

            var app = serviceProvider.GetService<IApplication>();
            app.Run();
        }
    }
}
