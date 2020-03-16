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
using System.Linq;

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

            var preStarters = GetPreStarters();

            foreach (var preStarter in preStarters)
            {
                preStarter.Start(services);
            }

            services.TryAddSingleton<IEnumerable<IPreStarter>>(preStarters);

            return services;
        }

        /// <summary>
        /// 获取当前第三方程序集集合包含的所有预启动器列表。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyList{IBootstrapper}"/>。</returns>
        protected virtual IReadOnlyList<IPreStarter> GetPreStarters()
        {
            var baseType = typeof(IPreStarter);

            var preStarters = AssemblyUtility.CurrentAssembliesWithoutSystem
                .SelectMany(a => a.ExportedTypes)
                .Where(t => baseType.IsAssignableFromTargetType(t) && t.IsConcreteType())
                .Select(t => t.EnsureCreate<IPreStarter>())
                .ToList();

            preStarters.Sort();

            return preStarters;
        }

    }
}
