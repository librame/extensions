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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
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


    /// <summary>
    /// 父标识符接口。
    /// </summary>
    public interface IParentIdentifier : IIdentifier
    {
        /// <summary>
        /// 异步获取父标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{Object}"/>。</returns>
        Task<object> GetParentIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置父标识。
        /// </summary>
        /// <param name="obj">给定的父标识对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetParentIdAsync(object obj, CancellationToken cancellationToken = default);
    }
}
