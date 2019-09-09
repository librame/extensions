#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 抽象选项服务集合静态扩展。
    /// </summary>
    public static class AbstractionOptionsServiceCollectionExtensions
    {

        #region OnlyConfigure

        /// <summary>
        /// 仅配置选项实例（未集成 <see cref="OptionsServiceCollectionExtensions.AddOptions(IServiceCollection)"/>）。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TOptions}"/>。</param>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection OnlyConfigure<TOptions>(this IServiceCollection services,
            Action<TOptions> configureOptions = null, string name = null)
            where TOptions : class
        {
            if (configureOptions.IsNotNull())
            {
                var options = new ConfigureNamedOptions<TOptions>(name ?? Options.Options.DefaultName,
                    configureOptions);

                services.AddSingleton<IConfigureOptions<TOptions>>(options);
            }

            return services;
        }

        /// <summary>
        /// 仅配置后置选项实例（未集成 <see cref="OptionsServiceCollectionExtensions.AddOptions(IServiceCollection)"/>）。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TOptions}"/>。</param>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection OnlyPostConfigure<TOptions>(this IServiceCollection services,
            Action<TOptions> configureOptions = null, string name = null)
            where TOptions : class
        {
            if (configureOptions.IsNotNull())
            {
                var options = new PostConfigureOptions<TOptions>(name ?? Options.Options.DefaultName,
                    configureOptions);

                services.AddSingleton<IPostConfigureOptions<TOptions>>(options);
            }

            return services;
        }

        #endregion


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
                if (services.TryGet(serviceType, out ServiceDescriptor serviceDescriptor))
                    yield return serviceDescriptor;
            }
        }

        #endregion


        #region TryReplaceConfigureOptions

        /// <summary>
        /// 尝试替换配置选项。
        /// </summary>
        /// <typeparam name="TConfigureOptions">指定的配置选项类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryReplaceConfigureOptions<TConfigureOptions>(this IServiceCollection services)
            => services.TryReplaceConfigureOptions(typeof(TConfigureOptions));

        /// <summary>
        /// 尝试替换配置选项。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptionsType">给定的配置选项类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryReplaceConfigureOptions(this IServiceCollection services, Type configureOptionsType)
        {
            var serviceTypes = FindIConfigureOptions(configureOptionsType);

            foreach (var serviceType in serviceTypes)
                services.TryReplace(serviceType, configureOptionsType);

            return true;
        }

        /// <summary>
        /// 尝试替换配置选项。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptionsInstance">给定的配置选项实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryReplaceConfigureOptions(this IServiceCollection services, object configureOptionsInstance)
        {
            var serviceTypes = FindIConfigureOptions(configureOptionsInstance.GetType());

            foreach (var serviceType in serviceTypes)
                services.TryReplace(serviceType, configureOptionsInstance);

            return true;
        }

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

            bool IsAction()
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>);
            };
        }

        #endregion

    }
}
