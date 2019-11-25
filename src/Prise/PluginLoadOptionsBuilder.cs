﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Prise.AssemblyScanning;
using Prise.Infrastructure;

namespace Prise
{
    public class PluginLoadOptionsBuilder<T>
    {
        internal IAssemblyScanner<T> assemblyScanner;
        internal Type assemblyScannerType;
        internal IAssemblyScannerOptions<T> assemblyScannerOptions;
        internal Type assemblyScannerOptionsType;
        internal IPluginPathProvider pluginPathProvider;
        internal Type pluginPathProviderType;
        internal IAssemblyLoadStrategyProvider assemblyLoadStrategyProvider;
        internal Type assemblyLoadStrategyProviderType = typeof(DefaultAssemblyLoadStrategyProvider);
        internal ISharedServicesProvider sharedServicesProvider;
        internal Type sharedServicesProviderType;
        internal IRemotePluginActivator activator;
        internal Type activatorType;
        internal IProxyCreator<T> proxyCreator;
        internal Type proxyCreatorType;
        internal IResultConverter resultConverter;
        internal Type resultConverterType;
        internal IParameterConverter parameterConverter;
        internal Type parameterConverterType;
        internal IPluginAssemblyLoader<T> assemblyLoader;
        internal Type assemblyLoaderType;
        internal IPluginAssemblyNameProvider pluginAssemblyNameProvider;
        internal Type pluginAssemblyNameProviderType;
        internal IAssemblyLoadOptions assemblyLoadOptions;
        internal Type assemblyLoadOptionsType;
        internal INetworkAssemblyLoaderOptions networkAssemblyLoaderOptions;
        internal Type networkAssemblyLoaderOptionsType;
        internal Action<IServiceCollection> configureServices;
        internal IHostTypesProvider hostTypesProvider;
        internal Type hostTypesProviderType;
        internal IRemoteTypesProvider remoteTypesProvider;
        internal Type remoteTypesProviderType;
        internal IDependencyPathProvider dependencyPathProvider;
        internal Type dependencyPathProviderType;
        internal IProbingPathsProvider probingPathsProvider;
        internal Type probingPathsProviderType;
        internal IRuntimePlatformContext runtimePlatformContext;
        internal Type runtimePlatformContextType;
        internal IPluginSelector pluginSelector;
        internal Type pluginSelectorType;
        internal IDepsFileProvider depsFileProvider;
        internal Type depsFileProviderType;
        internal IPluginDependencyResolver pluginDependencyResolver;
        internal Type pluginDependencyResolverType;
        internal ITempPathProvider tempPathProvider;
        internal Type tempPathProviderType;
        internal INativeAssemblyUnloader nativeAssemblyUnloader;
        internal Type nativeAssemblyUnloaderType;
        internal IHostFrameworkProvider hostFrameworkProvider;
        internal Type hostFrameworkProviderType;

        internal PluginLoadOptionsBuilder()
        {
        }

