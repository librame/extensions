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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象访问器静态扩展。
    /// </summary>
    public static class AbstractionAccessorExtensions
    {
        /// <summary>
        /// 异步查询指定标识的实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="id">给定的标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含实体的异步操作。</returns>
        public static Task<TEntity> QueryByIdAsync<TEntity, TId>(this IAccessor accessor, TId id,
            CancellationToken cancellationToken = default)
            where TEntity : class, IId<TId>
            where TId : IEquatable<TId>
        {
            return accessor.QueryResultAsync<TEntity>(query =>
            {
                return query.FirstOrDefault(p => p.Id.Equals(id));
            },
            cancellationToken);
        }


        /// <summary>
        /// 异步查询分页列表。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="orderedFactory">给定的排序工厂方法。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static Task<IPageable<TEntity>> QueryPagingAsync<TEntity>(this IAccessor accessor,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            Action<PagingDescriptor> computeAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return accessor.QueryResultAsync<TEntity, IPageable<TEntity>>(query =>
            {
                return query.AsPaging(orderedFactory, computeAction);
            },
            cancellationToken);
        }


        /// <summary>
        /// 异步查询结果。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="factory">给定的查询工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含结果的异步操作。</returns>
        public static Task<TEntity> QueryResultAsync<TEntity>(this IAccessor accessor,
            Func<IQueryable<TEntity>, TEntity> factory,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return accessor.QueryResultAsync<TEntity, TEntity>(factory,
                cancellationToken);
        }

        /// <summary>
        /// 异步查询结果。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="factory">给定的查询工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含结果的异步操作。</returns>
        public static Task<TResult> QueryResultAsync<TEntity, TResult>(this IAccessor accessor,
            Func<IQueryable<TEntity>, TResult> factory,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            accessor.NotNull(nameof(accessor));
            factory.NotNull(nameof(factory));

            return Task.Factory.StartNew(() =>
            {
                return factory.Invoke(accessor.Queryable<TEntity>());
            },
            cancellationToken);
        }

    }
}
