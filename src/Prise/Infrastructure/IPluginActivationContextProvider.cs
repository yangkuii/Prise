using System;
using System.Reflection;

namespace Prise.Infrastructure
{
    public interface IPluginActivationContextProvider<T>
    {
        PluginActivationContext ProvideActivationContext(Type remoteType, ref Assembly pluginAssembly);
    }
}
