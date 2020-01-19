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
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Core.Bootstrappers
{
    using Utilities;

    /// <summary>
    /// 引导程序启动器。
    /// </summary>
    public class BootstrapperStarter : IBootstrapperStarter
    {
        /// <summary>
        /// 启动启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public virtual IServiceCollection Start(IServiceCollection services)
        {
            services.NotNull(nameof(services));

            foreach (var boot in LoadBootstrappers())
            {
                boot.Run(services);
            }

            return services;
        }

        /// <summary>
        /// 加载当前第三方程序集集合包含的所有引导程序列表。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyList{IBootstrapper}"/>。</returns>
        protected virtual IReadOnlyList<IBootstrapper> LoadBootstrappers()
        {
            var baseType = typeof(IBootstrapper);

            var bootstrappers = AssemblyUtility.CurrentAssembliesWithoutSystem
                .SelectMany(a => a.ExportedTypes)
                .Where(t => baseType.IsAssignableFromTargetType(t) && t.IsConcreteType())
                .Select(t => t.EnsureCreate<IBootstrapper>())
                .ToList();

            bootstrappers.Sort();

            return bootstrappers;
        }

    }
}
