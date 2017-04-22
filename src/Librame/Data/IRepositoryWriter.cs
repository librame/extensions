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
using System.Threading.Tasks;

namespace Librame.Data
{
    /// <summary>
    /// 仓库写入器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IRepositoryWriter<T> : IRepositoryReadiness<T>
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


    /// <summary>
    /// 仓库写入器静态扩展。
    /// </summary>
    public static class RepositoryWriterExtensions
    {
        /// <summary>
        /// 异步保存。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库写入器接口。</param>
        /// <returns>返回一个异步操作。</returns>
        public static Task SaveAsync<T>(this IRepositoryWriter<T> repotitory)
        {
            return Task.Factory.StartNew(() => repotitory.Save());
        }


        /// <summary>
        /// 异步创建类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库写入器接口。</param>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回一个带类型实例的异步操作。</returns>
        public static Task<T> CreateAsync<T>(this IRepositoryWriter<T> repotitory, T entity, bool syncDatabase = true)
        {
            return Task.Factory.StartNew(() => repotitory.Create(entity, syncDatabase));
        }


        /// <summary>
        /// 异步更新类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库写入器接口。</param>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回一个带类型实例的异步操作。</returns>
        public static Task<T> UpdateAsync<T>(this IRepositoryWriter<T> repotitory, T entity, bool syncDatabase = true)
        {
            return Task.Factory.StartNew(() => repotitory.Update(entity, syncDatabase));
        }

        /// <summary>
        /// 通过更新获取单条符合指定查询表达式的类型实例。
        /// </summary>
        /// <param name="repotitory">给定的仓库写入器接口。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="updateAction">给定更新此类型实例的方法。</param>
        /// <returns>返回对象。</returns>
        public static Task<T> GetByUpdateAsync<T>(this IRepositoryWriter<T> repotitory, Expression<Func<T, bool>> predicate = null, Action<T> updateAction = null)
        {
            return Task<T>.Factory.StartNew(() => repotitory.GetByUpdate(predicate, updateAction));
        }


        /// <summary>
        /// 异步删除类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库写入器接口。</param>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回一个带类型实例的异步操作。</returns>
        public static Task<T> DeleteAsync<T>(this IRepositoryWriter<T> repotitory, T entity, bool syncDatabase = true)
        {
            return Task.Factory.StartNew(() => repotitory.Delete(entity, syncDatabase));
        }

    }
}
