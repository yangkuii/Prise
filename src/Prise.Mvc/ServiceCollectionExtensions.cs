using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
#if NETCORE3_0

using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
#endif
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        public static PluginLoadOptionsBuilder<T> AddPriseControllersAsPlugins<T>(this PluginLoadOptionsBuilder<T> builder, string webRootPath)
        {
            var actionDescriptorChangeProvider = new PriseActionDescriptorChangeProvider();
#if NETCORE2_1
            System.Diagnostics.Debugger.Break();
#endif
#if NETCORE3_0
            System.Diagnostics.Debugger.Break();
#endif

            return builder
                 // Use a singleton cache
                 .WithSingletonCache()
                 .ConfigureServices(services =>
                 services
#if NETCORE2_1
                    .Configure<RazorViewEngineOptions>(options =>
                     {
                         options.FileProviders.Add(new PrisePluginViewsAssemblyFileProvider<T>(webRootPath));
                     })
#endif
#if NETCORE3_0
                    .Configure<MvcRazorRuntimeCompilationOptions>(options =>
                     {
                         options.FileProviders.Add(new PrisePluginViewsAssemblyFileProvider<T>(webRootPath));
                     })
#endif
                    // Registers the static Plugin Cache Accessor
                    .AddSingleton<IPluginCacheAccessorBootstrapper<T>, StaticPluginCacheAccessorBootstrapper<T>>()
                    // Registers the change provider
                    .AddSingleton<IPriseActionDescriptorChangeProvider>(actionDescriptorChangeProvider)
                    .AddSingleton<IActionDescriptorChangeProvider>(actionDescriptorChangeProvider)
                    // Registers the activator for controllers from plugin assemblies
                    .Replace(ServiceDescriptor.Transient<IControllerActivator, PriseControllersAsPluginActivator<T>>()))
                 // Makes sure controllers can be casted to the host's representation of ControllerBase
                 .WithHostType(typeof(ControllerBase))
                 .WithHostType(typeof(ITempDataDictionaryFactory))
            ;
        }
    }
}
