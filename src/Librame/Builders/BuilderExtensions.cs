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

namespace Librame.Builders
{
    /// <summary>
    /// 构建器静态扩展。
    /// </summary>
    public static class BuilderExtensions
    {

        private static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            return services.AddLogging()
                .AddOptions();
        }

        /// <summary>
        /// 注册 Librame 服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddLibrame(this IServiceCollection services)
        {
            return services.AddDependencies()
                .AsDefaultBuilder()
                .AddBuffers()
                .AddConverters();
        }

        /// <summary>
        /// 转换为默认构建器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AsDefaultBuilder(this IServiceCollection services)
        {
            return new DefaultBuilder(services);
        }

    }
}
