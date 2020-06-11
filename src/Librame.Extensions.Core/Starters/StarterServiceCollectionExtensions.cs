#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Starters;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 启动器服务集合静态扩展。
    /// </summary>
    public static class StarterServiceCollectionExtensions
    {
        internal static IServiceCollection UsePreStarter(this IServiceCollection services)
        {
            if (services.TryGetAll<IPreStarterFactory>(out var descriptors))
            {
                descriptors.ForEach(descriptor =>
                {
                    var factory = InitializeFactory(descriptor);
                    factory.Create(services);
                });
            }

            return services;
        }

        private static IPreStarterFactory InitializeFactory(ServiceDescriptor descriptor)
        {
            IPreStarterFactory factory = null;

            if (descriptor.ImplementationInstance.IsNotNull())
            {
                factory = descriptor.ImplementationInstance as IPreStarterFactory;
            }
            else if (descriptor.ImplementationType.IsNotNull())
            {
                factory = descriptor.ImplementationType.EnsureCreate<IPreStarterFactory>();
            }

            if (factory.IsNull())
                factory = new PreStarterFactory();

            return factory;
        }


        /// <summary>
        /// 添加 Librame 预启动器。
        /// </summary>
        /// <typeparam name="TStarter">指定的启动器类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddLibramePreStarter<TStarter>(this IServiceCollection services)
            where TStarter : class, IPreStarterFactory
            => services.AddSingleton<IPreStarterFactory, TStarter>();

        /// <summary>
        /// 添加 Librame 预启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="starter">给定的 <see cref="IPreStarterFactory"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddLibramePreStarter(this IServiceCollection services, IPreStarterFactory starter)
            => services.AddSingleton(starter);
    }
}
