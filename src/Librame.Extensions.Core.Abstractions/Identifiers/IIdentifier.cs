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

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IIdentifier<TId> : IIdentifier
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识。
        /// </summary>
        TId Id { get; set; }


        /// <summary>
        /// 获取标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TId}"/>。</returns>
        new Task<TId> GetIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 设置标识。
        /// </summary>
        /// <param name="id">给定的 <typeparamref name="TId"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetIdAsync(TId id, CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 标识符接口。
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// 异步获取标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{Object}"/>。</returns>
        Task<object> GetIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置标识。
        /// </summary>
        /// <param name="obj">给定的标识对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetIdAsync(object obj, CancellationToken cancellationToken = default);
    }
}
