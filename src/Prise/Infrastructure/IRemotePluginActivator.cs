using Prise.Plugin;
using System;
using System.Reflection;

namespace Prise.Infrastructure
{
    public interface IRemotePluginActivator : IDisposable
    {
        object CreateRemoteBootstrapper(Type bootstrapperType, Assembly assembly);
        object CreateRemoteInstance(Type pluginType, Assembly assembly, IPluginBootstrapper bootstrapper = null, MethodInfo factoryMethod = null);
    }
}