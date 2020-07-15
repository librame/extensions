#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Collections;

    /// <summary>
    /// 租户存储接口。
    /// </summary>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface ITenantStore<TTenant> : IStore
        where TTenant : class
    {
        /// <summary>
        /// 租户查询。
        /// </summary>
        IQueryable<TTenant> Tenants { get; }


        /// <summary>
        /// 异步查找指定租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        ValueTask<TTenant> FindTenantAsync(CancellationToken cancellationToken,
            params object[] keyValues);

        /// <summary>
        /// 异步获取所有租户集合。
        /// </summary>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IReadOnlyList{TTenant}"/> 的异步操作。</returns>
        ValueTask<IReadOnlyList<TTenant>> GetAllTenantsAsync
            (Func<IQueryable<TTenant>, IQueryable<TTenant>> queryFactory = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTenant}"/> 的异步操作。</returns>
        ValueTask<IPageable<TTenant>> GetPagingTenantsAsync(int index, int size,
            Func<IQueryable<TTenant>, IQueryable<TTenant>> queryFactory = null,
            CancellationToken cancellationToken = default);
    }
}
