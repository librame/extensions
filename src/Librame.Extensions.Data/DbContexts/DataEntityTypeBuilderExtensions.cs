#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 实体类型构建器静态扩展。
    /// </summary>
    public static class DataEntityTypeBuilderExtensions
    {

        /// <summary>
        /// 映射表名。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="table">给定的 <see cref="ITableOptions"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, ITableOptions table)
            where TEntity : class
        {
            table.NotDefault(nameof(table));

            if (table.Schema.IsNotEmpty())
                return entityTypeBuilder.ToTable(table.Name);

            return entityTypeBuilder.ToTable(table.Name, table.Schema);
        }

        /// <summary>
        /// 映射分表名。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <param name="createRuleFactory">给定的创建分表规则工厂方法。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToShardingTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, IShardingOptions sharding,
            Func<IShardingOptions, IShardingRule> createRuleFactory = null)
            where TEntity : class
        {
            sharding.NotDefault(nameof(sharding));

            if (createRuleFactory.IsDefault())
            {
                createRuleFactory = options =>
                {
                    try
                    {
                        return (IShardingRule)Activator.CreateInstance(sharding.RuleType);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };
            }

            var shardingRule = createRuleFactory.Invoke(sharding);
            var table = shardingRule.ToTable(sharding, typeof(TEntity));

            return entityTypeBuilder.ToTable(table);
        }

    }
}
