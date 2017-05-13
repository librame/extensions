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

namespace Librame.Data.Repositories
{
    /// <summary>
    /// 仓库写入器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IRepositoryWriter<T> : IRepositoryEntry<T>
        where T : class
    {
        /// <summary>
        /// 保存。
        /// </summary>
        void Save();


        /// <summary>
        /// 创建类型实例。
        /// </summary>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回类型实例。</returns>
        T Create(T entity, bool syncDatabase = true);


        /// <summary>
        /// 更新类型实例。
        /// </summary>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回类型实例。</returns>
        T Update(T entity, bool syncDatabase = true);

        /// <summary>
        /// 通过更新获取单条符合指定查询表达式的类型实例。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询第一条数据）。</param>
        /// <param name="updateAction">给定更新此类型实例的方法。</param>
        /// <returns>返回类型实例。</returns>
        T GetByUpdate(Expression<Func<T, bool>> predicate = null, Action<T> updateAction = null);


        /// <summary>
        /// 删除类型实例。
        /// </summary>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回类型实例。</returns>
        T Delete(T entity, bool syncDatabase = true);
    }
}
