#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Librame.Extensions.Data.Accessors
{
    using Core.Combiners;
    using Data.Builders;

    /// <summary>
    /// 数据库上下文访问器助手。
    /// </summary>
    public static class DbContextAccessorHelper
    {

        #region AccessorIsolateableString

        /// <summary>
        /// 生成访问器（持久化）文件路径组合器（支持访问器、设计时、连接字符串、租户隔离）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="extension">给定的文件扩展名。</param>
        /// <param name="basePathFactory">给定的基础路径工厂方法。</param>
        /// <param name="designTimeType">给定的设计时类型（可选）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner GenerateAccessorFilePath(DbContextAccessorBase accessor,
            string extension, Func<DataBuilderDependency, string> basePathFactory,
            Type designTimeType = null)
        {
            var basePath = basePathFactory?.Invoke(accessor?.Dependency);
            return GenerateAccessorFilePath(accessor, extension, basePath, designTimeType);
        }

        /// <summary>
        /// 生成访问器（持久化）文件路径组合器（支持访问器、设计时、连接字符串、租户隔离）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="extension">给定的文件扩展名。</param>
        /// <param name="basePath">给定的基础路径（可选）。</param>
        /// <param name="designTimeType">给定的设计时类型（可选）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static FilePathCombiner GenerateAccessorFilePath(DbContextAccessorBase accessor,
            string extension, string basePath = null, Type designTimeType = null)
        {
            accessor.NotNull(nameof(accessor));
            extension.NotEmpty(nameof(extension));

            var filePath = new FilePathCombiner(GetAccessorFileName(), basePath);
            filePath.CreateDirectory();

            return filePath;

            // GetAccessorFileName
            string GetAccessorFileName()
            {
                var connector = DataSettings.Preference.AccessorIsolateableStringConnector;

                var builder = new StringBuilder();

                AppendAccessorIsolateableString(accessor, builder, connector, designTimeType);

                builder.Append(extension);

                return builder.ToString();
            }
        }


        /// <summary>
        /// 生成访问器（持久化）键名（借助访问器文件路径组合器支持访问器、设计时、连接字符串、租户隔离）。
        /// </summary>
        /// <param name="invokeType">给定的调用类型。</param>
        /// <param name="accessorFilePath">给定的访问器（持久化）文件路径组合器。</param>
        /// <returns>返回字符串。</returns>
        public static string GenerateAccessorKey(Type invokeType,
            FilePathCombiner accessorFilePath)
            => GenerateAccessorKey(invokeType.GetDisplayName(true), accessorFilePath);

        /// <summary>
        /// 生成访问器（持久化）键名（借助访问器文件路径组合器支持访问器、设计时、连接字符串、租户隔离）。
        /// </summary>
        /// <param name="invokeTypeName">给定的调用类型名称。</param>
        /// <param name="accessorFilePath">给定的访问器（持久化）文件路径组合器。</param>
        /// <returns>返回字符串。</returns>
        public static string GenerateAccessorKey(string invokeTypeName,
            FilePathCombiner accessorFilePath)
        {
            invokeTypeName.NotNull(nameof(invokeTypeName));
            accessorFilePath.NotNull(nameof(accessorFilePath));

            var sb = new StringBuilder();

            sb.Append(invokeTypeName);
            sb.Append(';');

            // 访问器文件路径组合器已作有效隔离
            sb.Append(accessorFilePath);

            return sb.ToString();
        }


        /// <summary>
        /// 获取迁移缓存键。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <returns>返回字符串。</returns>
        public static string GetMigrationCacheKey(DbContextAccessorBase dbContextAccessor)
            => GenerateAccessorKey(dbContextAccessor, "MigrationAccessorService");

        /// <summary>
        /// 生成访问器（持久化）键名（支持访问器、设计时、连接字符串、租户隔离）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="invokeType">给定的调用类型。</param>
        /// <param name="designTimeType">给定的设计时类型（可选）。</param>
        /// <param name="partialKeys">给定的部分键集合。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static string GenerateAccessorKey(DbContextAccessorBase accessor,
            Type invokeType, Type designTimeType = null, params string[] partialKeys)
            => GenerateAccessorKey(accessor, invokeType.GetDisplayName(true),
                designTimeType, partialKeys);

        /// <summary>
        /// 生成访问器（持久化）键名（支持访问器、设计时、连接字符串、租户隔离）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="invokeTypeName">给定的调用类型名称。</param>
        /// <param name="designTimeType">给定的设计时类型（可选）。</param>
        /// <param name="partialKeys">给定的部分键集合。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static string GenerateAccessorKey(DbContextAccessorBase accessor,
            string invokeTypeName, Type designTimeType = null, params string[] partialKeys)
        {
            accessor.NotNull(nameof(accessor));
            invokeTypeName.NotNull(nameof(invokeTypeName));

            var connector = DataSettings.Preference.AccessorIsolateableStringConnector;

            var builder = new StringBuilder();

            builder.Append(invokeTypeName);
            builder.Append(connector);

            AppendAccessorIsolateableString(accessor, builder, connector, designTimeType);

            partialKeys.ForEach((key, index) =>
            {
                builder.Append(key);

                if (index < partialKeys.Length - 1)
                    builder.Append(connector);
            });

            return builder.ToString();
        }


        private static void AppendAccessorIsolateableString(DbContextAccessorBase accessor,
            StringBuilder builder, char connector, Type designTimeType = null)
        {
            if (designTimeType.IsNull())
            {
                var dataBuilder = accessor.ApplicationServiceProvider.GetService<IDataBuilder>();
                designTimeType = dataBuilder.DatabaseDesignTimeType;
            }

            builder.Append(accessor.CurrentType.GetDisplayName(true)
                .TrimEnd("DbContextAccessor").TrimEnd("Accessor"));
            builder.Append(connector);

            builder.Append(designTimeType.GetDisplayName(true)
                .TrimEnd("DesignTimeServices"));
            builder.Append(connector);

            builder.Append(accessor.GetCurrentConnectionDescription()
                .TrimEnd("ConnectionString"));
            builder.Append(connector);

            builder.Append(accessor.CurrentTenant.Name);
            builder.Append(connector);

            builder.Append(accessor.CurrentTenant.Host);
        }

        #endregion

    }
}
