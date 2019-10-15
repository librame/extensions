#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Linq.Expressions;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储表达式。
    /// </summary>
    public static class StoreExpression
    {
        /// <summary>
        /// 获取数据实体唯一索引表达式。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <param name="schema">给定的架构。</param>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回查询表达式。</returns>
        public static Expression<Func<TEntity, bool>> GetEntityUniqueIndexExpression<TEntity, TGenId>(string schema, string name)
            where TEntity : DataEntity<TGenId>
            where TGenId : IEquatable<TGenId>
        {
            schema.NotEmpty(nameof(schema));
            name.NotEmpty(nameof(name));

            return p => p.Schema == schema && p.Name == name;
        }

        ///// <summary>
        ///// 获取数据迁移唯一索引表达式。
        ///// </summary>
        ///// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        ///// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        ///// <param name="modelHash">给定的模型哈希。</param>
        ///// <returns>返回查询表达式。</returns>
        //public static Expression<Func<TMigration, bool>> GetMigrationUniqueIndexExpression<TMigration, TGenId>(string modelHash)
        //    where TMigration : DataMigration<TGenId>
        //    where TGenId : IEquatable<TGenId>
        //{
        //    modelHash.NotEmpty(nameof(modelHash));

        //    return p => p.ModelHash == modelHash;
        //}

        /// <summary>
        /// 获取数据租户唯一索引表达式。
        /// </summary>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回查询表达式。</returns>
        public static Expression<Func<TTenant, bool>> GetTenantUniqueIndexExpression<TTenant, TGenId>(string name, string host)
            where TTenant : DataTenant<TGenId>
            where TGenId : IEquatable<TGenId>
        {
            name.NotEmpty(nameof(name));
            host.NotEmpty(nameof(host));

            return p => p.Name == name && p.Host == host;
        }

    }
}
