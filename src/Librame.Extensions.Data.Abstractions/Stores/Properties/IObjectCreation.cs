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
    /// 对象创建接口。
    /// </summary>
    public interface IObjectCreation
    {
        /// <summary>
        /// 获取创建时间类型。
        /// </summary>
        Type CreatedTimeType { get; }

        /// <summary>
        /// 获取创建者类型。
        /// </summary>
        Type CreatedByType { get; }


        /// <summary>
        /// 异步获取对象创建时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        ValueTask<object> GetObjectCreatedTimeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取对象创建者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object> GetObjectCreatedByAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置对象创建时间。
        /// </summary>
        /// <param name="newCreatedTime">给定的新创建时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        ValueTask<object> SetObjectCreatedTimeAsync(object newCreatedTime, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置对象创建者。
        /// </summary>
        /// <param name="newCreatedBy">给定的新创建者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object> SetObjectCreatedByAsync(object newCreatedBy, CancellationToken cancellationToken = default);
    }
}
