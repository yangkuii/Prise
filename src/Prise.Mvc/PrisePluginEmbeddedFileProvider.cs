using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Prise.Infrastructure;

namespace Prise.Mvc
{
    public class PrisePluginEmbeddedFileProvider<T> : IFileProvider
    {
        private readonly IPluginCache<T> cache;

        public PrisePluginEmbeddedFileProvider(IPluginCache<T> cache)
        {
            this.cache = cache;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var assembly = this.cache.GetAll().First();
            var provider = new EmbeddedFileProvider(assembly);
            return provider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var assembly = this.cache.GetAll().First();
            var provider = new EmbeddedFileProvider(assembly);
            return provider.GetFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            var assembly = this.cache.GetAll().First();
            var provider = new EmbeddedFileProvider(assembly);
            return provider.Watch(filter);
        }
    }
}
