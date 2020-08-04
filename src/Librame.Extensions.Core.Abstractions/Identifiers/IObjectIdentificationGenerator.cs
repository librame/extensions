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
    using Services;

    /// <summary>
    /// 对象标识生成器接口。
    /// </summary>
    public interface IObjectIdentificationGenerator
    {
        /// <summary>
        /// 标识类型。
        /// </summary>
        Type IdType { get; }


        /// <summary>
        /// 生成对象标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回标识符对象。</returns>
        object GenerateObjectId(IClockService clock);

        /// <summary>
        /// 异步生成对象标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识符对象的异步操作。</returns>
        Task<object> GenerateObjectIdAsync(IClockService clock,
            CancellationToken cancellationToken = default);
    }
}
