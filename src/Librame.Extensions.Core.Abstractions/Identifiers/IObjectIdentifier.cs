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
    /// 对象标识符接口。
    /// </summary>
    public interface IObjectIdentifier
    {
        /// <summary>
        /// 标识类型。
        /// </summary>
        Type IdType { get; }


        /// <summary>
        /// 异步获取对象标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含对象标识（兼容各种引用与值类型标识）的异步操作。</returns>
        ValueTask<object> GetObjectIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="newId">给定的新标识对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含对象标识（兼容各种引用与值类型标识）的异步操作。</returns>
        ValueTask<object> SetObjectIdAsync(object newId, CancellationToken cancellationToken = default);
    }
}
