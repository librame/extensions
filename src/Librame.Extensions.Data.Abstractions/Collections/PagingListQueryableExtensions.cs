#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// 分页列表查询静态扩展。
    /// </summary>
    public static class PagingListQueryableExtensions
    {
        /// <summary>
        /// 异步当作索引分页列表（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
        public static Task<IPagingList<TEntity>> AsPagingListByIndexAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory, int index, int size,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return Task.Factory.StartNew(() => query.AsPagingListByIndex(orderedFactory, index, size), cancellationToken);
        }

        /// <summary>
        /// 当作索引分页列表（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
        public static IPagingList<TEntity> AsPagingListByIndex<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory, int index, int size)
            where TEntity : class
        {
            return query.AsPagingList(orderedFactory, total => PagingDescriptor.CreateByIndex(index, size, total));
        }


        /// <summary>
        /// 异步当作跳过分页列表（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
        public static Task<IPagingList<TEntity>> AsPagingListBySkipAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory, int skip, int take,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return Task.Factory.StartNew(() => query.AsPagingListBySkip(orderedFactory, skip, take), cancellationToken);
        }

        /// <summary>
        /// 当作跳过分页列表（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
        public static IPagingList<TEntity> AsPagingListBySkip<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory, int skip, int take)
            where TEntity : class
        {
            return query.AsPagingList(orderedFactory, total => PagingDescriptor.CreateBySkip(skip, take, total));
        }


        /// <summary>
        /// 异步当作分页列表（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="pagingFactory">给定创建分页描述符的方法（输入参数为数据总条数）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
        public static Task<IPagingList<TEntity>> AsPagingListAsync<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory, Func<int, PagingDescriptor> pagingFactory,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return Task.Factory.StartNew(() => query.AsPagingList(orderedFactory, pagingFactory), cancellationToken);
        }

        /// <summary>
        /// 当作分页列表（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="orderedFactory">给定的排序方式。</param>
        /// <param name="pagingFactory">给定创建分页描述符的方法（输入参数为数据总条数）。</param>
        /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
        public static IPagingList<TEntity> AsPagingList<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory, Func<int, PagingDescriptor> pagingFactory)
            where TEntity : class
        {
            try
            {
                // 计算分页信息
                var total = orderedFactory.Invoke(query).Count();
                var descriptor = pagingFactory.Invoke(total);

                return new PagingList<TEntity>(query, descriptor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
