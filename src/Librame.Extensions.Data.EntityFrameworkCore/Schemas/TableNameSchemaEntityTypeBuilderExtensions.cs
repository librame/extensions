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
using Librame.Extensions.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="TableNameSchema"/> 实体类型构建器静态扩展。
    /// </summary>
    public static class TableNameSchemaEntityTypeBuilderExtensions
    {

        #region EntityTypeBuilder<TEntity>

        /// <summary>
        /// 映射表名架构（默认以实体名称复数形式为表名）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="descrAction">给定的 <see cref="Action{TableNameDescriptor}"/>（可自定义表名前后缀、连接符）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> builder,
            Action<TableNameDescriptor> descrAction = null,
            string schema = null)
            where TEntity : class
        {
            (builder as EntityTypeBuilder).ToTable(descrAction, schema);
            return builder;
        }

        /// <summary>
        /// 映射表名架构（默认以实体名称复数形式为表名）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="tableFactory">给定的实体类型转换表名架构的工厂方法。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> builder,
            Func<TableNameDescriptor, TableNameSchema> tableFactory)
            where TEntity : class
        {
            (builder as EntityTypeBuilder).ToTable(tableFactory);
            return builder;
        }

        /// <summary>
        /// 映射表名架构（默认以实体名称复数形式为表名）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="descrAction">给定的 <see cref="Action{TableNameDescriptor}"/>（可自定义表名前后缀、连接符）。</param>
        /// <param name="tableFactory">给定的实体类型转换表名架构的工厂方法。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> builder,
            Action<TableNameDescriptor> descrAction,
            Func<TableNameDescriptor, TableNameSchema> tableFactory)
            where TEntity : class
        {
            (builder as EntityTypeBuilder).ToTable(descrAction, tableFactory);
            return builder;
        }

        /// <summary>
        /// 映射表名架构。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="table">给定的 <see cref="TableNameSchema"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> builder,
            TableNameSchema table)
            where TEntity : class
        {
            (builder as EntityTypeBuilder).ToTable(table);
            return builder;
        }

        #endregion


        #region EntityTypeBuilder

        /// <summary>
        /// 映射表名架构（默认以实体名称复数形式为表名）。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="descrAction">给定的 <see cref="Action{TableNameDescriptor}"/>（可自定义表名前后缀、连接符）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder ToTable(this EntityTypeBuilder builder,
            Action<TableNameDescriptor> descrAction = null,
            string schema = null)
            => builder.ToTable(descrAction, descr => descr.AsSchema(schema));

        /// <summary>
        /// 映射表名架构（默认以实体名称复数形式为表名）。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="tableFactory">给定的实体类型转换表名架构的工厂方法。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder ToTable(this EntityTypeBuilder builder,
            Func<TableNameDescriptor, TableNameSchema> tableFactory)
            => builder.ToTable(descrAction: null, tableFactory);

        /// <summary>
        /// 映射表名架构（默认以实体名称复数形式为表名）。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="descrAction">给定的 <see cref="Action{TableNameDescriptor}"/>（可自定义表名前后缀、连接符）。</param>
        /// <param name="tableFactory">给定的实体类型转换表名架构的工厂方法。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static EntityTypeBuilder ToTable(this EntityTypeBuilder builder,
            Action<TableNameDescriptor> descrAction,
            Func<TableNameDescriptor, TableNameSchema> tableFactory)
        {
            builder.NotNull(nameof(builder));

            if (tableFactory.IsNull())
                tableFactory = descr => descr.AsSchema();

            var descriptor = new TableNameDescriptor(builder.Metadata.ClrType);
            descrAction?.Invoke(descriptor);

            var table = tableFactory.Invoke(descriptor);
            return builder.ToTable(table);
        }

        /// <summary>
        /// 映射表名架构。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        /// <param name="table">给定的 <see cref="TableNameSchema"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static EntityTypeBuilder ToTable(this EntityTypeBuilder builder, TableNameSchema table)
        {
            builder.NotNull(nameof(builder));
            table.NotNull(nameof(table));

            if (table.Schema.IsNotEmpty())
                return builder.ToTable(table.NameOrDescriptorString);

            return builder.ToTable(table.NameOrDescriptorString, table.Schema);
        }

        #endregion

    }
}
