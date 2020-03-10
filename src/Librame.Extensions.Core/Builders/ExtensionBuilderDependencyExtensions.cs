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
using BaseOptions = Microsoft.Extensions.Options.Options;

namespace Librame.Extensions.Core.Builders
{
    using Combiners;
    using Dependencies;
    using Options;

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
        /// appsettings.json 根配置结构参考：
        /// <code>
        /// {
        ///     "CoreBuilderDependency":
        ///     {
        ///         "BaseDirectory": "/DirectoryPath",
        ///         "ConfigDirectory": "/DirectoryPath",
        ///         "ExportDirectory": "/DirectoryPath",
        ///         
        ///         "OptionsType": { "Value": "Librame.Extensions.Core.Builders.CoreBuilderOptions, Librame.Extensions.Core" }
        ///         "Options": {
        ///             "Encoding": { "Value": "utf-8" },
        ///             "ClockRefluxOffset": 1,
        ///             "IsUtcClock": false,
        ///             "ThreadsCount": 12
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
        ///         },
        ///         
        ///         "Name": "CoreBuilderDependency",
        ///         "Type": { "Value": "Librame.Extensions.Core.CoreBuilderDependency, Librame.Extensions.Core" }
        ///     }
        /// }
        /// </code>
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

            // 利用选项实例的引用唯一性，注册一致性依赖选项实例以便 ConsistencyOptionsFactory 调用
            // 如：CoreBuilderDependency 的 CoreBuilderOptions 选项
            var optionsPropertyName = nameof(OptionsDependency<TDependency>.Options);
            var optionsProperty = typeof(TDependency).GetProperty(optionsPropertyName);
            var options = optionsProperty.GetValue(dependency, index: null);
            ConsistencyOptionsPool.AddOrUpdate(optionsProperty.PropertyType, options);

            // 获取所有依赖属性集合
            var properties = typeof(TDependency).GetProperties().Where(p =>
            {
                return p.PropertyType.IsAssignableToBaseType(OptionsDependencyTypes.BaseDependencyType)
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
                        propertyOptionsDependency, optionsPropertyName);
                }
            }

            return dependency;
        }

        private static void RegisterPropertyOptionsDependency<TDependency>(IServiceCollection services,
            PropertyInfo property, IOptionsDependency propertyOptionsDependency, string optionsPropertyName)
            where TDependency : class, IExtensionBuilderDependency
        {
            // 利用选项实例的引用唯一性，注册一致性依赖属性选项实例以便 ConsistencyOptionsFactory 调用
            // 如：CoreBuilderDependency.Localization 的 LocalizationOptions 选项
            var propertyOptionsType = propertyOptionsDependency.OptionsType.Source;
            var propertyOptions = property.PropertyType.GetProperty(optionsPropertyName)
                .GetValue(propertyOptionsDependency, index: null);
            ConsistencyOptionsPool.AddOrUpdate(propertyOptionsType, propertyOptions);

            // 如果需要注册依赖配置对象的选项配置节点
            //var propertyOptionsConfigurationRegistered = false;
            if (propertyOptionsDependency.Configuration.IsNotNull())
            {
                var propertyOptionsConfiguration = propertyOptionsDependency.Configuration.GetSection(optionsPropertyName);
                RegisterPropertyOptionsConfiguration(services, propertyOptionsConfiguration, propertyOptionsType);

                //propertyOptionsConfigurationRegistered = true;
            }

            //if (!propertyOptionsDependency.AutoConfigureOptions && !propertyOptionsDependency.AutoPostConfigureOptions)
            //    return;

            //var propertyConfigureOptionsName = nameof(OptionsDependency<TDependency>.ConfigureOptions);
            //var propertyConfigureOptionsAction = property.PropertyType.GetProperty(propertyConfigureOptionsName)?
            //    .GetValue(propertyOptionsDependency, index: null);

            //if (propertyConfigureOptionsAction.IsNull())
            //    return;

            //if (propertyOptionsDependency.AutoConfigureOptions && !propertyOptionsConfigurationRegistered)
            //    RegisterPropertyConfigureOptions(services, propertyOptionsType, propertyConfigureOptionsAction);

            //// ConfigureOptions 与 PostConfigureOptions 二选一
            ////if (!propertyOptionsDependency.AutoConfigureOptions && propertyOptionsDependency.AutoPostConfigureOptions)
            //if (propertyOptionsDependency.AutoPostConfigureOptions)
            //    RegisterPropertyPostConfigureOptions(services, propertyOptionsType, propertyConfigureOptionsAction);
        }

        private static IServiceCollection RegisterPropertyOptionsConfiguration(IServiceCollection services,
            IConfiguration propertyOptionsConfiguration, Type propertyOptionsType)
        {
            // services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            // services.AddSingleton<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));

            var baseSourceType = OptionsDependencyTypes.BaseOptionsChangeTokenSourceType.MakeGenericType(propertyOptionsType);
            var baseOptionsType = OptionsDependencyTypes.BaseConfigureOptionsType.MakeGenericType(propertyOptionsType);

            var configSourceType = OptionsDependencyTypes.ConfigurationChangeTokenSourceType.MakeGenericType(propertyOptionsType);
            var configOptionsType = OptionsDependencyTypes.NamedConfigureFromConfigurationOptionsType.MakeGenericType(propertyOptionsType);

            var configSource = configSourceType.EnsureCreateObject(BaseOptions.DefaultName, propertyOptionsConfiguration);
            var configOptions = configOptionsType.EnsureCreateObject(BaseOptions.DefaultName, propertyOptionsConfiguration);

            services.AddSingleton(baseSourceType, configSource);
            services.AddSingleton(baseOptionsType, configOptions);

            return services;
        }

        //private static IServiceCollection RegisterPropertyConfigureOptions(IServiceCollection services,
        //    Type propertyOptionsType, object configureOptionsAction)
        //{
        //    // services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name, configureOptions));

        //    var configureOptionsType = OptionsDependencyTypes.BaseConfigureOptionsType.MakeGenericType(propertyOptionsType);
        //    var configureNamedOptions = OptionsDependencyTypes.ConfigureNamedOptionsType.MakeGenericType(propertyOptionsType)
        //        .EnsureCreateObject(BaseOptions.DefaultName, configureOptionsAction);

        //    services.AddSingleton(configureOptionsType, configureNamedOptions);

        //    return services;
        //}

        //private static IServiceCollection RegisterPropertyPostConfigureOptions(IServiceCollection services,
        //    Type propertyOptionsType, object configureOptionsAction)
        //{
        //    // services.AddSingleton<IPostConfigureOptions<TOptions>>(new PostConfigureOptions<TOptions>(name, configureOptions));

        //    var postConfigureOptionsType = OptionsDependencyTypes.BasePostConfigureOptionsType.MakeGenericType(propertyOptionsType);
        //    var postConfigureOptions = OptionsDependencyTypes.PostConfigureOptionsType.MakeGenericType(propertyOptionsType)
        //        .EnsureCreateObject(BaseOptions.DefaultName, configureOptionsAction);

        //    services.AddSingleton(postConfigureOptionsType, postConfigureOptions);

        //    return services;
        //}

        #endregion

    }
}
