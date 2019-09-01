#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象生成式标识（默认标识类型为 <see cref="string"/>）。
    /// </summary>
    public abstract class AbstractGenId : AbstractGenId<string>, IGenId
    {
        /// <summary>
        /// 空标识。
        /// </summary>
        public static readonly string EmptyId
            = Guid.Empty.AsCombGuid().ToString();


        /// <summary>
        /// 构造一个 <see cref="AbstractGenId"/> 默认实例。
        /// </summary>
        public AbstractGenId()
        {
            // 默认使用空标识，新增推荐使用服务注入
            Id = EmptyId;
        }
    }


    /// <summary>
    /// 抽象生成式标识。
    /// </summary>
    /// <typeparam name="TGenId">指定的增量式标识类型。</typeparam>
    public abstract class AbstractGenId<TGenId> : AbstractId<TGenId>, IGenId<TGenId>
        where TGenId : IEquatable<TGenId>
    {
    }
}
