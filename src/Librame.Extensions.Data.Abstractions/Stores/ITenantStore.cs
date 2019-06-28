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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 租户存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface ITenantStore<TAccessor, TTenant> : IStore<TAccessor>
        where TAccessor : IAccessor
        where TTenant : class
    {
        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTenant}"/> 的异步操作。</returns>
        Task<IPageable<TTenant>> GetPagingTenantsAsync(int index, int size, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步创建租户。
        /// </summary>
        /// <param name="tenant">给定的 <typeparamref name="TTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        Task<EntityResult> CreateAsync(TTenant tenant, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步更新租户。
        /// </summary>
        /// <param name="tenant">给定的 <typeparamref name="TTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        Task<EntityResult> UpdateAsync(TTenant tenant, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步删除租户。
        /// </summary>
        /// <param name="tenant">给定的 <typeparamref name="TTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        Task<EntityResult> DeleteAsync(TTenant tenant, CancellationToken cancellationToken = default);
    }
}
