using System;
using Prise.Infrastructure;
using Prise.Plugin;

namespace Prise
{
    public abstract class PluginProxyCreator
    {
        public static TProxyType CreateProxy<TProxyType>(object remoteObject, IParameterConverter parameterConverter, IResultConverter resultConverter)
        {
            var proxy = PluginProxy<TProxyType>.Create();
            ((PluginProxy<TProxyType>)proxy)
                .SetRemoteObject(remoteObject)
                .SetParameterConverter(parameterConverter)
                .SetResultConverter(resultConverter);
            return (TProxyType)proxy;
        }
    }

    public class PluginProxyCreator<T> : PluginProxyCreator,  IProxyCreator<T>
    {
        protected bool disposed = false;

        public IPluginBootstrapper CreateBootstrapperProxy(object remoteBootstrapper) =>
            CreateProxy<IPluginBootstrapper>(remoteBootstrapper, new PassthroughParameterConverter(), new PassthroughResultConverter());

        public T CreatePluginProxy(object remoteObject, IParameterConverter parameterConverter, IResultConverter resultConverter) =>
            CreateProxy<T>(remoteObject, parameterConverter, resultConverter);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                // Nothing to do here               
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}