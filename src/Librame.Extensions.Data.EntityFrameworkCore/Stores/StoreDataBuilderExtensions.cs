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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储数据构建器静态扩展。
    /// </summary>
    public static class StoreDataBuilderExtensions
    {
        /// <summary>
        /// 添加存储集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        internal static IDataBuilder AddStores(this IDataBuilder builder)
        {
            builder.Services.TryAddScoped(typeof(IStoreHub<>), typeof(StoreHub<>));

            builder.Services.TryAddScoped<IStoreIdentifier, StoreIdentifier>();
            builder.Services.TryAddScoped<IStoreInitializer, StoreInitializer>();

            return builder;
        }


        /// <summary>
        /// 添加标识符。
        /// </summary>
        /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddIdentifier<TIdentifier>(this IDataBuilder builder)
            where TIdentifier : class, IStoreIdentifier
        {
            builder.Services.TryReplace<IStoreIdentifier, TIdentifier>();
            builder.Services.TryAddScoped(provider => (TIdentifier)provider.GetRequiredService<IStoreIdentifier>());

            return builder;
        }

        /// <summary>
        /// 添加初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddInitializer<TInitializer>(this IDataBuilder builder)
            where TInitializer : class, IStoreInitializer
        {
            builder.Services.TryReplace<IStoreInitializer, TInitializer>();
            builder.Services.TryAddScoped(provider => (TInitializer)provider.GetRequiredService<IStoreInitializer>());

            return builder;
        }

        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="TStoreHub">指定的存储中心类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddStoreHub<TStoreHub>(this IDataBuilder builder)
            where TStoreHub : class, IStoreHub
        {
            builder.Services.TryAddScoped<IStoreHub, TStoreHub>();
            builder.Services.TryAddScoped(provider => (TStoreHub)provider.GetRequiredService<IStoreHub>());

            return builder;
        }

    }
}
