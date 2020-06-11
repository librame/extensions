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

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IStoreInitializer<TGenId> : IStoreInitializerIndication
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 存储标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</value>
        IStoreIdentifierGenerator<TGenId> IdentifierGenerator { get; }


        /// <summary>
        /// 初始化存储。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TGenId}"/>。</param>
        void Initialize(IStoreHub<TGenId> stores);
    }
}
