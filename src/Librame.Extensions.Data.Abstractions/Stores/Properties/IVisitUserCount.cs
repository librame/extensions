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
    /// 访问用户计数接口。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public interface IVisitUserCount<TValue> : IUserCount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 访问次数。
        /// </summary>
        /// <value>返回 <typeparamref name="TValue"/>。</value>
        TValue VisitCount { get; set; }

        /// <summary>
        /// 访问人数。
        /// </summary>
        /// <value>返回 <typeparamref name="TValue"/>。</value>
        TValue VisitorCount { get; set; }


        /// <summary>
        /// 异步累加访问次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveVisitCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步累加访问人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveVisitorCountAsync(CancellationToken cancellationToken = default);
    }
}
