#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
    using Services;

    /// <summary>
    /// 标识生成器接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IIdentityGenerator<TId> : IObjectIdentityGenerator
    {
        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateId(IClockService clock);

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateIdAsync(IClockService clock,
            CancellationToken cancellationToken = default);
    }
}
