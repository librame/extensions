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
    /// 对象锁定期接口。
    /// </summary>
    public interface IObjectLockout
    {
        /// <summary>
        /// 锁定期结束时间类型。
        /// </summary>
        Type LockoutEndType { get; }


        /// <summary>
        /// 获取对象锁定期结束时间。
        /// </summary>
        /// <returns>返回锁定期结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
        object GetObjectLockoutEnd();

        /// <summary>
        /// 异步获取对象锁定期结束时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含锁定期结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
        ValueTask<object> GetObjectLockoutEndAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 设置对象锁定期结束时间。
        /// </summary>
        /// <param name="newLockoutEnd">给定的新锁定期结束时间对象。</param>
        /// <returns>返回锁定期结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
        object SetObjectLockoutEnd(object newLockoutEnd);

        /// <summary>
        /// 异步设置对象锁定期结束时间。
        /// </summary>
        /// <param name="newLockoutEnd">给定的新锁定期结束时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含锁定期结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
        ValueTask<object> SetObjectLockoutEndAsync(object newLockoutEnd, CancellationToken cancellationToken = default);
    }
}
