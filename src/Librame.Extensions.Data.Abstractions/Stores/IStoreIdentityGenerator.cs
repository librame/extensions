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
    using Core.Identifiers;
    using Core.Services;

    /// <summary>
    /// 存储标识生成器接口。
    /// </summary>
    public interface IStoreIdentityGenerator : IService
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        IClockService Clock { get; }

        /// <summary>
        /// 标识生成器工厂。
        /// </summary>
        /// <value>返回 <see cref="IIdentificationGeneratorFactory"/>。</value>
        IIdentificationGeneratorFactory Factory { get; }


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateId<TId>(string idName)
            where TId : IEquatable<TId>;

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateIdAsync<TId>(string idName, CancellationToken cancellationToken = default)
            where TId : IEquatable<TId>;
    }
}
