namespace Prise.Proxy
{
    public static class ProxyCreator
    {
        public static TProxyType CreateProxy<TProxyType>(
            object remoteObject,
            IParameterConverter parameterConverter = null,
            IResultConverter resultConverter = null)
        {
            if (parameterConverter == null)
                parameterConverter = new PassthroughParameterConverter();

            if (resultConverter == null)
                resultConverter = new PassthroughResultConverter();

            var proxy = PriseProxy<TProxyType>.Create();
            ((PriseProxy<TProxyType>)proxy)
                .SetRemoteObject(remoteObject)
                .SetParameterConverter(parameterConverter)
                .SetResultConverter(resultConverter);

            return (TProxyType)proxy;
        }
    }
}
