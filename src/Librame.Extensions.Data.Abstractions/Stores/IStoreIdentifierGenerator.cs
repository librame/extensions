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
    using Core.Services;

    /// <summary>
    /// 存储标识符生成器接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IStoreIdentifierGenerator<TGenId> : IStoreIdentifierGenerator
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GenerateAuditIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GenerateEntityIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GenerateTenantIdAsync(CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 存储标识符生成器接口（主要用作标记）。
    /// </summary>
    public interface IStoreIdentifierGenerator : IService
    {
        /// <summary>
        /// 时钟。
        /// </summary>
        IClockService Clock { get; }


        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GenerateAuditIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GenerateEntityIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GenerateMigrationIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GenerateTenantIdAsync(CancellationToken cancellationToken = default);
    }
}
