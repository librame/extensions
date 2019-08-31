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
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储数据构建器静态扩展。
    /// </summary>
    public static class StoreDataBuilderExtensions
    {
        private static IDataBuilder AccessorTypeNotNull(this IDataBuilder builder)
        {
            if (builder.AccessorType.IsNull())
                throw new ArgumentException($"Required {nameof(builder)}.AddAccessor<TAccessor>()");

            return builder;
        }


        /// <summary>
        /// 增加存储中心。
        /// </summary>
        /// <typeparam name="TStoreHub">指定继承自 <see cref="IStoreHub{TAccessor}"/> 的存储中心类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStoreHub<TStoreHub>(this IDataBuilder builder)
            where TStoreHub : class, IStoreHub
        {
            builder.AccessorTypeNotNull();

            var serviceType = typeof(IStoreHub<>).MakeGenericType(builder.AccessorType);

            builder.Services.AddScoped(serviceType, typeof(TStoreHub));
            builder.Services.AddScoped(serviceProvider =>
            {
                return (TStoreHub)serviceProvider.GetRequiredService(serviceType);
            });

            return builder;
        }

        /// <summary>
        /// 增加存储中心。
        /// </summary>
        /// <typeparam name="TService">指定继承自 <see cref="IStoreHub{TAccessor}"/> 或 <see cref="IStoreHub{TAccessor, TAudit, TTenant}"/> 的存储中心服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定 <typeparamref name="TService"/> 的实现类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStoreHub<TService, TImplementation>(this IDataBuilder builder)
            where TService : class, IStoreHub
            where TImplementation : class, TService
        {
            builder.Services.AddScoped<TService, TImplementation>();
            builder.Services.AddScoped(serviceProvider =>
            {
                return (TImplementation)serviceProvider.GetRequiredService<TService>();
            });

            //builder.Services.AddScoped(typeof(IStoreHub<>), typeof(StoreHubBase<>));
            //builder.Services.AddScoped(typeof(IStoreHub<,,>), typeof(StoreHubBase<,,>));

            return builder;
        }


        /// <summary>
        /// 增加初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定继承自 <see cref="IStoreInitializer{TAccessor}"/> 的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddInitializer<TInitializer>(this IDataBuilder builder)
            where TInitializer : class, IStoreInitializer
        {
            builder.AccessorTypeNotNull();

            var serviceType = typeof(IStoreInitializer<>).MakeGenericType(builder.AccessorType);

            builder.Services.AddScoped(serviceType, typeof(TInitializer));
            builder.Services.AddScoped(serviceProvider =>
            {
                return (TInitializer)serviceProvider.GetRequiredService(serviceType);
            });

            return builder;
        }

        /// <summary>
        /// 增加初始化器。
        /// </summary>
        /// <typeparam name="TService">指定继承自 <see cref="IStoreInitializer{TAccessor}"/> 或 <see cref="IStoreInitializer{TAccessor, TIdentifier}"/> 的访问器服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定 <typeparamref name="TService"/> 的实现类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddInitializer<TService, TImplementation>(this IDataBuilder builder)
            where TService : class, IStoreInitializer
            where TImplementation : class, TService
        {
            builder.Services.AddSingleton<TService, TImplementation>();
            builder.Services.AddSingleton(serviceProvider =>
            {
                return (TImplementation)serviceProvider.GetRequiredService<TService>();
            });

            //builder.Services.AddSingleton(typeof(IStoreInitializer<>), typeof(StoreInitializerBase<>));
            //builder.Services.AddSingleton(typeof(IStoreInitializer<,>), typeof(StoreInitializerBase<,>));

            return builder;
        }


        /// <summary>
        /// 增加标识符。
        /// </summary>
        /// <typeparam name="TIdentifier">指定继承自 <see cref="IStoreIdentifier"/> 的标识符类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddIdentifier<TIdentifier>(this IDataBuilder builder)
            where TIdentifier : class, IStoreIdentifier
        {
            return builder.AddIdentifier<IStoreIdentifier, TIdentifier>();
        }

        /// <summary>
        /// 增加标识符。
        /// </summary>
        /// <typeparam name="TService">指定继承自 <see cref="IStoreIdentifier"/> 的标识符服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定 <typeparamref name="TService"/> 的实现类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddIdentifier<TService, TImplementation>(this IDataBuilder builder)
            where TService : class, IStoreIdentifier
            where TImplementation : class, TService
        {
            builder.Services.AddSingleton<TService, TImplementation>();
            builder.Services.AddSingleton(serviceProvider =>
            {
                return (TImplementation)serviceProvider.GetRequiredService<TService>();
            });

            return builder;
        }

    }
}
