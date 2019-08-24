#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using System;

//namespace Librame.Extensions.Core
//{
//    /// <summary>
//    /// 扩展构建器选项静态扩展。
//    /// </summary>
//    public static class ExtensionBuilderOptionsExtensions
//    {
//        /// <summary>
//        /// 配置构建器选项。
//        /// </summary>
//        /// <typeparam name="TOptions">指定的构建器选项类型。</typeparam>
//        /// <param name="extensionBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
//        /// <param name="configureOptions">给定的配置选项动作（可选；高优先级）。</param>
//        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
//        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
//        /// <returns>返回构建器选项实例。</returns>
//        public static TOptions Configure<TOptions>(this IExtensionBuilder extensionBuilder,
//            Action<TOptions> configureOptions = null,
//            IConfiguration configuration = null,
//            Action<BinderOptions> configureBinderOptions = null)
//            where TOptions : class, IExtensionBuilderOptions, new()
//        {
//            return extensionBuilder.Services.ConfigureBuilder(configureOptions,
//                configuration, configureBinderOptions);
//        }

//        /// <summary>
//        /// 配置构建器选项。
//        /// </summary>
//        /// <typeparam name="TOptions">指定的构建器选项类型。</typeparam>
//        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
//        /// <param name="configureOptions">给定的配置选项动作（可选；高优先级）。</param>
//        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
//        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
//        /// <returns>返回构建器选项实例。</returns>
//        public static TOptions ConfigureBuilder<TOptions>(this IServiceCollection services,
//            Action<TOptions> configureOptions = null,
//            IConfiguration configuration = null,
//            Action<BinderOptions> configureBinderOptions = null)
//            where TOptions : class, IExtensionBuilderOptions, new()
//        {
//            // 方式一
//            // 优点：可直接使用 Options进行其他功能配置
//            // 缺点：暂时仅可使用 IOptions 功能
//            // Configure Options: Default Priority
//            var options = new TOptions();

//            // Configure Options: Normal Priority
//            if (configuration.IsNotNull())
//                ConfigurationBinder.Bind(configuration, options, configureBinderOptions);

//            // Configure Options: High Priority
//            configureOptions?.Invoke(options);

//            services.AddOptions();
//            services.AddSingleton(Options.Create(options));

//            // 方式二
//            // 优点：可完整使用 Options 功能（诸如 IConfigureOptions、IPostConfigreOptions 等）
//            // 缺点：暂时无法直接使用 Options进行其他功能配置（services.BuildServiceProvider() 暂不考虑）
//            //var builder = services.AddOptions<TOptions>();

//            //if (configureOptions.IsNotNull())
//            //    builder.Configure(configureOptions);

//            return options;
//        }

//    }
//}
