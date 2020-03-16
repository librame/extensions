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
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    using Stores;

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

            builder.Services.TryAddSingleton<IStoreInitializer, StoreInitializer>();
            builder.Services.TryAddSingleton<IStoreIdentifier, StoreIdentifier>();

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
            builder.NotNull(nameof(builder));

            builder.Services.TryAddScoped<IStoreHub, TStoreHub>();
            builder.Services.TryAddScoped(provider => (TStoreHub)provider.GetRequiredService<IStoreHub>());

            return builder;
        }

        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="TStoreHub">指定的存储中心类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationFactory">给定的实现工厂方法。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddStoreHub<TStoreHub>(this IDataBuilder builder,
            Func<IServiceProvider, TStoreHub> implementationFactory)
            where TStoreHub : class, IStoreHub
        {
            builder.NotNull(nameof(builder));

            builder.Services.TryAddScoped(implementationFactory);

            return builder;
        }


        /// <summary>
        /// 添加存储标识符。
        /// </summary>
        /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddStoreIdentifier<TIdentifier>(this IDataBuilder builder)
            where TIdentifier : class, IStoreIdentifier
        {
            builder.NotNull(nameof(builder));

            builder.Services.TryReplace<IStoreIdentifier, TIdentifier>();
            builder.Services.TryAddSingleton(provider => (TIdentifier)provider.GetRequiredService<IStoreIdentifier>());

            return builder;
        }

        /// <summary>
        /// 添加存储标识符。
        /// </summary>
        /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationFactory">给定的实现工厂方法。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddStoreIdentifier<TIdentifier>(this IDataBuilder builder,
            Func<IServiceProvider, TIdentifier> implementationFactory)
            where TIdentifier : class, IStoreIdentifier
        {
            builder.NotNull(nameof(builder));

            builder.Services.TryAddSingleton(implementationFactory);

            return builder;
        }


        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddStoreInitializer<TInitializer>(this IDataBuilder builder)
            where TInitializer : class, IStoreInitializer
        {
            builder.NotNull(nameof(builder));

            builder.Services.TryReplace<IStoreInitializer, TInitializer>();
            builder.Services.TryAddSingleton(provider => (TInitializer)provider.GetRequiredService<IStoreInitializer>());

            return builder;
        }

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationFactory">给定的实现工厂方法。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddStoreInitializer<TInitializer>(this IDataBuilder builder,
            Func<IServiceProvider, TInitializer> implementationFactory)
            where TInitializer : class, IStoreInitializer
        {
            builder.NotNull(nameof(builder));

            builder.Services.TryAddSingleton(implementationFactory);

            return builder;
        }

    }
}
