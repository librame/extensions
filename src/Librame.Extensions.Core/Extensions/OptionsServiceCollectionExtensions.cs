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
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 选项服务集合静态扩展。
    /// </summary>
    public static class OptionsServiceCollectionExtensions
    {
        private static readonly Type _optionsConfiguratorType
            = typeof(IOptionsConfigurator);


        /// <summary>
        /// 添加所有选项配置器集合。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖选项类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <typeparamref name="TDependencyOptions"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddAllOptionsConfigurators<TDependencyOptions>(this IServiceCollection services,
            TDependencyOptions options)
            where TDependencyOptions : class, IExtensionBuilderDependencyOptions
        {
            services.AddSingleton(options);

            // 获取所有包含选项配置器的属性信息集合
            var configuratorProperties = typeof(TDependencyOptions).GetProperties().Where(p =>
            {
                return p.PropertyType.IsAssignableToBaseType(_optionsConfiguratorType)
                    && p.PropertyType.IsGenericType;
            });

            foreach (var propertyInfo in configuratorProperties)
            {
                var configurator = (IOptionsConfigurator)propertyInfo.GetValue(options, index: null);

                // 使用配置对象配置选项时，限定配置器名称不能为空
                if (configurator.Name.IsNotEmpty() && options.Configuration.IsNotNull())
                {
                    configurator.Configuration = options.Configuration.GetSection(configurator.Name);
                    services.ConfigureOptionsByConfiguration(configurator);
                }

                if (configurator is IOptionsActionConfigurator actionConfigurator)
                {
                    if (!actionConfigurator.AutoConfigureAction && !actionConfigurator.AutoPostConfigureAction)
                        continue;

                    var action = propertyInfo.PropertyType.GetProperty("Action").GetValue(actionConfigurator, index: null);
                    if (action.IsNull())
                        continue;

                    if (actionConfigurator.AutoConfigureAction)
                        services.ConfigureOptionsByAction(actionConfigurator, action);

                    // ConfigureOptions 与 PostConfigureOptions 二选一
                    if (!actionConfigurator.AutoConfigureAction && actionConfigurator.AutoPostConfigureAction)
                        services.PostConfigureOptionsByAction(actionConfigurator, action);
                }
            }

            return services;
        }


        private static readonly Type _optionsChangeTokenSourceServiceType
            = typeof(IOptionsChangeTokenSource<>);

        private static readonly Type _configureOptionsServiceType
            = typeof(IConfigureOptions<>);


        private static readonly Type _configurationChangeTokenSourceType
            = typeof(ConfigurationChangeTokenSource<>);

        private static readonly Type _namedConfigureFromConfigurationOptionsType
            = typeof(NamedConfigureFromConfigurationOptions<>);


        private static IServiceCollection ConfigureOptionsByConfiguration(this IServiceCollection services,
            IOptionsConfigurator configurator)
        {
            var sourceServiceType = _optionsChangeTokenSourceServiceType.MakeGenericType(configurator.OptionsType);
            var optionsServiceType = _configureOptionsServiceType.MakeGenericType(configurator.OptionsType);

            var sourceType = _configurationChangeTokenSourceType.MakeGenericType(configurator.OptionsType);
            var optionsType = _namedConfigureFromConfigurationOptionsType.MakeGenericType(configurator.OptionsType);

            var source = sourceType.EnsureCreateObject(Options.Options.DefaultName, configurator.Configuration);
            var options = optionsType.EnsureCreateObject(Options.Options.DefaultName, configurator.Configuration);

            services.AddSingleton(sourceServiceType, source);
            services.AddSingleton(optionsServiceType, options);

            return services;
        }


        private static readonly Type _configureNamedOptionsType
            = typeof(ConfigureNamedOptions<>);


        private static IServiceCollection ConfigureOptionsByAction(this IServiceCollection services,
            IOptionsActionConfigurator configurator, object action)
        {
            var serviceType = _configureOptionsServiceType.MakeGenericType(configurator.OptionsType);
            var optionsType = _configureNamedOptionsType.MakeGenericType(configurator.OptionsType);

            var options = optionsType.EnsureCreateObject(Options.Options.DefaultName, action);

            services.AddSingleton(serviceType, options);

            return services;
        }


        private static readonly Type _postConfigureOptionsServiceType
            = typeof(IPostConfigureOptions<>);

        private static readonly Type _postConfigureOptionsType
            = typeof(PostConfigureOptions<>);


        private static IServiceCollection PostConfigureOptionsByAction(this IServiceCollection services,
            IOptionsActionConfigurator configurator, object action)
        {
            var serviceType = _postConfigureOptionsServiceType.MakeGenericType(configurator.OptionsType);
            var postType = _postConfigureOptionsType.MakeGenericType(configurator.OptionsType);

            var post = postType.EnsureCreateObject(Options.Options.DefaultName, action);

            services.AddSingleton(serviceType, post);

            return services;
        }

    }
}
