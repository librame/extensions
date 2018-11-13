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
using System.Linq;

namespace Librame.Extensions
{
    using Data;

    /// <summary>
    /// 分页查询静态扩展。
    /// </summary>
    public static class PagingQueryableExtensions
    {

        /// <summary>
        /// 当作索引分页（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="order">给定的排序方式。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
        public static IPagingList<TEntity> AsPagingByIndex<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order, int index, int size)
            where TEntity : class
        {
            return query.AsPaging(order, total => PagingDescriptor.CreateByIndex(index, size, total));
        }

        /// <summary>
        /// 当作跳过分页（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="order">给定的排序方式。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
        public static IPagingList<TEntity> AsPagingBySkip<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order, int skip, int take)
            where TEntity : class
        {
            return query.AsPaging(order, total => PagingDescriptor.CreateBySkip(skip, take, total));
        }

        /// <summary>
        /// 当作分页（符合指定查询表达式的）实体集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <param name="order">给定的排序方式。</param>
        /// <param name="createDescriptorFactory">给定创建分页描述符的方法（输入参数为数据总条数）。</param>
        /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
        public static IPagingList<TEntity> AsPaging<TEntity>(this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order, Func<int, PagingDescriptor> createDescriptorFactory)
            where TEntity : class
        {
            try
            {
                // 计算分页信息
                var total = order.Invoke(query).Count();
                var descriptor = createDescriptorFactory.Invoke(total);

                // 跳过条数
                if (descriptor.Skip > 0)
                    query = query.Skip(descriptor.Skip);

                // 获取条数
                if (descriptor.Size > 0)
                    query = query.Take(descriptor.Size);

                // 执行查询
                return query.ToList().AsPaging(descriptor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
