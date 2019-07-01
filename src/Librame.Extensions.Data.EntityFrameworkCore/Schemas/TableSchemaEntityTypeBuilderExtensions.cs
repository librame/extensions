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

        #region EntityTypeBuilder<TEntity>

        /// <summary>
        /// 映射表架构。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="factory">给定的实体类型转换表架构的工厂方法。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> builder, Func<Type, ITableSchema> factory)
            where TEntity : class
        {
            return builder.ToTable(factory?.Invoke(builder.Metadata.ClrType));
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
            (entityTypeBuilder as EntityTypeBuilder).ToTable(table);

            return entityTypeBuilder;
        }

        #endregion


        #region EntityTypeBuilder

        ///// <summary>
        ///// 映射表架构工厂方法。
        ///// </summary>
        ///// <typeparam name="TEntity">指定的实体类型。</typeparam>
        ///// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        ///// <param name="factory">给定的实体类型映射表架构的工厂方法。</param>
        ///// <returns>返回 <see cref="EntityTypeBuilder"/>。</returns>
        //public static EntityTypeBuilder ToTable<TEntity>(this EntityTypeBuilder entityTypeBuilder, Func<Type, ITableSchema> factory)
        //{
        //    return entityTypeBuilder.ToTable(factory?.Invoke(typeof(TEntity)));
        //}

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

        #endregion

    }
}
