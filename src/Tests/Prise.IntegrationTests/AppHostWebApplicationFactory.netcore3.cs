using Prise.IntegrationTestsHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Prise.IntegrationTests
{
    public partial class AppHostWebApplicationFactory
       : WebApplicationFactory<Prise.IntegrationTestsHost.Startup>
    {
#if NETCORE3_0
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                    services.AddSingleton<ICommandLineArguments>(new CommandLineArgumentsLazy()))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Prise.IntegrationTestsHost.Startup>();
                });
        }
#endif
    }
}