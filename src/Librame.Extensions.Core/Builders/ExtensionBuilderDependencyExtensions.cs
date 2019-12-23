#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using OptionsHelper = Microsoft.Extensions.Options.Options;

namespace Librame.Extensions.Core.Builders
{
    using Combiners;
    using Dependencies;

    /// <summary>
    /// 扩展构建器依赖静态扩展。
    /// </summary>
    public static class ExtensionBuilderDependencyExtensions
    {

        #region ConfigureDependency

        /// <summary>
        /// 配置扩展构建器依赖（支持从文件加载初始配置）。
        /// </summary>
        /// <example>
        /// ex. "appsettings.json" // see CoreBuilderDependencyTests.AllTest()
        /// {
        ///     "CoreBuilderDependency":
        ///     {
        ///         "Name": "CoreBuilderDependency",
        ///         "Type": { "Value": "Librame.Extensions.Core.CoreBuilderDependency, Librame.Extensions.Core" }
        ///         "BaseDirectory": "/DirectoryPath",
        ///         "ConfigDirectory": "/DirectoryPath",
        ///         "ExportDirectory": "/DirectoryPath",
        ///         
        ///         "Builder":
        ///         {
        ///             "Name": "CoreBuilderOptions",
        ///             "Type": { "Value": "Librame.Extensions.Core.Dependencies.OptionsDependency`1[Librame.Extensions.Core.Builders.CoreBuilderOptions], Librame.Extensions.Core.Abstractions" }
        ///             "AutoConfigureAction": true,
        ///             "AutoPostConfigureAction": false,
        ///             "OptionsType": { "Value": "Librame.Extensions.Core.Builders.CoreBuilderOptions, Librame.Extensions.Core" }
        ///             "Options": {
        ///                 "Encoding": { "Value": "utf-8" },
        ///                 "ClockRefluxOffset": 1,
        ///                 "IsUtcClock": false,
        ///                 "ThreadsCount": 12
        ///             }
        ///         },
        ///         
        ///         "Localization":
        ///         {
        ///             // LocalizationOptions
        ///             ......
        ///         },
        ///         
        ///         "MemoryCache":
        ///         {
        ///             // MemoryCacheOptions
        ///             ......
        ///         },
        ///         
        ///         "MemoryDistributedCache":
        ///         {
        ///             // MemoryDistributedCacheOptions
        ///             ......
        ///         }
        ///     }
        /// }
        /// </example>
        /// <typeparam name="TDependencyRoot">指定的扩展构建器依赖根类型。</typeparam>
        /// <param name="configureDependency">给定的配置依赖动作方法（可空）。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <typeparamref name="TDependencyRoot"/>。</returns>
        public static TDependencyRoot ConfigureDependencyRoot<TDependencyRoot>(this Action<TDependencyRoot> configureDependency,
            IServiceCollection services)
            where TDependencyRoot : class, IExtensionBuilderDependency, IDependencyRoot
        {
            var dependency = configureDependency.ConfigureDependency(baseBuilder: null);
            return dependency.RegisterDependency(services);
        }

        /// <summary>
        /// 配置依赖选项（支持从文件加载初始配置）。
        /// </summary>
        /// <typeparam name="TDependency">指定的选项依赖类型。</typeparam>
        /// <param name="configureDependency">给定的配置动作（可空）。</param>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>（可空）。</param>
        /// <param name="initialDependency">给定的初始 <typeparamref name="TDependency"/>（可选；默认使用类型构造）。</param>
        /// <param name="rootConfigFileName">给定的根配置文件名（可选；默认为“appsettings.json”）。</param>
        /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
        public static TDependency ConfigureDependency<TDependency>(this Action<TDependency> configureDependency,
            IExtensionBuilder baseBuilder, TDependency initialDependency = null, string rootConfigFileName = "appsettings.json")
            where TDependency : class, IExtensionBuilderDependency
        {
            if (initialDependency == null)
                initialDependency = typeof(TDependency).EnsureCreate<TDependency>();

            // configureDependency: dependency => dependency.Configuration = ...;
            configureDependency?.Invoke(initialDependency);

            initialDependency.BindConfiguration(rootConfigFileName);

            if (baseBuilder.IsNotNull())
                return initialDependency.RegisterDependency(baseBuilder.Services);

            return initialDependency;
        }

        private static void BindConfiguration<TDependency>(this TDependency dependency, string rootConfigFileName)
            where TDependency : class, IExtensionBuilderDependency
        {
            if (dependency is IDependencyRoot dependencyRoot)
            {
                if (dependencyRoot.ConfigurationRoot.IsNull())
                {
                    // 默认从根配置文件中读取配置
                    var filePath = rootConfigFileName.AsFilePathCombiner(dependency.ConfigDirectory);
                    if (filePath.Exists())
                    {
                        var root = new ConfigurationBuilder()
                            .AddJsonFile(filePath) // default(optional: false, reloadOnChange: false)
                            .Build();
                        dependencyRoot.ConfigurationRoot = root;
                    }
                }

                if (dependency.Configuration.IsNull() && dependencyRoot.ConfigurationRoot.IsNotNull())
                    dependency.Configuration = dependencyRoot.ConfigurationRoot.GetSection(dependency.Name);
            }
        }

        #endregion


        #region RegisterDependency

