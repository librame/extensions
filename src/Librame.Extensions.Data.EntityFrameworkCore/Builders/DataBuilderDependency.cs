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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;

    /// <summary>
    /// 数据构建器依赖选项。
    /// </summary>
    public class DataBuilderDependency : AbstractExtensionBuilderDependency<DataBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderDependency"/>。
        /// </summary>
        public DataBuilderDependency()
            : base(nameof(DataBuilderDependency))
        {
        }


        /// <summary>
        /// 绑定配置根包含的连接字符串集合配置节点。
        /// </summary>
        /// <examples>
        /// JSON 根配置结构参考：
        /// <code>
        /// {
        ///     "ConnectionStrings": {
        ///         "DefaultConnectionString": "default connection string",
        ///         "WritingConnectionString": "writing connection string"
        ///     }
        /// }
        /// </code>
        /// </examples>
        /// <param name="configurationRoot">给定的 <see cref="IConfigurationRoot"/>。</param>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "connectionStringsConfiguration")]
        public DataBuilderDependency BindConnectionStrings(IConfigurationRoot configurationRoot,
            Func<string, string> validateFactory = null)
        {
            configurationRoot.NotNull(nameof(configurationRoot));

            return BindConnectionStrings(configurationRoot.GetSection("ConnectionStrings"), validateFactory);
        }

        /// <summary>
        /// 绑定连接字符串集合配置节点。
        /// </summary>
        /// <examples>
        /// JSON 配置节点结构参考：
        /// <code>
        /// "ConnectionStrings": {
        ///     "DefaultConnectionString": "default connection string",
        ///     "WritingConnectionString": "writing connection string"
        /// }
        /// </code>
        /// </examples>
        /// <param name="configurationSection">给定的连接字符串集合 <see cref="IConfigurationSection"/>。</param>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "connectionStringsConfiguration")]
        public DataBuilderDependency BindConnectionStrings(IConfigurationSection configurationSection,
            Func<string, string> validateFactory = null)
        {
            configurationSection.NotNull(nameof(configurationSection));

            UpdateDefaultConnectionString(configurationSection, validateFactory);
            UpdateWritingConnectionString(configurationSection, validateFactory);

            return this;
        }


        /// <summary>
        /// 绑定默认租户配置对象。
        /// </summary>
        /// <examples>
        /// JSON 配置节点结构参考：
        /// <code>
        /// "DefaultTenant": {
        ///     "Name": "name",
        ///     "Host": "host",
        ///     "DefaultConnectionString": "default connection string",
        ///     "WritingConnectionString": "writing connection string",
        ///     "WritingSeparation": true // or false
        /// }
        /// </code>
        /// </examples>
        /// <param name="configurationSection">给定的默认租户 <see cref="IConfigurationSection"/>。</param>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "defaultTenantConfiguration")]
        public DataBuilderDependency BindDefaultTenant(IConfigurationSection configurationSection,
            Func<string, string> validateFactory = null)
        {
            configurationSection.NotNull(nameof(configurationSection));

            UpdateDefaultConnectionString(configurationSection, validateFactory);
            UpdateWritingConnectionString(configurationSection, validateFactory);

            var writingSeparation = configurationSection
                .GetSection(nameof(Options.DefaultTenant.WritingSeparation))?.Value;
            if (writingSeparation.IsNotEmpty())
                Options.DefaultTenant.WritingSeparation = bool.Parse(writingSeparation);

            var name = configurationSection
                .GetSection(nameof(Options.DefaultTenant.Name))?.Value;
            if (name.IsNotEmpty())
                Options.DefaultTenant.Name = name;

            var host = configurationSection
                .GetSection(nameof(Options.DefaultTenant.Host))?.Value;
            if (host.IsNotEmpty())
                Options.DefaultTenant.Host = host;

            return this;
        }


        private void UpdateDefaultConnectionString(IConfiguration connectionStringsConfiguration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = connectionStringsConfiguration
                .GetSection(nameof(Options.DefaultTenant.DefaultConnectionString))?.Value;

            if (connectionString.IsEmpty())
                return;

            Options.DefaultTenant.DefaultConnectionString = validateFactory?.Invoke(connectionString)
                .NotEmptyOrDefault(connectionString);
        }

        private void UpdateWritingConnectionString(IConfiguration connectionStringsConfiguration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = connectionStringsConfiguration
                .GetSection(nameof(Options.DefaultTenant.WritingConnectionString))?.Value;

            if (connectionString.IsEmpty())
                return;

            Options.DefaultTenant.WritingConnectionString = validateFactory?.Invoke(connectionString)
                .NotEmptyOrDefault(connectionString);
        }

    }
}
