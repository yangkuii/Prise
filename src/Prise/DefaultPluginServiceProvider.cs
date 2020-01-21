using System;
using System.Linq;
using Prise.Plugin;

namespace Prise
{
    public class DefaultPluginServiceProvider : IPluginServiceProvider
    {
        private readonly IServiceProvider localProvider;
        private readonly Type[] sharedTypes;

        public DefaultPluginServiceProvider(IServiceProvider localProvider, Type[] sharedTypes)
        {
            this.localProvider = localProvider;
            this.sharedTypes = sharedTypes;
        }

        public object GetPluginService(Type type)
        {
            var instanceType = this.sharedTypes.FirstOrDefault(t => t.Name == type.Name);
            var instance = this.localProvider.GetService(instanceType);
            return instance;
        }

        public T GetPluginService<T>()
        {
            return (T)this.localProvider.GetService(typeof(T));
        }

        public object GetSharedHostService(Type type)
        {
            var instanceType = this.sharedTypes.FirstOrDefault(t => t.Name == type.Name);
            return this.localProvider.GetService(instanceType);
        }
    }
}
