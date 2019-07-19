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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 内部存储数据构建器静态扩展。
    /// </summary>
    internal static class InternalStoreDataBuilderExtensions
    {
        /// <summary>
        /// 添加存储集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStores(this IDataBuilder builder)
        {
            builder.Services.AddScoped(typeof(IStoreHub<>), typeof(StoreHubBase<>));
            builder.Services.AddScoped(typeof(IStoreHub<,,>), typeof(StoreHubBase<,,>));

            return builder;
        }

    }
}
