#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
            CompilersDirectory = ConfigDirectory.CombinePath(DataSettings.Preference.CompilersFolder);
            DatabasesDierctory = ConfigDirectory.CombinePath(DataSettings.Preference.DatabasesFolder);
            MigrationsDirectory = ConfigDirectory.CombinePath(DataSettings.Preference.MigrationsFolder);
        }


        /// <summary>
        /// 编译目录。
        /// </summary>
        public string CompilersDirectory { get; set; }

        /// <summary>
        /// 数据库目录。
        /// </summary>
        public string DatabasesDierctory { get; set; }

        /// <summary>
        /// 迁移目录。
        /// </summary>
        public string MigrationsDirectory { get; set; }


        /// <summary>
        /// 支持实体框架设计时服务（默认支持）。
        /// </summary>
        public bool SupportsEntityFrameworkDesignTimeServices { get; set; }
            = true;


        /// <summary>
        /// 绑定配置根包含的连接字符串集合配置节（支持加密连接字符串）。
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
        /// ConfigurationRoot is null.
        /// </exception>
        /// <param name="validateFactory">给定用于验证连接字符串的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDependency"/>。</returns>
        public virtual DataBuilderDependency BindConnectionStrings(Func<string, string> validateFactory = null)
        {
            ConfigurationRoot.NotNull(nameof(ConfigurationRoot));
            return BindConnectionStrings(ConfigurationRoot.GetSection(ConnectionStringsSectionName), validateFactory);
        }

        /// <summary>
        /// 绑定连接字符串集合配置节（支持加密连接字符串）。
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
        public virtual DataBuilderDependency BindConnectionStrings(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            configuration.NotNull(nameof(configuration));

            UpdateDefaultConnectionString(configuration, validateFactory);
            UpdateWritingConnectionString(configuration, validateFactory);

            return this;
        }


        /// <summary>
        /// 绑定配置根包含的默认租户配置节（支持加密连接字符串）。
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
        public virtual DataBuilderDependency BindDefaultTenant(Func<string, string> validateFactory = null)
        {
            ConfigurationRoot.NotNull(nameof(ConfigurationRoot));
            return BindDefaultTenant(ConfigurationRoot.GetSection(DefaultTenantSectionName), validateFactory);
        }

        /// <summary>
        /// 绑定默认租户配置节（支持加密连接字符串）。
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
        public virtual DataBuilderDependency BindDefaultTenant(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            configuration.NotNull(nameof(configuration));

            Options.DefaultTenant.Name
                = configuration.GetValue(nameof(Options.DefaultTenant.Name), Options.DefaultTenant.Name);

            Options.DefaultTenant.Host
                = configuration.GetValue(nameof(Options.DefaultTenant.Host), Options.DefaultTenant.Host);

            Options.DefaultTenant.EncryptedConnectionStrings
                = configuration.GetValue(nameof(Options.DefaultTenant.EncryptedConnectionStrings), defaultValue: false);

            Options.DefaultTenant.WritingSeparation
                = configuration.GetValue(nameof(Options.DefaultTenant.WritingSeparation), defaultValue: false);

            UpdateDefaultConnectionString(configuration, validateFactory);
            UpdateWritingConnectionString(configuration, validateFactory);

            return this;
        }


        /// <summary>
        /// 更新默认连接字符串（支持加密连接字符串）。
        /// </summary>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="validateFactory">给定可验证字符串的工厂方法（可选）。</param>
        protected virtual void UpdateDefaultConnectionString(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = configuration.GetValue(nameof(Options.DefaultTenant.DefaultConnectionString),
                Options.DefaultTenant.DefaultConnectionString);

            if (Options.DefaultTenant.EncryptedConnectionStrings)
                connectionString = DecryptConnectionString(connectionString);

            Options.DefaultTenant.DefaultConnectionString = validateFactory?.Invoke(connectionString) ?? connectionString;
        }

        /// <summary>
        /// 更新写入连接字符串（支持加密连接字符串）。
        /// </summary>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="validateFactory">给定可验证字符串的工厂方法（可选）。</param>
        protected virtual void UpdateWritingConnectionString(IConfiguration configuration,
            Func<string, string> validateFactory = null)
        {
            var connectionString = configuration.GetValue(nameof(Options.DefaultTenant.WritingConnectionString),
                Options.DefaultTenant.WritingConnectionString);

            if (Options.DefaultTenant.EncryptedConnectionStrings)
                connectionString = DecryptConnectionString(connectionString);

            Options.DefaultTenant.WritingConnectionString = validateFactory?.Invoke(connectionString) ?? connectionString;
        }


        /// <summary>
        /// 加密连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回加密字符串。</returns>
        public static string EncryptConnectionString(string connectionString)
        {
            var buffer = connectionString.FromEncodingString();
            return buffer.AsAes().AsBase64String();
        }

        /// <summary>
        /// 解密连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回原始字符串。</returns>
        public static string DecryptConnectionString(string connectionString)
        {
            try
            {
                var buffer = connectionString.FromBase64String();
                return buffer.FromAes().AsEncodingString();
            }
            catch (FormatException)
            {
                // 非加密格式直接返回
                return connectionString;
            }
        }

    }
}
