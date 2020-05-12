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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;

    /// <summary>
    /// 父标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IParentIdentifier<TId> : IIdentifier<TId>, IParentIdentifier
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 父标识。
        /// </summary>
        TId ParentId { get; set; }


        /// <summary>
        /// 获取父标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TId}"/>。</returns>
        new Task<TId> GetParentIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 设置父标识。
        /// </summary>
        /// <param name="parentId">给定的 <typeparamref name="TId"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetParentIdAsync(TId parentId, CancellationToken cancellationToken = default);
    }
}