        private static TDependency RegisterDependency<TDependency>(this TDependency dependency, IServiceCollection services)
            where TDependency : class, IExtensionBuilderDependency
        {
            services.AddSingleton(dependency);

            // 获取所有依赖属性集合
            var properties = typeof(TDependency).GetProperties().Where(p =>
            {
                return p.PropertyType.IsAssignableToBaseType(OptionsTypeReferences.BaseDependencyType)
                    && p.PropertyType.IsGenericType;
            });

            foreach (var property in properties)
            {
                var propertyDependency = (IDependency)property.GetValue(dependency, index: null);

                if (dependency.Configuration.IsNotNull() && propertyDependency.Configuration.IsNull())
                {
                    // 绑定可能存在的配置
                    propertyDependency.Configuration = dependency.Configuration.GetSection(property.Name);
                }

                if (propertyDependency is IOptionsDependency propertyOptionsDependency)
                {
                    RegisterPropertyOptionsDependency<TDependency>(services, property,
                        propertyOptionsDependency, dependency);
                }
            }

            return dependency;
        }

        private static void RegisterPropertyOptionsDependency<TDependency>(IServiceCollection services,
            PropertyInfo property, IOptionsDependency propertyOptionsDependency, TDependency builderDependency)
            where TDependency : class, IExtensionBuilderDependency
        {
            var propertyOptionsType = propertyOptionsDependency.OptionsType.Source;

            var propertyOptionsConfigurationRegistered = false;
            var propertyOptionsName = nameof(OptionsDependency<TDependency>.Options);

            if (propertyOptionsDependency.Configuration.IsNotNull())
            {
                var propertyOptionsConfiguration = propertyOptionsDependency.Configuration.GetSection(propertyOptionsName);
                RegisterPropertyOptionsConfiguration(services, propertyOptionsConfiguration, propertyOptionsType);

                propertyOptionsConfigurationRegistered = true;
            }

            // 注册选项实例以供 OptionsFactory 调用
            var propertyOptions = property.PropertyType.GetProperty(propertyOptionsName)?
                .GetValue(propertyOptionsDependency, index: null);
            OptionsDependencyTable.AddOrUpdate(propertyOptionsType, propertyOptions, builderDependency);

            if (!propertyOptionsDependency.AutoConfigureOptions && !propertyOptionsDependency.AutoPostConfigureOptions)
                return;

            var propertyConfigureOptionsName = nameof(OptionsDependency<TDependency>.ConfigureOptions);
            var propertyConfigureOptions = property.PropertyType.GetProperty(propertyConfigureOptionsName)?
                .GetValue(propertyOptionsDependency, index: null);

            if (propertyConfigureOptions.IsNull())
                return;

            if (propertyOptionsDependency.AutoConfigureOptions && !propertyOptionsConfigurationRegistered)
                RegisterPropertyConfigureOptions(services, propertyOptionsType, propertyConfigureOptions);

            // ConfigureOptions 与 PostConfigureOptions 二选一
            //if (!propertyOptionsDependency.AutoConfigureOptions && propertyOptionsDependency.AutoPostConfigureOptions)
            if (propertyOptionsDependency.AutoPostConfigureOptions)
                RegisterPropertyPostConfigureOptions(services, propertyOptionsType, propertyConfigureOptions);
        }

        private static IServiceCollection RegisterPropertyOptionsConfiguration(IServiceCollection services,
            IConfiguration propertyOptionsConfiguration, Type propertyOptionsType)
        {
            // services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            // services.AddSingleton<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));

            var baseSourceType = OptionsTypeReferences.BaseOptionsChangeTokenSourceType.MakeGenericType(propertyOptionsType);
            var baseOptionsType = OptionsTypeReferences.BaseConfigureOptionsType.MakeGenericType(propertyOptionsType);

            var configSourceType = OptionsTypeReferences.ConfigurationChangeTokenSourceType.MakeGenericType(propertyOptionsType);
            var configOptionsType = OptionsTypeReferences.NamedConfigureFromConfigurationOptionsType.MakeGenericType(propertyOptionsType);

            var configSource = configSourceType.EnsureCreateObject(OptionsHelper.DefaultName, propertyOptionsConfiguration);
            var configOptions = configOptionsType.EnsureCreateObject(OptionsHelper.DefaultName, propertyOptionsConfiguration);

            services.AddSingleton(baseSourceType, configSource);
            services.AddSingleton(baseOptionsType, configOptions);

            return services;
        }

        private static IServiceCollection RegisterPropertyConfigureOptions(IServiceCollection services,
            Type propertyOptionsType, object configureOptions)
        {
            // services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name, configureOptions));

            var configureOptionsType = OptionsTypeReferences.BaseConfigureOptionsType.MakeGenericType(propertyOptionsType);
            var configureNamedOptions = OptionsTypeReferences.ConfigureNamedOptionsType.MakeGenericType(propertyOptionsType)
                .EnsureCreateObject(OptionsHelper.DefaultName, configureOptions);

            services.AddSingleton(configureOptionsType, configureNamedOptions);

            return services;
        }

        private static IServiceCollection RegisterPropertyPostConfigureOptions(IServiceCollection services,
            Type propertyOptionsType, object configureOptions)
        {
            // services.AddSingleton<IPostConfigureOptions<TOptions>>(new PostConfigureOptions<TOptions>(name, configureOptions));

            var postConfigureOptionsType = OptionsTypeReferences.BasePostConfigureOptionsType.MakeGenericType(propertyOptionsType);
            var postConfigureOptions = OptionsTypeReferences.PostConfigureOptionsType.MakeGenericType(propertyOptionsType)
                .EnsureCreateObject(OptionsHelper.DefaultName, configureOptions);

            services.AddSingleton(postConfigureOptionsType, postConfigureOptions);

            return services;
        }

        #endregion

    }
}
