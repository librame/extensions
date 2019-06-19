#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="ITableSchema"/> 实体类型构建器静态扩展。
    /// </summary>
    public static class TableSchemaEntityTypeBuilderExtensions
    {
        /// <summary>
        /// 映射内部表架构（格式参考：__names?）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="formatter">给定的格式化器。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToInternalTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Func<string, string> formatter = null, string schema = null)
            where TEntity : class
        {
            return entityTypeBuilder.ToTable(typeof(TEntity).AsInternalTableSchema(formatter, schema));
        }

        /// <summary>
        /// 映射表架构（格式参考：names?）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="formatter">给定的表名格式化器（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Func<string, string> formatter = null, string schema = null)
            where TEntity : class
        {
            return entityTypeBuilder.ToTable(typeof(TEntity).AsTableSchema(formatter, schema));
        }

        /// <summary>
        /// 映射表架构。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="table">给定的 <see cref="ITableSchema"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, ITableSchema table)
            where TEntity : class
        {
            table.NotNull(nameof(table));

            if (!table.Schema.IsNullOrEmpty())
                return entityTypeBuilder.ToTable(table.Name);

            return entityTypeBuilder.ToTable(table.Name, table.Schema);
        }

        /// <summary>
        /// 映射表架构。
        /// </summary>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        /// <param name="table">给定的 <see cref="ITableSchema"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder"/>。</returns>
        public static EntityTypeBuilder ToTable(this EntityTypeBuilder entityTypeBuilder, ITableSchema table)
        {
            table.NotNull(nameof(table));

            if (!table.Schema.IsNullOrEmpty())
                return entityTypeBuilder.ToTable(table.Name);

            return entityTypeBuilder.ToTable(table.Name, table.Schema);
        }
    }
}
