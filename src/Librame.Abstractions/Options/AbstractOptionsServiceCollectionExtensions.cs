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
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Options
{
    /// <summary>
    /// 抽象选项服务集合静态扩展。
    /// </summary>
    public static class AbstractOptionsServiceCollectionExtensions
    {

        /// <summary>
        /// 配置选项。
        /// </summary>
        /// <typeparam name="TConfigure">指定的配置类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureInstance">给定的配置实例。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection ConfigureOptions<TConfigure>(this IServiceCollection services, object configureInstance)
        {
            return services.ConfigureOptions(typeof(TConfigure), configureInstance);
        }

        /// <summary>
        /// 配置选项。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureType">给定的配置类型。</param>
        /// <param name="configureInstance">给定的配置实例。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection ConfigureOptions(this IServiceCollection services, Type configureType, object configureInstance)
        {
            services.AddOptions();

            var serviceTypes = FindIConfigureOptions(configureType);

            foreach (var serviceType in serviceTypes)
                services.AddSingleton(serviceType, configureInstance);

            return services;
        }


        private static IEnumerable<Type> FindIConfigureOptions(Type optionsType)
        {
            var serviceTypes = optionsType.GetTypeInfo().ImplementedInterfaces
                .Where(t => t.GetTypeInfo().IsGenericType &&
                (t.GetGenericTypeDefinition() == typeof(IConfigureOptions<>)
                || t.GetGenericTypeDefinition() == typeof(IPostConfigureOptions<>)));

            if (!serviceTypes.Any())
            {
                throw new InvalidOperationException(
                    IsAction(optionsType)
                    ? "No IConfigureOptions<> or IPostConfigureOptions<> implementations were found, did you mean to call Configure<> or PostConfigure<>?"
                    : "No IConfigureOptions<> or IPostConfigureOptions<> implementations were found.");
            }

            return serviceTypes;
        }

        private static bool IsAction(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>);
        }

    }
}
