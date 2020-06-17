#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 抽象选项服务集合静态扩展。
    /// </summary>
    public static class AbstractionOptionsServiceCollectionExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        private static IEnumerable<Type> FindIConfigureOptions(Type type)
        {
            var serviceTypes = type.GetTypeInfo().ImplementedInterfaces
                .Where(t => t.IsGenericType &&
                (t.GetGenericTypeDefinition() == typeof(IConfigureOptions<>)
                || t.GetGenericTypeDefinition() == typeof(IPostConfigureOptions<>)));

            if (!serviceTypes.Any())
            {
                throw new InvalidOperationException(
                    IsAction()
                    ? "No IConfigureOptions<> or IPostConfigureOptions<> implementations were found, did you mean to call Configure<> or PostConfigure<>?"
                    : "No IConfigureOptions<> or IPostConfigureOptions<> implementations were found.");
            }

            return serviceTypes;

            // IsAction
            bool IsAction()
                => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>);
        }


        #region TryGetConfigureOptions

        /// <summary>
        /// 尝试获取配置选项已添加的服务描述符集合。
        /// </summary>
        /// <typeparam name="TConfigureOptions">指定的配置选项类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IEnumerable{ServiceDescriptor}"/>。</returns>
        public static IEnumerable<ServiceDescriptor> TryGetConfigureOptions<TConfigureOptions>(this IServiceCollection services)
            => services.TryGetConfigureOptions(typeof(TConfigureOptions));

        /// <summary>
        /// 尝试获取配置选项已添加的服务描述符集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptionsType">给定的配置选项类型。</param>
        /// <returns>返回 <see cref="IEnumerable{ServiceDescriptor}"/>。</returns>
        public static IEnumerable<ServiceDescriptor> TryGetConfigureOptions(this IServiceCollection services, Type configureOptionsType)
        {
            var serviceTypes = FindIConfigureOptions(configureOptionsType);

            foreach (var serviceType in serviceTypes)
            {
                if (services.TryGetAll(serviceType, out var descriptors))
                {
                    foreach (var descriptor in descriptors)
                        yield return descriptor;
                }
            }
        }

        #endregion


        #region TryReplaceConfigureOptions

        /// <summary>
        /// 尝试替换配置选项。
        /// </summary>
        /// <typeparam name="TConfigureOptions">指定的配置选项类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryReplaceConfigureOptions<TConfigureOptions>(this IServiceCollection services,
            bool throwIfNotFound = true)
            => services.TryReplaceConfigureOptions(typeof(TConfigureOptions), throwIfNotFound);

        /// <summary>
        /// 尝试替换配置选项。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptionsType">给定的配置选项类型。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryReplaceConfigureOptions(this IServiceCollection services,
            Type configureOptionsType, bool throwIfNotFound = true)
        {
            var serviceTypes = FindIConfigureOptions(configureOptionsType);

            foreach (var serviceType in serviceTypes)
                services.TryReplaceAll(serviceType, configureOptionsType, throwIfNotFound: throwIfNotFound);

            return true;
        }

        /// <summary>
        /// 尝试替换配置选项。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptionsInstance">给定的配置选项实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceConfigureOptions(this IServiceCollection services,
            object configureOptionsInstance, bool throwIfNotFound = true)
        {
            configureOptionsInstance.NotNull(nameof(configureOptionsInstance));

            var serviceTypes = FindIConfigureOptions(configureOptionsInstance.GetType());

            foreach (var serviceType in serviceTypes)
                services.TryReplaceAll(serviceType, configureOptionsInstance, throwIfNotFound: throwIfNotFound);

            return true;
        }

        #endregion

    }
}