        public PluginLoadOptionsBuilder<T> WithPluginPath(string path)
        {
            this.pluginPathProvider = new DefaultPluginPathProvider(path);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginPathProvider<TType>()
            where TType : IPluginPathProvider
        {
            this.pluginPathProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoadStrategyProvider(IAssemblyLoadStrategyProvider provider)
        {
            this.assemblyLoadStrategyProvider = provider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoadStrategyProvider<TType>()
            where TType : IAssemblyLoadStrategyProvider
        {
            this.assemblyLoadStrategyProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProxyCreator(IProxyCreator<T> proxyCreator)
        {
            this.proxyCreator = proxyCreator;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProxyCreator<TType>()
            where TType : IProxyCreator<T>
        {
            this.proxyCreatorType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginAssemblyName(string pluginAssemblyName)
        {
            this.pluginAssemblyNameProvider = new PluginAssemblyNameProvider(pluginAssemblyName);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginAssemblyNameProvider<TType>()
                   where TType : IPluginAssemblyNameProvider
        {
            this.pluginAssemblyNameProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithActivator(IRemotePluginActivator activator)
        {
            this.activator = activator;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithActivator<TType>()
            where TType : IRemotePluginActivator
        {
            this.activatorType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithParameterConverter(IParameterConverter parameterConverter)
        {
            this.parameterConverter = parameterConverter;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithParameterConverter<TType>()
            where TType : IParameterConverter
        {
            this.parameterConverterType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithResultConverter(IResultConverter resultConverter)
        {
            this.resultConverter = resultConverter;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithResultConverter<TType>()
            where TType : IResultConverter
        {
            this.resultConverterType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoader(IPluginAssemblyLoader<T> assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoader<TType>()
           where TType : IPluginAssemblyLoader<T>
        {
            this.assemblyLoaderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithLocalDiskAssemblyLoader(
            PluginPlatformVersion pluginPlatformVersion = null,
            NativeDependencyLoadPreference nativeDependencyLoadPreference = NativeDependencyLoadPreference.PreferInstalledRuntime
            )
        {
            if (pluginPlatformVersion == null)
                pluginPlatformVersion = PluginPlatformVersion.Empty();

            this.assemblyLoadOptions = new DefaultAssemblyLoadOptions(
                pluginPlatformVersion,
                false,
                nativeDependencyLoadPreference);

#if NETCORE3_0
            return this.WithAssemblyLoader<DefaultAssemblyLoaderWithNativeResolver<T>>();
#endif
#if NETCORE2_1
            return this.WithAssemblyLoader<DefaultAssemblyLoader<T>>();
#endif
        }

        public PluginLoadOptionsBuilder<T> WithLocalDiskAssemblyLoader<TType>()
            where TType : IAssemblyLoadOptions
        {
            this.assemblyLoadOptionsType = typeof(TType);

#if NETCORE3_0
            return this.WithAssemblyLoader<DefaultAssemblyLoaderWithNativeResolver<T>>();
#endif
#if NETCORE2_1
            return this.WithAssemblyLoader<DefaultAssemblyLoader<T>>();
#endif
        }

        public PluginLoadOptionsBuilder<T> WithNetworkAssemblyLoader(
            string baseUrl,
            PluginPlatformVersion pluginPlatformVersion = null,
            NativeDependencyLoadPreference nativeDependencyLoadPreference = NativeDependencyLoadPreference.PreferInstalledRuntime)
        {
            if (pluginPlatformVersion == null)
                pluginPlatformVersion = PluginPlatformVersion.Empty();

            this.networkAssemblyLoaderOptions = new NetworkAssemblyLoaderOptions(
                baseUrl,
                pluginPlatformVersion,
                false,
                nativeDependencyLoadPreference);

            this.depsFileProviderType = typeof(NetworkDepsFileProvider);
            this.pluginDependencyResolverType = typeof(NetworkPluginDependencyResolver);
            this.assemblyLoaderType = typeof(NetworkAssemblyLoader<T>);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithNetworkAssemblyLoader<TType>()
            where TType : INetworkAssemblyLoaderOptions
        {
            this.networkAssemblyLoaderOptionsType = typeof(TType);
            this.depsFileProviderType = typeof(NetworkDepsFileProvider);
            this.pluginDependencyResolverType = typeof(NetworkPluginDependencyResolver);
            this.assemblyLoaderType = typeof(NetworkAssemblyLoader<T>);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithNetworkTempPathProvider<TType>()
           where TType : ITempPathProvider
        {
            this.tempPathProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithNetworkTempPathProvider(ITempPathProvider tempPathProvider)
        {
            this.tempPathProvider = tempPathProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSharedServicesProvider<TType>()
           where TType : ISharedServicesProvider
        {
            this.sharedServicesProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> ConfigureServices(Action<IServiceCollection> configureServices)
        {
            this.configureServices = configureServices;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostTypeProvider<TType>()
            where TType : IHostTypesProvider
        {
            this.hostTypesProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostTypeProvider(IHostTypesProvider hostTypesProvider)
        {
            this.hostTypesProvider = hostTypesProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithRemoteTypesProvider<TType>()
            where TType : IRemoteTypesProvider
        {
            this.remoteTypesProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithRemoteTypesProvider(IRemoteTypesProvider remoteTypesProvider)
        {
            this.remoteTypesProvider = remoteTypesProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSelector(Func<IEnumerable<Type>, IEnumerable<Type>> pluginSelector)
        {
            this.pluginSelector = new PluginSelector(pluginSelector);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSelector<TType>()
            where TType : IPluginSelector
        {
            this.pluginSelectorType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostType(Type type)
        {
            var hostTypesProvider = this.hostTypesProvider as HostTypesProvider;
            if (hostTypesProvider == null)
                throw new PrisePluginException($"You're not using the default IHostTypesProvider {nameof(HostTypesProvider)}. Please add host types using your own provider.");
            hostTypesProvider.AddHostType(type);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithRemoteType(Type type)
        {
            var remoteTypesProvider = this.remoteTypesProvider as RemoteTypesProvider;
            if (remoteTypesProvider == null)
                throw new PrisePluginException($"You're not using the default IRemoteTypesProvider {nameof(RemoteTypesProvider)}. Please add remote types using your own provider.");
            remoteTypesProvider.AddRemoteType(type);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithDependencyPathProvider<TType>()
            where TType : IDependencyPathProvider
        {
            this.dependencyPathProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProbingPath(string path)
        {
            var probingPathsProvider = this.probingPathsProvider as ProbingPathsProvider;
            if (probingPathsProvider == null)
                throw new PrisePluginException($"You're not using the default IProbingPathsProvider {nameof(ProbingPathsProvider)}. Please add probing paths using your own provider.");
            probingPathsProvider.AddProbingPath(path);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProbingPathsProvider<TType>()
           where TType : IProbingPathsProvider
        {
            this.probingPathsProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProbingPathsProvider(IProbingPathsProvider probingPathsProvider)
        {
            this.probingPathsProvider = probingPathsProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithDependencyPathProvider(IDependencyPathProvider dependencyPathProvider)
        {
            this.dependencyPathProvider = dependencyPathProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostFrameworkProvider(IHostFrameworkProvider hostFrameworkProvider)
        {
            this.hostFrameworkProviderType = null;
            this.hostFrameworkProvider = hostFrameworkProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostFrameworkProvider<TType>()
           where TType : IHostFrameworkProvider
        {
            this.hostFrameworkProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> IgnorePlatformInconsistencies(bool ignore = true)
        {
            if (this.assemblyLoadOptionsType != null || this.networkAssemblyLoaderOptionsType != null || this.assemblyLoader != null)
                throw new PrisePluginException("Custom loaders and custom load options are not supported with IgnorePlatformInconsistencies(), please provide your own value for IgnorePlatformInconsistencies.");

            if (this.assemblyLoadOptions != null)
                this.assemblyLoadOptions = new DefaultAssemblyLoadOptions(
                    this.assemblyLoadOptions.PluginPlatformVersion,
                    ignore,
                    this.assemblyLoadOptions.NativeDependencyLoadPreference);

            if (this.networkAssemblyLoaderOptions != null)
                this.networkAssemblyLoaderOptions = new NetworkAssemblyLoaderOptions(
                   this.networkAssemblyLoaderOptions.BaseUrl,
                   this.networkAssemblyLoaderOptions.PluginPlatformVersion,
                   ignore,
                   this.networkAssemblyLoaderOptions.NativeDependencyLoadPreference);

            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostType<THostType>() => WithHostType(typeof(THostType));

        public PluginLoadOptionsBuilder<T> WithRemoteType<TRemoteType>() => WithRemoteType(typeof(TRemoteType));

        public PluginLoadOptionsBuilder<T> ConfigureSharedServices(Action<IServiceCollection> sharedServices)
        {
            if (this.sharedServicesProviderType != null)
                throw new PrisePluginException($"A custom {typeof(ISharedServicesProvider).Name} type cannot be used in combination with {nameof(ConfigureSharedServices)}service");

            var services = new ServiceCollection();
            sharedServices.Invoke(services);

            foreach (var sharedService in services)
                this
                    .WithHostType(sharedService.ServiceType)
                    .WithHostType(sharedService.ImplementationType ?? sharedService.ImplementationInstance?.GetType() ?? sharedService.ImplementationFactory?.Method.ReturnType)
                ; // If a shared service is added, it must be a added as a host type

            this.sharedServicesProvider = new DefaultSharedServicesProvider(services);
            this.activator = new DefaultRemotePluginActivator(this.sharedServicesProvider);
            return this;
        }

        public PluginLoadOptionsBuilder<T> ScanForAssemblies(Action<AssemblyScanningComposer<T>> composerOptions)
        {
            var composer = new AssemblyScanningComposer<T>();
            composerOptions(composer.WithDefaultOptions<DefaultAssemblyScanner<T>, DefaultAssemblyScannerOptions<T>>());
            var composition = composer.Compose();

            this.assemblyScanner = composition.Scanner;
            this.assemblyScannerType = composition.ScannerType;
            this.assemblyScannerOptions = composition.ScannerOptions;
            this.assemblyScannerOptionsType = composition.ScannerOptionsType;

            return this;
        }

        public PluginLoadOptionsBuilder<T> WithDefaultOptions(string pluginPath = null)
        {
            if (String.IsNullOrEmpty(pluginPath))
                pluginPath = Path.Join(GetLocalExecutionPath(), "Plugins");

            this.pluginPathProvider = new DefaultPluginPathProvider(pluginPath);
            this.dependencyPathProvider = new DependencyPathProvider(pluginPath);

            this.runtimePlatformContext = new RuntimePlatformContext();
            this.ScanForAssemblies(composer =>
                composer.WithDefaultOptions<DefaultAssemblyScanner<T>, DefaultAssemblyScannerOptions<T>>());

            //this.pluginPathProvider = new DefaultPluginPathProvider(pluginPath);
            this.pluginAssemblyNameProvider = new PluginAssemblyNameProvider($"{typeof(T).Name}.dll");
            this.sharedServicesProvider = new DefaultSharedServicesProvider(new ServiceCollection());
            this.activator = new DefaultRemotePluginActivator(this.sharedServicesProvider);
            this.proxyCreator = new PluginProxyCreator<T>();

            // Use System.Text.Json in 3.0
#if NETCORE3_0
            this.parameterConverter = new JsonSerializerParameterConverter();
            this.resultConverter = new JsonSerializerResultConverter();
            this.assemblyLoaderType = typeof(DefaultAssemblyLoaderWithNativeResolver<T>);
#endif
            // Use Newtonsoft.Json in 2.1
#if NETCORE2_1
            this.parameterConverter = new NewtonsoftParameterConverter();
            this.resultConverter = new NewtonsoftResultConverter();
            this.assemblyLoaderType = typeof(DefaultAssemblyLoader<T>);
#endif
            this.assemblyLoadOptions = new DefaultAssemblyLoadOptions(
                PluginPlatformVersion.Empty(),
                false,
                NativeDependencyLoadPreference.PreferInstalledRuntime);

            this.probingPathsProvider = new ProbingPathsProvider();

            var hostTypesProvider = new HostTypesProvider();
            hostTypesProvider.AddHostType(typeof(T)); // Add the contract to the host types
            hostTypesProvider.AddHostType(typeof(Prise.Plugin.PluginAttribute)); // Add the Prise.Infrastructure assembly to the host types
            hostTypesProvider.AddHostType(typeof(ServiceCollection));  // Adds the BuildServiceProvider assembly to the host types
            this.hostTypesProvider = hostTypesProvider;

            this.remoteTypesProvider = new RemoteTypesProvider();

            this.pluginSelector = new DefaultPluginSelector();
            this.depsFileProviderType = typeof(DefaultDepsFileProvider);
            this.pluginDependencyResolverType = typeof(DefaultPluginDependencyResolver);
            // Typically used for downloading and storing plugins from the network, but it could be useful for caching local plugins as well
            this.tempPathProviderType = typeof(UserProfileTempPathProvider);

            this.nativeAssemblyUnloaderType = typeof(DefaultNativeAssemblyUnloader);
            this.hostFrameworkProviderType = typeof(HostFrameworkProvider);

            return this;
        }

        internal IServiceCollection RegisterOptions(IServiceCollection services)
        {
            services
                .RegisterTypeOrInstance<IPluginPathProvider>(pluginPathProviderType, pluginPathProvider)
                .RegisterTypeOrInstance<IAssemblyLoadStrategyProvider>(assemblyLoadStrategyProviderType, assemblyLoadStrategyProvider)
                .RegisterTypeOrInstance<IAssemblyScanner<T>>(assemblyScannerType, assemblyScanner)
                .RegisterTypeOrInstance<IAssemblyScannerOptions<T>>(assemblyScannerOptionsType, assemblyScannerOptions)
                .RegisterTypeOrInstance<IProxyCreator<T>>(proxyCreatorType, proxyCreator)
                .RegisterTypeOrInstance<ISharedServicesProvider>(sharedServicesProviderType, sharedServicesProvider)
                .RegisterTypeOrInstance<IPluginAssemblyNameProvider>(pluginAssemblyNameProviderType, pluginAssemblyNameProvider)
                .RegisterTypeOrInstance<IRemotePluginActivator>(activatorType, activator)
                .RegisterTypeOrInstance<IResultConverter>(resultConverterType, resultConverter)
                .RegisterTypeOrInstance<IParameterConverter>(parameterConverterType, parameterConverter)
                .RegisterTypeOrInstance<IPluginAssemblyLoader<T>>(assemblyLoaderType, assemblyLoader)
                .RegisterTypeOrInstance<IHostTypesProvider>(hostTypesProviderType, hostTypesProvider)
                .RegisterTypeOrInstance<IRemoteTypesProvider>(remoteTypesProviderType, remoteTypesProvider)
                .RegisterTypeOrInstance<IDependencyPathProvider>(dependencyPathProviderType, dependencyPathProvider)
                .RegisterTypeOrInstance<IProbingPathsProvider>(probingPathsProviderType, probingPathsProvider)
                .RegisterTypeOrInstance<IRuntimePlatformContext>(runtimePlatformContextType, runtimePlatformContext)
                .RegisterTypeOrInstance<IPluginSelector>(pluginSelectorType, pluginSelector)
                .RegisterTypeOrInstance<IDepsFileProvider>(depsFileProviderType, depsFileProvider)
                .RegisterTypeOrInstance<IPluginDependencyResolver>(pluginDependencyResolverType, pluginDependencyResolver)
                .RegisterTypeOrInstance<ITempPathProvider>(tempPathProviderType, tempPathProvider)
                .RegisterTypeOrInstance<INativeAssemblyUnloader>(nativeAssemblyUnloaderType, nativeAssemblyUnloader)
                .RegisterTypeOrInstance<IHostFrameworkProvider>(hostFrameworkProviderType, hostFrameworkProvider)
                ;

            if (assemblyLoadOptions != null)
                services
                    .AddScoped<IAssemblyLoadOptions>(s => assemblyLoadOptions);
            if (assemblyLoadOptionsType != null)
                services
                    .AddScoped(typeof(IAssemblyLoadOptions), assemblyLoadOptionsType);

            if (networkAssemblyLoaderOptions != null)
                services
                    .AddScoped<INetworkAssemblyLoaderOptions>(s => networkAssemblyLoaderOptions)
                    .AddScoped<IAssemblyLoadOptions>(s => networkAssemblyLoaderOptions);
            if (networkAssemblyLoaderOptionsType != null)
                services
                    .AddScoped(typeof(INetworkAssemblyLoaderOptions), networkAssemblyLoaderOptionsType)
                    .AddScoped(typeof(IAssemblyLoadOptions), networkAssemblyLoaderOptionsType);

            configureServices?.Invoke(services);

            // Make use of DI by providing an injected instance of the registered services above
            return services.AddScoped<IPluginLoadOptions<T>, PluginLoadOptions<T>>();
        }

        private string GetLocalExecutionPath() => AppDomain.CurrentDomain.BaseDirectory;
    }

    internal static class PluginLoadOptionsBuilderExtensions
    {
        internal static IServiceCollection RegisterTypeOrInstance<TType>(this IServiceCollection services, Type type, TType instance)
            where TType : class
        {
            if (type != null)
                services.AddScoped(typeof(TType), type);
            else if (instance != null)
                services.AddScoped<TType>(s => instance);
            else
                throw new PrisePluginException($"Could not find type {type?.Name} or instance {typeof(TType).Name} to register");

            return services;
        }
    }
}