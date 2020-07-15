#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="DbSet{TEntity}"/> 静态扩展。
    /// </summary>
    public static class EFCoreDbSetExtensions
    {
        /// <summary>
        /// 转换为数据库集管理器。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <returns>返回 <see cref="DbSetManager{TEntity}"/>。</returns>
        public static DbSetManager<TEntity> AsManager<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
            => new DbSetManager<TEntity>(dbSet);


        #region FirstOrDefaultByMax and FirstOrDefaultByMin

        /// <summary>
        /// 通过指定的结果选择器表达式查找符合最大结果的第一或默认实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="resultSelector">给定的结果选择器表达式。</param>
        /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
        public static TEntity FirstOrDefaultByMax<TEntity, TResult>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> resultSelector)
            where TEntity : class
            where TResult : IEquatable<TResult>
        {
            query.NotNull(nameof(query));
            return query.OrderByDescending(resultSelector).FirstOrDefault();
        }

        /// <summary>
        /// 异步通过指定的结果选择器表达式查找符合最大结果的第一或默认实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="query">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="resultSelector">给定的结果选择器表达式。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含 <typeparamref name="TEntity"/> 的异步操作。</returns>
        public static Task<TEntity> FirstOrDefaultByMaxAsync<TEntity, TResult>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> resultSelector,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TResult : IEquatable<TResult>
        {
            query.NotNull(nameof(query));
            return query.OrderByDescending(resultSelector).FirstOrDefaultAsync(cancellationToken);
        }


        /// <summary>
        /// 通过指定的结果选择器表达式查找符合最小结果的第一或默认实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="query">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="resultSelector">给定的结果选择器表达式。</param>
        /// <returns>返回实体。</returns>
        public static TEntity FirstOrDefaultByMin<TEntity, TResult>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> resultSelector)
            where TEntity : class
            where TResult : IEquatable<TResult>
        {
            query.NotNull(nameof(query));
            return query.OrderBy(resultSelector).FirstOrDefault();
        }

        /// <summary>
        /// 异步通过指定的结果选择器表达式查找符合最小结果的第一或默认实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="query">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="resultSelector">给定的结果选择器表达式。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含 <typeparamref name="TEntity"/> 的异步操作。</returns>
        public static Task<TEntity> FirstOrDefaultByMinAsync<TEntity, TResult>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> resultSelector,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TResult : IEquatable<TResult>
        {
            query.NotNull(nameof(query));
            return query.OrderBy(resultSelector).FirstOrDefaultAsync(cancellationToken);
        }

        #endregion

    }
}
