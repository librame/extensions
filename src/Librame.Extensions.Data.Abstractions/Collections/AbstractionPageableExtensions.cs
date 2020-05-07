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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Collections
{
    using Stores;

    /// <summary>
    /// 抽象可分页静态扩展。
    /// </summary>
    public static class AbstractionPageableExtensions
    {

        #region ICollection

        /// <summary>
        /// 转换为可分页集合（内存分页）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="rows">给定的 <see cref="IList{T}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{T}"/>。</returns>
        public static IPageable<T> AsPagingByIndex<T>(this ICollection<T> rows, int index, int size)
            where T : class
            => rows.AsPaging(paging => paging.ComputeByIndex(index, size));

        /// <summary>
        /// 转换为可分页集合（内存分页）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="rows">给定的 <see cref="IList{T}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{T}"/>。</returns>
        public static IPageable<T> AsPagingBySkip<T>(this ICollection<T> rows, int skip, int take)
            where T : class
            => rows.AsPaging(paging => paging.ComputeBySkip(skip, take));

        private static IPageable<T> AsPaging<T>(this ICollection<T> rows, Action<PagingDescriptor> computeAction)
            where T : class
        {
            var descriptor = new PagingDescriptor((int)rows?.Count);
            computeAction?.Invoke(descriptor);

            return new PagingCollection<T>(rows, descriptor);
        }

        #endregion


        #region IQueryable by Index

        /// <summary>
        /// 异步转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingByIndexAsync<TEntity>(this IQueryable<TEntity> query,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<float>
            => query.AsPagingByIndexAsync<TEntity, float>(index, size, cancellationToken);

        /// <summary>
        /// 异步转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingByIndexAsync<TEntity, TRank>(this IQueryable<TEntity> query,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
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
            where TEntity : class, ISortable<float>
            => query.AsDescendingPagingByIndexAsync<TEntity, float>(index, size, cancellationToken);

        /// <summary>
        /// 异步转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsDescendingPagingByIndexAsync<TEntity, TRank>(this IQueryable<TEntity> query,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ValueTask<IPageable<TEntity>> AsPagingByIndexAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            orderedFactory.NotNull(nameof(orderedFactory));
            return orderedFactory.Invoke(query).AsPagingAsync(paging => paging.ComputeByIndex(index, size), cancellationToken);
        }


        /// <summary>
        /// 转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingByIndex<TEntity>(this IQueryable<TEntity> query,
            int index, int size)
            where TEntity : class, ISortable<float>
            => query.AsPagingByIndex<TEntity, float>(index, size);

        /// <summary>
        /// 转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingByIndex<TEntity, TRank>(this IQueryable<TEntity> query,
            int index, int size)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
            => query.AsPagingByIndex(q => q.OrderBy(k => k.Rank), index, size);


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
            where TEntity : class, ISortable<float>
            => query.AsDescendingPagingByIndex<TEntity, float>(index, size);

        /// <summary>
        /// 转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsDescendingPagingByIndex<TEntity, TRank>(this IQueryable<TEntity> query,
            int index, int size)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
            => query.AsPagingByIndex(q => q.OrderByDescending(k => k.Rank), index, size);


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
            => orderedFactory?.Invoke(query).AsPaging(paging => paging.ComputeByIndex(index, size));

        #endregion


        #region IQueryable by Skip

        /// <summary>
        /// 异步转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingBySkipAsync<TEntity>(this IQueryable<TEntity> query,
            int skip, int take, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<float>
            => query.AsPagingBySkipAsync<TEntity, float>(skip, take, cancellationToken);

        /// <summary>
        /// 异步转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingBySkipAsync<TEntity, TRank>(this IQueryable<TEntity> query,
            int skip, int take, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
            => query.AsPagingBySkipAsync(ordered => ordered.OrderBy(k => k.Rank), skip, take, cancellationToken);


        /// <summary>
        /// 异步转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsDescendingPagingBySkipAsync<TEntity>(this IQueryable<TEntity> query,
            int skip, int take, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<float>
            => query.AsDescendingPagingBySkipAsync<TEntity, float>(skip, take, cancellationToken);

        /// <summary>
        /// 异步转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsDescendingPagingBySkipAsync<TEntity, TRank>(this IQueryable<TEntity> query,
            int skip, int take, CancellationToken cancellationToken = default)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
            => query.AsPagingBySkipAsync(ordered => ordered.OrderByDescending(k => k.Rank), skip, take, cancellationToken);


        /// <summary>
        /// 异步转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ValueTask<IPageable<TEntity>> AsPagingBySkipAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            int skip, int take, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            orderedFactory.NotNull(nameof(orderedFactory));
            return orderedFactory.Invoke(query).AsPagingAsync(paging => paging.ComputeBySkip(skip, take), cancellationToken);
        }


        /// <summary>
        /// 转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingBySkip<TEntity>(this IQueryable<TEntity> query,
            int skip, int take)
            where TEntity : class, ISortable<float>
            => query.AsPagingBySkip<TEntity, float>(skip, take);

        /// <summary>
        /// 转换为可升序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingBySkip<TEntity, TRank>(this IQueryable<TEntity> query,
            int skip, int take)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
            => query.AsPagingBySkip(q => q.OrderBy(k => k.Rank), skip, take);


        /// <summary>
        /// 转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsDescendingPagingBySkip<TEntity>(this IQueryable<TEntity> query,
            int skip, int take)
            where TEntity : class, ISortable<float>
            => query.AsDescendingPagingBySkip<TEntity, float>(skip, take);

        /// <summary>
        /// 转换为可降序分页集合（注：默认按排序字段进行排序）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TRank">指定的排序类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsDescendingPagingBySkip<TEntity, TRank>(this IQueryable<TEntity> query,
            int skip, int take)
            where TEntity : class, ISortable<TRank>
            where TRank : struct
            => query.AsPagingBySkip(q => q.OrderByDescending(k => k.Rank), skip, take);


        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingBySkip<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
            int skip, int take)
            where TEntity : class
            => orderedFactory?.Invoke(query).AsPaging(paging => paging.ComputeBySkip(skip, take));

        #endregion


        #region IOrderedQueryable

        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="orderedQuery">给定的 <see cref="IOrderedQueryable{TEntity}"/>。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingByIndexAsync<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            int index, int size, CancellationToken cancellationToken = default)
            where TEntity : class
            => orderedQuery.AsPagingAsync(paging => paging.ComputeByIndex(index, size), cancellationToken);

        /// <summary>
        /// 转换为可分页集合（注：需要对查询接口进行排序操作，否则 LINQ 会抛出未排序异常）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="orderedQuery">给定的 <see cref="IOrderedQueryable{TEntity}"/>。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public static ValueTask<IPageable<TEntity>> AsPagingBySkipAsync<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            int skip, int take, CancellationToken cancellationToken = default)
            where TEntity : class
            => orderedQuery.AsPagingAsync(paging => paging.ComputeBySkip(skip, take), cancellationToken);

        private static ValueTask<IPageable<TEntity>> AsPagingAsync<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            Action<PagingDescriptor> computeAction, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            orderedQuery.NotNull(nameof(orderedQuery));
            computeAction.NotNull(nameof(computeAction));

            return new ValueTask<IPageable<TEntity>>(Task.Run(() => orderedQuery.AsPagingCore(computeAction), cancellationToken));
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
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPageable{TEntity}"/>。</returns>
        public static IPageable<TEntity> AsPagingBySkip<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            int skip, int take)
            where TEntity : class
            => orderedQuery.AsPaging(paging => paging.ComputeBySkip(skip, take));

        private static IPageable<TEntity> AsPaging<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            Action<PagingDescriptor> computeAction)
            where TEntity : class
        {
            orderedQuery.NotNull(nameof(orderedQuery));
            computeAction.NotNull(nameof(computeAction));

            return orderedQuery.AsPagingCore(computeAction);
        }


        private static IPageable<TEntity> AsPagingCore<TEntity>(this IOrderedQueryable<TEntity> orderedQuery,
            Action<PagingDescriptor> computeAction)
            where TEntity : class
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

            return new PagingCollection<TEntity>(query.ToList(), descriptor);
        }

        #endregion

    }
}
