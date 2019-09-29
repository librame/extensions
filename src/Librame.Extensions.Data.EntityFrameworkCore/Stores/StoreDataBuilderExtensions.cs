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
using System.Linq;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储数据构建器静态扩展。
    /// </summary>
    public static class StoreDataBuilderExtensions
    {
        private static readonly Type _accessorMarkType = typeof(IAccessor);
        private static readonly Type _initializerMarkType = typeof(IStoreInitializer);
        private static readonly Type _storeHubMarkType = typeof(IStoreHub);


        /// <summary>
        /// 增加存储集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        internal static IDataBuilder AddStores(this IDataBuilder builder)
        {
            builder.Services.AddScoped<IStoreIdentifier, StoreIdentifierBase>();

            builder.Services.AddScoped(typeof(IStoreInitializer<>), typeof(StoreInitializerBase<>));
            builder.Services.AddScoped(typeof(IStoreInitializer<,>), typeof(StoreInitializerBase<,>));

            builder.Services.AddScoped(typeof(IStoreHub<>), typeof(StoreHubBase<>));
            builder.Services.AddScoped(typeof(IStoreHub<,,,,>), typeof(StoreHubBase<,,,,>));

            return builder;
        }


        /// <summary>
        /// 增加存储中心。
        /// </summary>
        /// <typeparam name="TStoreHub">指定继承自 <see cref="IStoreHub{TAccessor}"/> 的存储中心类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStoreHubWithAccessor<TStoreHub>(this IDataBuilder builder)
            where TStoreHub : class, IStoreHub
        {
            var storeHubType = typeof(TStoreHub);

            var serviceTypes = FindInterfaceWithAccessorTypes(storeHubType, _storeHubMarkType);
            serviceTypes.ForEach(type => builder.Services.AddScoped(type, storeHubType));

            builder.Services.AddScoped(serviceProvider =>
            {
                return (TStoreHub)serviceProvider.GetRequiredService(serviceTypes.Last());
            });

            return builder;
        }

        /// <summary>
        /// 增加存储中心。
        /// </summary>
        /// <typeparam name="TService">指定继承自 <see cref="IStoreHub{TAccessor}"/> 或 <see cref="IStoreHub{TAccessor, TAudit, TEntity, TMigration, TTenant}"/> 的存储中心服务类型。</typeparam>
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

            return builder;
        }


        /// <summary>
        /// 增加初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定继承自 <see cref="IStoreInitializer{TAccessor}"/> 的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddInitializerWithAccessor<TInitializer>(this IDataBuilder builder)
            where TInitializer : class, IStoreInitializer
        {
            var initializerType = typeof(TInitializer);

            var serviceTypes = FindInterfaceWithAccessorTypes(initializerType, _initializerMarkType);
            serviceTypes.ForEach(type => builder.Services.AddScoped(type, initializerType));

            builder.Services.AddScoped(serviceProvider =>
            {
                return (TInitializer)serviceProvider.GetRequiredService(serviceTypes.Last());
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
            builder.Services.AddScoped<TService, TImplementation>();
            builder.Services.AddScoped(serviceProvider =>
            {
                return (TImplementation)serviceProvider.GetRequiredService<TService>();
            });

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
            if (builder.Services.TryReplace<IStoreIdentifier, TIdentifier>())
            {
                builder.Services.AddScoped(serviceProvider =>
                {
                    return (TIdentifier)serviceProvider.GetRequiredService<IStoreIdentifier>();
                });
            }
            else
            {
                builder.AddIdentifier<IStoreIdentifier, TIdentifier>();
            }

            return builder;
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
            builder.Services.AddScoped<TService, TImplementation>();
            builder.Services.AddScoped(serviceProvider =>
            {
                return (TImplementation)serviceProvider.GetRequiredService<TService>();
            });

            return builder;
        }


        private static Type[] FindInterfaceWithAccessorTypes(Type findType, Type markType)
        {
            var withAccessorTypes = findType.GetInterfaces()
                .Where(p => p.IsAssignableToBaseType(markType)
                    && p.GenericTypeArguments.Length > 0
                    && p.GenericTypeArguments.Any(a => a.IsAssignableToBaseType(_accessorMarkType)))
                .ToArray();

            if (withAccessorTypes.IsEmpty())
                throw new ArgumentNullException($"The {findType} does not implement {markType.GetSimpleFullName()}<TAccessor>");

            return withAccessorTypes;
        }

    }
}
