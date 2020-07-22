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
    /// <summary>
    /// 对象计数接口。
    /// </summary>
    public interface IObjectCount
    {
        /// <summary>
        /// 值类型。
        /// </summary>
        Type ValueType { get; }


        /// <summary>
        /// 累减对象计数。
        /// </summary>
        /// <param name="count">给定要累减的计数对象。</param>
        /// <returns>返回对象。</returns>
        object DegressiveObjectCount(object count);

        /// <summary>
        /// 异步累减对象计数。
        /// </summary>
        /// <param name="count">给定要累减的计数对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
        ValueTask<object> DegressiveObjectCountAsync(object count, CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加对象计数。
        /// </summary>
        /// <param name="count">给定要累加的对象计数对象。</param>
        /// <returns>返回对象。</returns>
        object ProgressiveObjectCount(object count);

        /// <summary>
        /// 异步累加对象计数。
        /// </summary>
        /// <param name="count">给定要累加的对象计数对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
        ValueTask<object> ProgressiveObjectCountAsync(object count, CancellationToken cancellationToken = default);
    }
}
