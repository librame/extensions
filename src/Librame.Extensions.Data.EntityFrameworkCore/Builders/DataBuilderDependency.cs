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
        private const string ConnectionStringsSectionName
            = "ConnectionStrings";

        private const string DefaultTenantSectionName
            = nameof(DataBuilderOptions.DefaultTenant);


        /// <summary>
        /// 构造一个 <see cref="DataBuilderDependency"/>。
        /// </summary>
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        public DataBuilderDependency(IExtensionBuilderDependency parentDependency = null)
            : base(nameof(DataBuilderDependency), parentDependency)
        {
            MigrationCommandsDirectory = ConfigDirectory.CombinePath("migration_commands");
            ModelSnapshotsDirectory = ConfigDirectory.CombinePath("model_snapshots");
        }


        /// <summary>
        /// 迁移命令目录。
        /// </summary>
        public string MigrationCommandsDirectory { get; set; }

        /// <summary>
        /// 模型快照目录。
        /// </summary>
        public string ModelSnapshotsDirectory { get; set; }


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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public DataBuilderDependency BindConnectionStrings(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            configuration.NotNull(nameof(configuration));

            UpdateDefaultConnectionString(configuration, validateFactory);
            UpdateWritingConnectionString(configuration, validateFactory);

            return this;
        }


        /// <summary>
        /// 绑定配置根包含的默认租户配置节。。
        /// </summary>
        /// <examples>
        /// JSON 配置节点结构参考：
        /// <code>
        /// {
        ///     "DefaultTenant": {
        ///         "Name": "name",
        ///         "Host": "host",
        ///         "DefaultConnectionString": "default connection string",
        ///         "WritingConnectionString": "writing connection string",
        ///         "WritingSeparation": true // or false
        ///     }
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public DataBuilderDependency BindDefaultTenant(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            configuration.NotNull(nameof(configuration));

            UpdateDefaultConnectionString(configuration, validateFactory);
            UpdateWritingConnectionString(configuration, validateFactory);

            Options.DefaultTenant.WritingSeparation
                = configuration.GetValue(nameof(Options.DefaultTenant.WritingSeparation), defaultValue: false);

            Options.DefaultTenant.Name
                = configuration.GetValue(nameof(Options.DefaultTenant.Name), Options.DefaultTenant.Name);

            Options.DefaultTenant.Host
                = configuration.GetValue(nameof(Options.DefaultTenant.Host), Options.DefaultTenant.Host);

            return this;
        }


        private void UpdateDefaultConnectionString(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = configuration.GetValue(nameof(Options.DefaultTenant.DefaultConnectionString),
                Options.DefaultTenant.DefaultConnectionString);

            Options.DefaultTenant.DefaultConnectionString = validateFactory?.Invoke(connectionString) ?? connectionString;
        }

        private void UpdateWritingConnectionString(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = configuration.GetValue(nameof(Options.DefaultTenant.WritingConnectionString),
                Options.DefaultTenant.WritingConnectionString);

            Options.DefaultTenant.WritingConnectionString = validateFactory?.Invoke(connectionString) ?? connectionString;
        }

    }
}
