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
using System.Diagnostics.CodeAnalysis;
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
        /// 配置扩展构建器依赖根（默认支持从 JSON 文件加载初始配置）。
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
        ///         ......
        ///         
        ///         "Name": "CoreBuilderDependency",
        ///         "Type": { "Value": "Librame.Extensions.Core.CoreBuilderDependency, Librame.Extensions.Core" }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="TDependency">指定的扩展构建器依赖入口类型。</typeparam>
        /// <param name="configureDependency">给定的配置依赖动作方法（可空）。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="rootConfigFileName">给定的根配置文件名（可选；默认为“appsettings.json”）。</param>
        /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "services")]
        public static TDependency ConfigureDependency<TDependency>(this Action<TDependency> configureDependency,
            IServiceCollection services, string rootConfigFileName = "appsettings.json")
            where TDependency : class, IExtensionBuilderDependency
        {
            services.NotNull(nameof(services));

            var dependency = typeof(TDependency).EnsureCreate<TDependency>();

            // configureDependency: dependency => dependency.Configuration = ...;
            configureDependency?.Invoke(dependency);

            // 如果依赖配置根不存在，则尝试从配置文件加载
            if (dependency.ConfigurationRoot.IsNull() && rootConfigFileName.IsNotEmpty())
                UseDefaultConfigurationRoot();

            // 如果配置节不存在，则默认尝试从配置根中获取
            if (dependency.Configuration.IsNull())
                dependency.Configuration = dependency.ConfigurationRoot?.GetSection(dependency.Name);

            // Bind Configuration
            if (dependency.Configuration.IsNotNull())
                dependency.Configuration.Bind(dependency);

            return dependency.RegisterDependency(services);

            // UseDefaultConfigurationRoot
            void UseDefaultConfigurationRoot()
            {
                var filePath = rootConfigFileName.AsFilePathCombiner(dependency.BaseDirectory);
                if (filePath.Exists())
                {
                    var root = new ConfigurationBuilder()
                        .AddJsonFile(filePath) // default(optional: false, reloadOnChange: false)
                        .Build();
                    dependency.ConfigurationRoot = root;
                }
            }
        }

        /// <summary>
        /// 配置扩展构建器依赖（默认支持从基础扩展构建器的配置根加载初始配置）。
        /// </summary>
        /// <example>
        /// appsettings.json 根配置结构参考：
        /// <code>
        /// {
        ///     "XXXBuilderDependency":
        ///     {
        ///         "BaseDirectory": "/DirectoryPath",
        ///         "ConfigDirectory": "/DirectoryPath",
        ///         "ExportDirectory": "/DirectoryPath",
        ///         
        ///         "OptionsType": { "Value": "Librame.Extensions.XXX.Builders.XXXBuilderOptions, Librame.Extensions.XXX" }
        ///         "Options": {
        ///             //...
        ///         },
        ///         
        ///         "Name": "XXXBuilderDependency",
        ///         "Type": { "Value": "Librame.Extensions.XXX.XXXBuilderDependency, Librame.Extensions.XXX" }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="TDependency">指定的扩展构建器依赖类型。</typeparam>
        /// <param name="configureDependency">给定的配置依赖动作方法（可空）。</param>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static TDependency ConfigureDependency<TDependency>(this Action<TDependency> configureDependency,
            IExtensionBuilder baseBuilder)
            where TDependency : class, IExtensionBuilderDependency
        {
            baseBuilder.NotNull(nameof(baseBuilder));

            var dependency = typeof(TDependency).EnsureCreate<TDependency>(baseBuilder.Dependency);
            dependency.ConfigurationRoot = baseBuilder.Dependency.ConfigurationRoot;
            dependency.Configuration = dependency.ConfigurationRoot?.GetSection(dependency.Name);

            // Bind Configuration
            if (dependency.Configuration.IsNotNull())
                dependency.Configuration.Bind(dependency);

            // configureDependency: dependency => dependency.Configuration = ...;
            configureDependency?.Invoke(dependency);

            return dependency.RegisterDependency(baseBuilder.Services);
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
            AddConsistencyOptions();

            // 获取所有依赖属性集合
            var properties = typeof(TDependency).GetProperties().Where(p =>
            {
                return p.PropertyType.IsAssignableToBaseType(OptionsDependencyTypes.DependencyType)
                    && p.PropertyType.IsGenericType;
            });

            foreach (var property in properties)
            {
                var propertyDependency = (IDependency)property.GetValue(dependency, index: null);

                if (propertyDependency.Configuration.IsNull())
                    propertyDependency.Configuration = dependency.Configuration?.GetSection(property.Name);

                if (propertyDependency is IOptionsDependency propertyOptionsDependency)
                {
                    RegisterPropertyOptionsDependency<TDependency>(services, property,
                        propertyOptionsDependency, optionsPropertyName);
                }
            }

            return dependency;

            // AddConsistencyOptions
            void AddConsistencyOptions()
            {
                var optionsProperty = typeof(TDependency).GetProperty(optionsPropertyName);
                var options = optionsProperty.GetValue(dependency, index: null);

                ConsistencyOptionsPool.AddOrUpdate(optionsProperty.PropertyType, options);
            }
        }

        private static void RegisterPropertyOptionsDependency<TDependency>(IServiceCollection services,
            PropertyInfo property, IOptionsDependency propertyOptionsDependency, string optionsPropertyName)
            where TDependency : class, IExtensionBuilderDependency
        {
            // 利用选项实例的引用唯一性，注册一致性依赖属性选项实例以便 ConsistencyOptionsFactory 调用
            // 如：CoreBuilderDependency.Localization 的 LocalizationOptions 选项
            var propertyOptionsType = propertyOptionsDependency.OptionsType.Source;
            AddPropertyConsistencyOptions();

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

            // AddPropertyConsistencyOptions
            void AddPropertyConsistencyOptions()
            {
                var propertyOptions = property.PropertyType.GetProperty(optionsPropertyName)
                    .GetValue(propertyOptionsDependency, index: null);

                ConsistencyOptionsPool.AddOrUpdate(propertyOptionsType, propertyOptions);
            }
        }

        private static IServiceCollection RegisterPropertyOptionsConfiguration(IServiceCollection services,
            IConfiguration propertyOptionsConfiguration, Type propertyOptionsType)
        {
            // services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            // services.AddSingleton<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));

            var baseSourceType = OptionsDependencyTypes.OptionsChangeTokenSourceTypeDefinition.MakeGenericType(propertyOptionsType);
            var baseOptionsType = OptionsDependencyTypes.ConfigureOptionsTypeDefinition.MakeGenericType(propertyOptionsType);

            var configSourceType = OptionsDependencyTypes.ConfigurationChangeTokenSourceTypeDefinition.MakeGenericType(propertyOptionsType);
            var configOptionsType = OptionsDependencyTypes.NamedConfigureFromConfigurationOptionsTypeDefinition.MakeGenericType(propertyOptionsType);

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
