using Prise.Plugin;
using System;

namespace Prise.Infrastructure
{
    public interface IProxyCreator<T> : IDisposable
    {
        IPluginBootstrapper CreateBootstrapperProxy(object remoteBootstrapper);
        T CreatePluginProxy(object remoteObject, IParameterConverter parameterConverter, IResultConverter resultConverter);
    }
}