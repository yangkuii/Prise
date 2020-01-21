﻿using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Prise.Infrastructure;

namespace Prise
{
    [DebuggerDisplay("{ProvideSharedServices()?.Count()}")]
    public class DefaultSharedServicesProvider<T> : ISharedServicesProvider<T>
    {
        private readonly IServiceCollection services;
        protected bool disposed = false;

        public DefaultSharedServicesProvider(IServiceCollection services)
        {
            this.services = services;
        }

        public IServiceCollection ProvideSharedServices() => this.services;

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
