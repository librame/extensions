#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Bootstrappers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 引导程序服务集合静态扩展。
    /// </summary>
    public static class BootstrapperServiceCollectionExtensions
    {
        internal static IServiceCollection UseBootstrapperStarter(this IServiceCollection services)
        {
            IBootstrapperStarter starter = null;

            if (services.TryGet<IBootstrapperStarter>(out ServiceDescriptor descriptor))
            {
                if (descriptor.ImplementationInstance.IsNotNull())
                {
                    starter = descriptor.ImplementationInstance as IBootstrapperStarter;
                }
                else if (descriptor.ImplementationType.IsNotNull())
                {
                    starter = descriptor.ImplementationType.EnsureCreate<IBootstrapperStarter>();
                }
            }

            if (starter.IsNull())
                starter = new BootstrapperStarter();

            return starter.Start(services);
        }


        /// <summary>
        /// 添加 Librame 引导程序启动器。
        /// </summary>
        /// <typeparam name="TStarter">指定的启动器类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddLibrameBootstrapperStarter<TStarter>(this IServiceCollection services)
            where TStarter : class, IBootstrapperStarter
            => services.AddSingleton<IBootstrapperStarter, TStarter>();

        /// <summary>
        /// 添加 Librame 引导程序启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="starter">给定的 <see cref="IBootstrapperStarter"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddLibrameBootstrapperStarter(this IServiceCollection services, IBootstrapperStarter starter)
            => services.AddSingleton(starter);
    }
}
