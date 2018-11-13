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
using Microsoft.Extensions.Localization;

namespace Librame.Builders
{
    using Resources;

    /// <summary>
    /// 资源构建器静态扩展。
    /// </summary>
    public static class ResourceBuilderExtensions
    {

        /// <summary>
        /// 注册资源集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddResources(this IBuilder builder)
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, DefaultEnhancedStringLocalizerFactory>());
            builder.Services.AddTransient(typeof(IEnhancedStringLocalizer<>), typeof(DefaultEnhancedStringLocalizer<>));

            return builder;
        }

    }
}
