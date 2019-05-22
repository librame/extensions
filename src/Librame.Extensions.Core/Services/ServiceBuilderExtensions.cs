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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 服务构建器静态扩展。
    /// </summary>
    public static class ServiceBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddServices(this IBuilder builder)
        {
            builder.Services.AddTransient<IAccessTokenService, InternalAccessTokenService>();
            builder.Services.AddTransient<IClockService, InternalClockService>();
            builder.Services.AddTransient<IPlatformService, InternalPlatformService>();

            return builder;
        }

    }
}
