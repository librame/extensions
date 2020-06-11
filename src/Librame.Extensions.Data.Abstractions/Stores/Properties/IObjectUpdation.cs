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

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 对象更新接口。
    /// </summary>
    public interface IObjectUpdation : IObjectCreation
    {
        /// <summary>
        /// 异步获取对象更新时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        ValueTask<object> GetObjectUpdatedTimeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取对象更新者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object> GetObjectUpdatedByAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置对象更新时间。
        /// </summary>
        /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        ValueTask<object> SetObjectUpdatedTimeAsync(object newUpdatedTime, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置对象更新者。
        /// </summary>
        /// <param name="newUpdatedBy">给定的新更新者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object> SetObjectUpdatedByAsync(object newUpdatedBy, CancellationToken cancellationToken = default);
    }
}
