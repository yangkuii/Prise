using Microsoft.AspNetCore.Mvc.Testing;
using Prise.IntegrationTestsHost;

namespace Prise.IntegrationTests
{
    public class CommandLineArgumentsLazy : ICommandLineArguments
    {
        public bool UseLazyService => true;
    }

    public partial class AppHostWebApplicationFactory
       : WebApplicationFactory<Prise.IntegrationTestsHost.Startup>
    {
        private bool useLazyServices = false;
        public void ConfigureLazyService()
        {
            this.useLazyServices = true;
        }
    }
}