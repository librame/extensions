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
    /// 对象发表接口。
    /// </summary>
    public interface IObjectPublication : IObjectCreation
    {
        /// <summary>
        /// 异步获取对象发表时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        ValueTask<object> GetObjectPublishedTimeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取对象发表者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object> GetObjectPublishedByAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置对象发表时间。
        /// </summary>
        /// <param name="newPublishedTime">给定的新发表时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        ValueTask<object> SetObjectPublishedTimeAsync(object newPublishedTime, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置对象发表者。
        /// </summary>
        /// <param name="newPublishedBy">给定的新发表者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object> SetObjectPublishedByAsync(object newPublishedBy, CancellationToken cancellationToken = default);
    }
}
