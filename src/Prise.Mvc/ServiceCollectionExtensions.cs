using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prise.Mvc.Infrastructure;

namespace Prise.Mvc
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Does all of the plumbing to add API Controllers from Prise Plugins.
        /// Limitiations:
        /// - No DispatchProxy can be used, backwards compatability is compromised (DispatchProxy requires an interface as base class, not ControllerBase)
        /// - Plugin Cache is set to Singleton because we cannot unload assemblies, this would disable the controller routing (and result in 404)
        /// - Assembly unloading is disabled, memory leaks can occur
        /// </summary>
        /// <typeparam name="T">The Plugin Contract Type</typeparam>
        /// <param name="builder"></param>
        /// <returns>A fully configured Prise setup that will load Controllers from Plugin Assemblies</returns>
        public static PluginLoadOptionsBuilder<T> AddPriseControllersAsPlugins<T>(this PluginLoadOptionsBuilder<T> builder)
        {
            var actionDescriptorChangeProvider = new PriseActionDescriptorChangeProvider();
            return builder
                // Use a singleton cache
                 .WithSingletonCache()
                 .ConfigureServices(services =>
                 services
                     // Registers the change provider
                     .AddSingleton<IPriseActionDescriptorChangeProvider>(actionDescriptorChangeProvider)
                     .AddSingleton<IActionDescriptorChangeProvider>(actionDescriptorChangeProvider)
                     // Registers the activator for controllers from plugin assemblies
                     .Replace(ServiceDescriptor.Transient<IControllerActivator, PriseControllersAsPluginActivator<T>>()))
                 // Makes sure controllers can be casted to the host's representation of ControllerBase
                 .WithHostType(typeof(ControllerBase));
        }
    }
}
