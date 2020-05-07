#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Librame.Extensions.Core.Starters
{
    using Utilities;

    /// <summary>
    /// 预启动器工厂。
    /// </summary>
    public class PreStarterFactory : IPreStarterFactory
    {
        /// <summary>
        /// 创建预启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{IPrestarter}"/>。</returns>
        public virtual IServiceCollection Create(IServiceCollection services)
        {
            services.NotNull(nameof(services));

            var preStarters = AssemblyUtility.CurrentExportedInstancesWithoutSystem<IPreStarter>();
            foreach (var preStarter in preStarters)
            {
                preStarter.Start(services);
            }

            services.TryAddSingleton<IEnumerable<IPreStarter>>(preStarters);

            return services;
        }

    }
}
