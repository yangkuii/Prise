﻿#if NETCORE3_0
using Prise.Infrastructure;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Prise
{
    public class DefaultAssemblyLoadContextWithNativeResolver<T> : DefaultAssemblyLoadContext<T>
    {
        protected AssemblyDependencyResolver resolver;

        public DefaultAssemblyLoadContextWithNativeResolver(
            IPluginLogger<T> logger,
            IAssemblyLoadOptions<T> options,
            IHostFrameworkProvider hostFrameworkProvider,
            IHostTypesProvider hostTypesProvider,
            IRemoteTypesProvider<T> remoteTypesProvider,
            IDependencyPathProvider<T> dependencyPathProvider,
            IProbingPathsProvider<T> probingPathsProvider,
            IRuntimePlatformContext runtimePlatformContext,
            IDepsFileProvider<T> depsFileProvider,
            IPluginDependencyResolver<T> pluginDependencyResolver,
            INativeAssemblyUnloader nativeAssemblyUnloader,
            IAssemblyLoadStrategyProvider assemblyLoadStrategyProvider
        ) : base(
                logger,
                options,
                hostFrameworkProvider,
                hostTypesProvider,
                remoteTypesProvider,
                dependencyPathProvider,
                probingPathsProvider,
                runtimePlatformContext,
                depsFileProvider,
                pluginDependencyResolver,
                nativeAssemblyUnloader,
                assemblyLoadStrategyProvider
            )
        { }

        public override Assembly LoadPluginAssembly(IPluginLoadContext pluginLoadContext)
        {
            // contains rootpath + plugin folder + plugin assembly name
            // HostApplication/bin/Debug/netcoreapp3.0 + Plugins + Plugin.dll
            this.resolver = new AssemblyDependencyResolver(Path.Join(pluginLoadContext.PluginAssemblyPath, pluginLoadContext.PluginAssemblyName));
            return base.LoadPluginAssembly(pluginLoadContext);
        }

        public override Task<Assembly> LoadPluginAssemblyAsync(IPluginLoadContext pluginLoadContext)
        {
            // contains rootpath + plugin folder + plugin assembly name
            // HostApplication/bin/Debug/netcoreapp3.0 + Plugins + Plugin.dll
            this.resolver = new AssemblyDependencyResolver(Path.Join(pluginLoadContext.PluginAssemblyPath, pluginLoadContext.PluginAssemblyName));
            return base.LoadPluginAssemblyAsync(pluginLoadContext);
        }

        /// <summary>
        /// This override includes the netcore 3.0 resolver
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        protected override ValueOrProceed<Assembly> LoadFromDependencyContext(IPluginLoadContext pluginLoadContext, AssemblyName assemblyName)
        {
            var assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (!String.IsNullOrEmpty(assemblyPath) && File.Exists(assemblyPath))
            {
                return ValueOrProceed<Assembly>.FromValue(LoadFromAssemblyPath(assemblyPath), false);
            }

            return base.LoadFromDependencyContext(pluginLoadContext, assemblyName);
        }

        /// <summary>
        /// This override includes the netcore 3.0 resolver
        /// </summary>
        /// <param name="unmanagedDllName"></param>
        /// <returns></returns>
        protected override ValueOrProceed<string> LoadUnmanagedFromDependencyContext(IPluginLoadContext pluginLoadContext, string unmanagedDllName)
        {
            string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (!String.IsNullOrEmpty(libraryPath))
            {
                string runtimeCandidate = null;
                if (this.options.NativeDependencyLoadPreference == NativeDependencyLoadPreference.PreferInstalledRuntime)
                    // Prefer loading from runtime folder
                    runtimeCandidate = this.pluginDependencyResolver.ResolvePlatformDependencyPathToRuntime(this.options.PluginPlatformVersion, libraryPath);

                return ValueOrProceed<string>.FromValue(runtimeCandidate ?? libraryPath, false);
            }

            return base.LoadUnmanagedFromDependencyContext(pluginLoadContext, unmanagedDllName);
        }

        /// <summary>
        /// Load the assembly using the .NET Core 3 System.Runtime.InteropServices.NativeLibrary functionality 
        /// </summary>
        /// <param name="fullPathToNativeAssembly"></param>
        /// <returns>A loaded native library pointer</returns>
        protected override IntPtr LoadUnmanagedDllFromDependencyLookup(string fullPathToNativeAssembly) => System.Runtime.InteropServices.NativeLibrary.Load(fullPathToNativeAssembly);

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.disposing = true;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                this.hostTypesProvider = null;
                this.remoteTypesProvider = null;
                this.dependencyPathProvider = null;
                this.probingPathsProvider = null;
                this.runtimePlatformContext = null;
                this.depsFileProvider = null;
                this.pluginDependencyResolver = null;
                this.pluginDependencyContext = null;
                this.assemblyLoadStrategy = null;
                this.resolver = null;

                foreach (var nativeAssembly in this.loadedNativeLibraries)
                    this.nativeAssemblyUnloader.UnloadNativeAssembly(nativeAssembly.Key, nativeAssembly.Value);

                this.nativeAssemblyUnloader = null;
                this.loadedNativeLibraries = null;
            }
            this.disposed = true;
        }
    }
}
#endif