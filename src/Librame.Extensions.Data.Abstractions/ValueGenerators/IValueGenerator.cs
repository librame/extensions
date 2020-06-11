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

namespace Librame.Extensions.Data.ValueGenerators
{
    /// <summary>
    /// 值生成器接口。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    public interface IValueGenerator<TValue> : IValueGeneratorIndication
    {
        /// <summary>
        /// 值类型。
        /// </summary>
        Type ValueType { get; }


        /// <summary>
        /// 异步获取值。
        /// </summary>
        /// <typeparam name="TInvoke">指定的调用类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TValue"/> 的异步操作。</returns>
        Task<TValue> GetValueAsync<TInvoke>(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取值。
        /// </summary>
        /// <param name="invokeType">给定的调用类型。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TValue"/> 的异步操作。</returns>
        Task<TValue> GetValueAsync(Type invokeType, CancellationToken cancellationToken = default);
    }
}
