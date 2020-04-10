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
        private const string ConnectionStringsSectionName = "ConnectionStrings";
        private const string DefaultTenantSectionName = nameof(DataBuilderDependency.Options.DefaultTenant);


        /// <summary>
        /// 构造一个 <see cref="DataBuilderDependency"/>。
        /// </summary>
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        public DataBuilderDependency(IExtensionBuilderDependency parentDependency = null)
            : base(nameof(DataBuilderDependency), parentDependency)
        {
        }


        /// <summary>
        /// 绑定配置根包含的连接字符串集合配置节。
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
        /// <exception cref="ArgumentNullException">
        /// this.ConfigurationRoot is null.
        /// </exception>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        public DataBuilderDependency BindConnectionStrings(Func<string, string> validateFactory = null)
        {
            ConfigurationRoot.NotNull(nameof(ConfigurationRoot));
            return BindConnectionStrings(ConfigurationRoot.GetSection(ConnectionStringsSectionName), validateFactory);
        }

        /// <summary>
        /// 绑定连接字符串集合配置节。
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
        /// <exception cref="ArgumentNullException">
        /// configuration is null.
        /// </exception>
        /// <param name="configuration">给定的连接字符串集合 <see cref="IConfiguration"/>。</param>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "configuration")]
        public DataBuilderDependency BindConnectionStrings(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            configuration.NotNull(nameof(configuration));

            UpdateDefaultConnection(configuration, validateFactory);
            UpdateWritingConnection(configuration, validateFactory);

            return this;
        }


        /// <summary>
        /// 绑定默认租户配置节。
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
        /// <exception cref="ArgumentNullException">
        /// this.ConfigurationRoot is null.
        /// </exception>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        public DataBuilderDependency BindDefaultTenant(Func<string, string> validateFactory = null)
        {
            ConfigurationRoot.NotNull(nameof(ConfigurationRoot));
            return BindDefaultTenant(ConfigurationRoot.GetSection(DefaultTenantSectionName), validateFactory);
        }

        /// <summary>
        /// 绑定默认租户配置节。
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
        /// <exception cref="ArgumentNullException">
        /// configuration is null.
        /// </exception>
        /// <param name="configuration">给定的默认租户 <see cref="IConfiguration"/>。</param>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "configurationSection")]
        public DataBuilderDependency BindDefaultTenant(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            configuration.NotNull(nameof(configuration));

            UpdateDefaultConnection(configuration, validateFactory);
            UpdateWritingConnection(configuration, validateFactory);

            var writingSeparation = configuration
                .GetSection(nameof(Options.DefaultTenant.WritingSeparation))?.Value;
            if (writingSeparation.IsNotEmpty())
                Options.DefaultTenant.WritingSeparation = bool.Parse(writingSeparation);

            var name = configuration
                .GetSection(nameof(Options.DefaultTenant.Name))?.Value;
            if (name.IsNotEmpty())
                Options.DefaultTenant.Name = name;

            var host = configuration
                .GetSection(nameof(Options.DefaultTenant.Host))?.Value;
            if (host.IsNotEmpty())
                Options.DefaultTenant.Host = host;

            return this;
        }


        private void UpdateDefaultConnection(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = configuration
                .GetSection(nameof(Options.DefaultTenant.DefaultConnectionString))?.Value;

            if (connectionString.IsEmpty())
                return;

            Options.DefaultTenant.DefaultConnectionString = validateFactory?.Invoke(connectionString)
                .NotEmptyOrDefault(connectionString);
        }

        private void UpdateWritingConnection(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = configuration
                .GetSection(nameof(Options.DefaultTenant.WritingConnectionString))?.Value;

            if (connectionString.IsEmpty())
                return;

            Options.DefaultTenant.WritingConnectionString = validateFactory?.Invoke(connectionString)
                .NotEmptyOrDefault(connectionString);
        }

    }
}
