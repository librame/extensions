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

namespace Librame.Extensions.Core.Identifiers
{
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
