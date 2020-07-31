#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Starters
{
    using Utilities;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class PreStarterFactory : IPreStarterFactory
    {
        private static IReadOnlyList<IPreStarter> _preStarters
            = AssemblyUtility.CreateCurrentThirdPartyExportedInstances<IPreStarter>();


        public virtual IServiceCollection Create(IServiceCollection services)
        {
            services.NotNull(nameof(services));

            foreach (var preStarter in _preStarters)
            {
                preStarter.Start(services);
            }

            services.TryAddSingleton<IEnumerable<IPreStarter>>(_preStarters);

            return services;
        }

    }
}
