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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Collections;

    /// <summary>
    /// 表格存储接口。
    /// </summary>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    public interface ITabulationStore<TTabulation> : IStore
        where TTabulation : class
    {
        /// <summary>
        /// 表格查询。
        /// </summary>
        IQueryable<TTabulation> Tabulations { get; }


        /// <summary>
        /// 异步查找表格。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTabulation"/> 的异步操作。</returns>
        ValueTask<TTabulation> FindTabulationAsync(CancellationToken cancellationToken, params object[] keyValues);

        /// <summary>
        /// 异步获取分页表格集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTabulation}"/> 的异步操作。</returns>
        ValueTask<IPageable<TTabulation>> GetPagingTabulationsAsync(int index, int size,
            Func<IQueryable<TTabulation>, IQueryable<TTabulation>> queryFactory = null,
            CancellationToken cancellationToken = default);
    }
}
