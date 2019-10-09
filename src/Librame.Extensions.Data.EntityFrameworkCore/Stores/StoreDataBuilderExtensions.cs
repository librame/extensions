﻿#region License

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
            builder.Services.AddScoped(typeof(IStoreHub<>), typeof(StoreHub<>));

            builder.Services.AddScoped<IStoreIdentifier, StoreIdentifier>();
            builder.Services.AddScoped<IStoreInitializer, StoreInitializer>();

            return builder;
        }


        /// <summary>
        /// 添加标识符。
        /// </summary>
        /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddIdentifier<TIdentifier>(this IDataBuilder builder)
            where TIdentifier : class, IStoreIdentifier
        {
            builder.Services.TryReplace<IStoreIdentifier, TIdentifier>();
            builder.Services.AddScoped(provider => (TIdentifier)provider.GetRequiredService<IStoreIdentifier>());

            return builder;
        }

        /// <summary>
        /// 添加初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddInitializer<TInitializer>(this IDataBuilder builder)
            where TInitializer : class, IStoreInitializer
        {
            builder.Services.TryReplace<IStoreInitializer, TInitializer>();
            builder.Services.AddScoped(provider => (TInitializer)provider.GetRequiredService<IStoreInitializer>());

            return builder;
        }

        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="TStoreHub">指定的存储中心类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStoreHub<TStoreHub>(this IDataBuilder builder)
            where TStoreHub : class, IStoreHub
        {
            builder.Services.AddScoped<IStoreHub, TStoreHub>();
            builder.Services.AddScoped(provider => (TStoreHub)provider.GetRequiredService<IStoreHub>());

            return builder;
        }

        ///// <summary>
        ///// 添加存储中心。
        ///// </summary>
        ///// <typeparam name="TAccessor">指定的存储器类型。</typeparam>
        ///// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        ///// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        //internal static IDataBuilder AddStoreHubByAccessor<TAccessor>(this IDataBuilder builder)
        //    where TAccessor : DbContext, IAccessor
        //{
        //    var accessorType = typeof(TAccessor);
        //    var typeArguments = GetAccessorGenericTypes(accessorType);

        //    typeArguments = accessorType.YieldEnumerable().Concat(typeArguments).ToArray();
        //    var storeHubType = typeof(StoreHub<>).MakeGenericType(typeArguments);

        //    builder.Services.AddScoped(typeof(IStoreHub<>).MakeGenericType(typeArguments), storeHubType);

        //    if (!builder.Services.TryReplace(typeof(IStoreHub), storeHubType, throwIfNotFound: false))
        //        builder.Services.AddScoped(typeof(IStoreHub), storeHubType);

        //    return builder;
        //}

        //private static Type[] GetAccessorGenericTypes(Type accessorType)
        //{
        //    if (!accessorType.IsGenericType
        //        || accessorType.GenericTypeArguments.Length != 7
        //        || accessorType.GetInterface(nameof(IDbContextAccessorFlag)).IsNull())
        //    {
        //        if (accessorType.BaseType.IsNull())
        //            throw new ArgumentException($"Invalid '{accessorType}' inherits. Reference {typeof(DbContextAccessor<,,,,,,>)}");

        //        return GetAccessorGenericTypes(accessorType.BaseType);
        //    }

        //    return accessorType.GenericTypeArguments;
        //}

    }
}
