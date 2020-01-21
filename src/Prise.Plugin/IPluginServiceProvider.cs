using System;

namespace Prise.Plugin
{
    public interface IPluginServiceProvider
    {
        T GetPluginService<T>();
        object GetSharedHostService(Type type);
    }
}
