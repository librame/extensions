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
using Librame.Extensions.Core;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 构建器服务集合静态扩展。
    /// </summary>
    public static class BuilderServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureAction">给定的配置选项动作（可选）。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddLibrame(this IServiceCollection services,
            Action<LibrameOptions> configureAction = null)
        {
            var options = new LibrameOptions();
            configureAction?.Invoke(options);

            // AddOptions
            if (options.OptionsName.IsNullOrEmpty())
                services.AddOptions();
            else
                services.AddOptions<LibrameOptions>(options.OptionsName);

            // AddLocalization
            services.AddLocalization(options.ConfigureLocalization);

            // AddLogging
            services.AddLogging(options.ConfigureLogging);

            // AddAutoRegistrationServices
            if (options.UseAutoRegistrationServices)
                services.AddAutoRegistrationServices();

            var builder = new InternalBuilder(services);

            return builder
                .AddConverters()
                .AddLocalizations()
                .AddServices();
        }

        /// <summary>
        /// 添加自动注册服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddAutoRegistrationServices(this IServiceCollection services)
        {
            var objectType = typeof(object);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
            {
                if (type.TryGetCustomAttribute(out AutoRegistrationServiceAttribute serviceAttribute))
                {
                    var serviceType = serviceAttribute.ServiceType;

                    if (serviceType == null && serviceAttribute.UseBaseTypeAsServiceType)
                    {
                        // 如果类型的基础类型为空或是接口，则基础类型返回 Object 类型
                        serviceType = type.BaseType != objectType ? type.BaseType
                            : type.GetInterfaces().FirstOrDefault();
                    }

                    if (serviceType == null)
                    {
                        // 使用当前类型为服务类型
                        serviceType = type;
                    }

                    switch (serviceAttribute.Lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(serviceType, type);
                            break;

                        case ServiceLifetime.Scoped:
                            services.AddScoped(serviceType, type);
                            break;

                        case ServiceLifetime.Transient:
                            services.AddTransient(serviceType, type);
                            break;

                        default:
                            break;
                    }
                }
            }

            return services;
        }

    }
}
