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
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展构建器依赖选项静态扩展。
    /// </summary>
    public static class ExtensionBuilderDependencyOptionsExtensions
    {
        /// <summary>
        /// 配置依赖选项（支持从文件加载初始配置）。
        /// </summary>
        /// <example>
        /// ex. "appsettings.json"
        /// {
        ///     "CoreBuilderDependency": // CoreBuilderDependencyOptions
        ///     {
        ///         "CoreBuilder": // CoreBuilderDependencyOptions.Builder [OptionsConfigurator{TBuilderOptions}]
        ///         {
        ///             "IsUtcClock": false
        ///         },
        ///         "Localization": // CoreBuilderDependencyOptions.Localization [OptionsConfigurator{LocalizationOptions}]
        ///         {
        ///             "ResourcesPath": "Resources"
        ///         },...
        ///     }
        /// }
        /// </example>
        /// <typeparam name="TDependencyOptions">指定的依赖选项类型。</typeparam>
        /// <param name="configureAction">给定的配置动作（可选）。</param>
        /// <returns>返回 <typeparamref name="TDependencyOptions"/>。</returns>
        public static TDependencyOptions ConfigureDependency<TDependencyOptions>(this Action<TDependencyOptions> configureAction)
            where TDependencyOptions : class, IExtensionBuilderDependencyOptions
        {
            var options = typeof(TDependencyOptions).EnsureCreate<TDependencyOptions>();

            // configureAction = dependency.Configuration = ...;
            configureAction?.Invoke(options);

            if (options.Configuration.IsNull())
            {
                var filePath = "appsettings.json".AsFilePathCombiner(options.BaseDirectory);
                if (filePath.Exists() && options.Name.IsNotEmpty())
                {
                    var root = new ConfigurationBuilder()
                        .AddJsonFile(filePath) // default(optional: false, reloadOnChange: false)
                        .Build();
                    options.Configuration = root.GetSection(options.Name);
                }
            }

            return options;
        }

    }
}
