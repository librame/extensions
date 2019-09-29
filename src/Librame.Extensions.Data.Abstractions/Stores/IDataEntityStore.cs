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
    /// 数据实体存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public interface IDataEntityStore<out TAccessor, TEntity> : IStore<TAccessor>
        where TAccessor : IAccessor
        where TEntity : class
    {
        /// <summary>
        /// 实体查询。
        /// </summary>
        IQueryable<TEntity> Entities { get; }


        /// <summary>
        /// 异步查找实体。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步操作。</returns>
        ValueTask<TEntity> FindEntityAsync(CancellationToken cancellationToken, params object[] keyValues);

        /// <summary>
        /// 异步获取分页实体集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTable}"/> 的异步操作。</returns>
        ValueTask<IPageable<TEntity>> GetPagingEntitiesAsync(int index, int size,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFactory = null,
            CancellationToken cancellationToken = default);
    }
}
