#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 计数接口。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public interface ICount<TValue> : IObjectCount
        where TValue : struct
    {
        /// <summary>
        /// 累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue DegressiveCount(TValue count);

        /// <summary>
        /// 异步累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> DegressiveCountAsync(TValue count, CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ProgressiveCount(TValue count);

        /// <summary>
        /// 异步累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveCountAsync(TValue count, CancellationToken cancellationToken = default);
    }
}
