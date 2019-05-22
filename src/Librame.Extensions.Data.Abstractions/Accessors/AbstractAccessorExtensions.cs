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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    ///// <summary>
    ///// <see cref="IAccessor"/> 静态扩展。
    ///// </summary>
    //public static class AbstractAccessorExtensions
    //{
    //    /// <summary>
    //    /// 异步检测数据存在。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="predicate">给定的断定条件（可选）。</param>
    //    /// <returns>返回一个包含布尔值的异步操作。</returns>
    //    public static Task<bool> ExistsAsync<TEntity>(this IAccessor accessor,
    //        Expression<Func<TEntity, bool>> predicate = null)
    //        where TEntity : class
    //    {
    //        return accessor.QueryResultAsync<TEntity, bool>(query =>
    //        {
    //            return null == predicate ? query.Any() : query.Any(predicate);
    //        });
    //    }


    //    /// <summary>
    //    /// 异步查询结果。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="factory">给定的查询工厂方法。</param>
    //    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    //    /// <returns>返回一个包含结果的异步操作。</returns>
    //    public static Task<TEntity> QueryResultAsync<TEntity>(this IAccessor accessor,
    //        Func<IQueryable<TEntity>, TEntity> factory,
    //        CancellationToken cancellationToken = default)
    //        where TEntity : class
    //    {
    //        return accessor.QueryResultAsync(factory, cancellationToken);
    //    }


    //    /// <summary>
    //    /// 异步查询分页列表。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="index">给定的页索引。</param>
    //    /// <param name="size">给定的页大小。</param>
    //    /// <param name="orderedFactory">给定的排序工厂方法。</param>
    //    /// <param name="queryFactory">给定的查询工厂方法。</param>
    //    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    //    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    //    public static Task<IPagingList<TEntity>> QueryPagingListByIndexAsync<TEntity>(this IAccessor accessor,
    //        int index, int size,
    //        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFactory = null,
    //        CancellationToken cancellationToken = default)
    //        where TEntity : class
    //    {
    //        return accessor.QueryPagingListAsync(total => PagingDescriptor.CreateByIndex(index, size, total),
    //            orderedFactory, queryFactory, cancellationToken);
    //    }

    //    /// <summary>
    //    /// 异步查询分页列表。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="skip">给定的跳过条数。</param>
    //    /// <param name="take">给定的获取条数。</param>
    //    /// <param name="orderedFactory">给定的排序工厂方法。</param>
    //    /// <param name="queryFactory">给定的查询工厂方法。</param>
    //    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    //    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    //    public static Task<IPagingList<TEntity>> QueryPagingListBySkipAsync<TEntity>(this IAccessor accessor,
    //        int skip, int take,
    //        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedFactory,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFactory = null,
    //        CancellationToken cancellationToken = default)
    //        where TEntity : class
    //    {
    //        return accessor.QueryPagingListAsync(total => PagingDescriptor.CreateBySkip(skip, take, total),
    //            orderedFactory, queryFactory, cancellationToken);
    //    }


    //    /// <summary>
    //    /// 异步增加集合。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="entities">给定要增加的实体集合。</param>
    //    /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
    //    public static Task<EntityResult> AddAsync<TEntity>(this IAccessor accessor, params TEntity[] entities)
    //        where TEntity : class
    //    {
    //        return accessor.AddAsync(default, entities);
    //    }

    //    /// <summary>
    //    /// 异步更新集合。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="entities">给定要更新的实体集合。</param>
    //    /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
    //    public static Task<EntityResult> UpdateAsync<TEntity>(this IAccessor accessor, params TEntity[] entities)
    //        where TEntity : class
    //    {
    //        return accessor.UpdateAsync(default, entities);
    //    }

    //    /// <summary>
    //    /// 异步删除集合。
    //    /// </summary>
    //    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="entities">给定要删除的实体集合。</param>
    //    /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
    //    public static Task<EntityResult> DeleteAsync<TEntity>(this IAccessor accessor, params TEntity[] entities)
    //        where TEntity : class
    //    {
    //        return accessor.DeleteAsync(default, entities);
    //    }


    //    /// <summary>
    //    /// 重载保存更改。
    //    /// </summary>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <returns>返回受影响的行数。</returns>
    //    public static int SaveChanges(this IAccessor accessor)
    //    {
    //        return accessor.SaveChanges(true);
    //    }

    //    /// <summary>
    //    /// 重载异步保存更改。
    //    /// </summary>
    //    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    //    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    //    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    //    public static Task<int> SaveChangesAsync(this IAccessor accessor, CancellationToken cancellationToken = default)
    //    {
    //        return accessor.SaveChangesAsync(true, cancellationToken);
    //    }

    //}
}
