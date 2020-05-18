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
    using Data.Stores;

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
            builder.Services.TryAddScoped(typeof(IStoreHub<,>), typeof(StoreHub<,>));
            builder.Services.TryAddScoped(typeof(IStoreHub<,,,,,,>), typeof(StoreHub<,,,,,,>));

            builder.AddStoreIdentifierGenerator<GuidStoreIdentifierGenerator>(replaced: false);
            builder.AddStoreInitializer<GuidStoreInitializer>(replaced: false);

            return builder;
        }


        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="THub">指定实现 <see cref="IStoreHub{TGenId, TIncremId}"/> 或 <see cref="IStoreHub{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 接口的存储中心类型，推荐从 <see cref="StoreHub{TGenId, TIncremId}"/> 或 <see cref="StoreHub{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 派生。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddStoreHub<THub>(this IDataBuilder builder)
            where THub : class, IStoreHub
        {
            builder.NotNull(nameof(builder));

            var hubTypeDefinition = typeof(IStoreHub<,>);
            var hubType = typeof(THub);

            Type hubTypeGeneric = null;
            if (hubType.IsImplementedInterface(hubTypeDefinition, out Type resultType))
            {
                hubTypeGeneric = hubTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);
                builder.Services.TryAddScoped(hubTypeGeneric, hubType);
                builder.Services.TryAddScoped(serviceProvider => (THub)serviceProvider.GetRequiredService(hubTypeGeneric));
            }

            var hubTypeFullDefinition = typeof(IStoreHub<,,,,,,>);
            if (hubType.IsImplementedInterface(hubTypeFullDefinition, out resultType))
            {
                hubTypeGeneric = hubTypeFullDefinition.MakeGenericType(resultType.GenericTypeArguments);
                builder.Services.TryAddScoped(hubTypeGeneric, hubType);
                builder.Services.TryAddScoped(serviceProvider => (THub)serviceProvider.GetRequiredService(hubTypeGeneric));
            }

            if (resultType.IsNull())
                throw new ArgumentException($"The store hub type '{hubType}' does not implement '{hubTypeDefinition}' or '{hubTypeFullDefinition}' interface.");

            return builder;
        }


        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IStoreIdentifierGenerator{TGenId}"/> 接口的存储标识符类型，推荐从 <see cref="AbstractStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="replaced">是否替换可能已注册的服务（可选；默认替换）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddStoreIdentifierGenerator<TGenerator>(this IDataBuilder builder, bool replaced = true)
            where TGenerator : class, IStoreIdentifierGenerator
        {
            builder.NotNull(nameof(builder));

            var generatorTypeDefinition = typeof(IStoreIdentifierGenerator<>);
            var generatorType = typeof(TGenerator);

            if (generatorType.IsImplementedInterface(generatorTypeDefinition, out Type resultType))
            {
                var generatorTypeGeneric = generatorTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);

                if (replaced)
                    builder.Services.TryReplace(generatorTypeGeneric, generatorType);
                else
                    builder.Services.TryAddSingleton(generatorTypeGeneric, generatorType);

                builder.Services.TryAddSingleton(serviceProvider => (TGenerator)serviceProvider.GetRequiredService(generatorTypeGeneric));
            }
            else
            {
                throw new ArgumentException($"The store identifier type '{generatorType}' does not implement '{generatorTypeDefinition}' interface.");
            }

            return builder;
        }

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定实现 <see cref="IStoreInitializer{TGenId}"/> 接口的存储初始化器类型，推荐从 <see cref="AbstractStoreInitializer{TGenId}"/> 派生。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="replaced">是否替换可能已注册的服务（可选；默认替换）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddStoreInitializer<TInitializer>(this IDataBuilder builder, bool replaced = true)
            where TInitializer : class, IStoreInitializer
        {
            builder.NotNull(nameof(builder));

            var initializerTypeDefinition = typeof(IStoreInitializer<>);
            var initializerType = typeof(TInitializer);

            if (initializerType.IsImplementedInterface(initializerTypeDefinition, out Type resultType))
            {
                var initializerTypeGeneric = initializerTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);

                if (replaced)
                    builder.Services.TryReplace(initializerTypeGeneric, initializerType);
                else
                    builder.Services.TryAddSingleton(initializerTypeGeneric, initializerType);

                builder.Services.TryAddSingleton(serviceProvider => (TInitializer)serviceProvider.GetRequiredService(initializerTypeGeneric));
            }
            else
            {
                throw new ArgumentException($"The store initializer type '{initializerType}' does not implement '{initializerTypeDefinition}' interface.");
            }

            return builder;
        }

    }
}
