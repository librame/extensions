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
    /// 抽象可分页静态扩展。
    /// </summary>
    public static class AbstractionPageableExtensions
    {

        #region IList

        /// <summary>
        /// 转换为可分页集合（即内存分页）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="list">给定的 <see cref="IList{T}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{T}"/>。</returns>
        public static IPageable<T> AsPagingByIndex<T>(this IList<T> list, int index, int size)
            where T : class
            => list.AsPaging(paging => paging.ComputeByIndex(index, size));

        /// <summary>
        /// 转换为可分页集合（即内存分页）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="list">给定的 <see cref="IList{T}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <returns>返回 <see cref="IPageable{T}"/>。</returns>
        public static IPageable<T> AsPaging<T>(this IList<T> list, Action<PagingDescriptor> computeAction)
            where T : class
        {
            try
            {
                var descriptor = new PagingDescriptor(list.Count);
                computeAction.Invoke(descriptor);

                return new Paging<T>(list, descriptor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region IQueryable

        /// <summary>
        /// 异步转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsAscendingPagingByIndexAsync<TEntity>(this IQueryable<TEntity> query,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class, IRank
            => query.AsPagingByIndexAsync(ordered => ordered.OrderBy(k => k.Rank), index, size, cancellationToken);

        /// <summary>
        /// 异步转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsDescendingPagingByIndexAsync<TEntity>(this IQueryable<TEntity> query,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class, IRank
            => query.AsPagingByIndexAsync(ordered => ordered.OrderByDescending(k => k.Rank), index, size, cancellationToken);

        /// <summary>
        /// 异步转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingByIndexAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class
            => query.AsPagingAsync(orderedFactory, paging => paging.ComputeByIndex(index, size), cancellationToken);


        /// <summary>
        /// 异步转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsAscendingPagingAsync<TEntity>(this IQueryable<TEntity> query,
            Action<PagingDescriptor> computeAction, CancellationToken cancellationToken = default)
            where TEntity : class, IRank
            => query.AsPagingAsync(ordered => ordered.OrderBy(k => k.Rank), computeAction, cancellationToken);

        /// <summary>
        /// 异步转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsDescendingPagingAsync<TEntity>(this IQueryable<TEntity> query,
            Action<PagingDescriptor> computeAction, CancellationToken cancellationToken = default)
            where TEntity : class, IRank
            => query.AsPagingAsync(ordered => ordered.OrderByDescending(k => k.Rank), computeAction, cancellationToken);

        /// <summary>
        /// 异步转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            Action<PagingDescriptor> computeAction, CancellationToken cancellationToken = default)
            where TEntity : class
            => orderedFactory.NotNull(nameof(orderedFactory))
                .Invoke(query).AsPagingAsync(computeAction, cancellationToken);


        /// <summary>
        /// 转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsAscendingPagingByIndex<TEntity>(this IQueryable<TEntity> query,
            int index, int size)
            where TEntity : class, IRank
            => query.AsPagingByIndex(ordered => ordered.OrderBy(k => k.Rank), index, size);

        /// <summary>
        /// 转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsDescendingPagingByIndex<TEntity>(this IQueryable<TEntity> query,
            int index, int size)
            where TEntity : class, IRank
            => query.AsPagingByIndex(ordered => ordered.OrderByDescending(k => k.Rank), index, size);

        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingByIndex<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            int index, int size)
            where TEntity : class
            => query.AsPaging(orderedFactory, paging => paging.ComputeByIndex(index, size));


        /// <summary>
        /// 转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsAscendingPaging<TEntity>(this IQueryable<TEntity> query,
            Action<PagingDescriptor> computeAction)
            where TEntity : class, IRank
            => query.AsPaging(ordered => ordered.OrderBy(k => k.Rank), computeAction);

        /// <summary>
        /// 转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsDescendingPaging<TEntity>(this IQueryable<TEntity> query,
            Action<PagingDescriptor> computeAction)
            where TEntity : class, IRank
            => query.AsPaging(ordered => ordered.OrderByDescending(k => k.Rank), computeAction);

        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPaging<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            Action<PagingDescriptor> computeAction)
            where TEntity : class
        {
            orderedFactory.NotNull(nameof(orderedFactory));
            return orderedFactory.Invoke(query).AsPaging(computeAction);
        }

        #endregion


        #region IOrderedQueryable

        /// <summary>
        /// 异步转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="orderedQuery">给定的 <see cref="IOrderedQueryable{TEntity}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingAsync<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            Action<PagingDescriptor> computeAction, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            orderedQuery.NotNull(nameof(orderedQuery));
            computeAction.NotNull(nameof(computeAction));

            return new ValueTask<IPageable<TEntity>>(Task.Run(() =>
            {
                IPageable<TEntity> paging = orderedQuery.AsPagingCore(computeAction);
                return paging;
            },
            cancellationToken));
        }


        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="orderedQuery">给定的 <see cref="IOrderedQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingByIndex<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            int index, int size)
            where TEntity : class
            => orderedQuery.AsPaging(paging => paging.ComputeByIndex(index, size));

        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="orderedQuery">给定的 <see cref="IOrderedQueryable{TEntity}"/>。</param>
        /// <param name="computeAction">给定的分页计算动作。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPaging<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            Action<PagingDescriptor> computeAction)
            where TEntity : class
            => orderedQuery.NotNull(nameof(orderedQuery)).AsPagingCore(computeAction);


        private static IPageable<TEntity> AsPagingCore<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            Action<PagingDescriptor> computeAction)
            where TEntity : class
        {
            try
            {
                var descriptor = new PagingDescriptor(orderedQuery.Count());
                computeAction.Invoke(descriptor);

                IQueryable<TEntity> query = orderedQuery;

                // 跳过条数
                if (descriptor.Skip > 0)
                    query = query.Skip(descriptor.Skip);

                // 获取条数
                if (descriptor.Size > 0)
                    query = query.Take(descriptor.Size);

                return new Paging<TEntity>(query.ToList(), descriptor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
