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
    /// 抽象生成式标识实体（默认标识类型为 <see cref="string"/>、排序类型为 <see cref="float"/>、状态类型为 <see cref="DataStatus"/>）。
    /// </summary>
    public abstract class AbstractEntityWithGenId : AbstractEntity<string>, IGenId
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntityWithGenId"/> 默认实例。
        /// </summary>
        public AbstractEntityWithGenId()
        {
            // 默认使用空标识，新增推荐使用服务注入
            Id =  AbstractGenId.EmptyId;
        }
    }

    /// <summary>
    /// 抽象生成式标识实体（默认标识类型为 <see cref="string"/>）。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntityWithGenId<TRank, TStatus> : AbstractEntity<string, TRank, TStatus>, IGenId
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntityWithGenId"/> 默认实例。
        /// </summary>
        public AbstractEntityWithGenId()
        {
            // 默认使用空标识，新增推荐使用服务注入
            Id = AbstractGenId.EmptyId;
        }
    }


    /// <summary>
    /// 抽象生成式标识实体（默认排序类型为 <see cref="float"/>、状态类型为 <see cref="DataStatus"/>）。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public abstract class AbstractEntityWithGenId<TGenId> : AbstractEntity<TGenId>, IGenId<TGenId>
        where TGenId : IEquatable<TGenId>
    {
    }

    /// <summary>
    /// 抽象生成式标识实体。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntityWithGenId<TGenId, TRank, TStatus> : AbstractEntity<TGenId, TRank, TStatus>, IGenId<TGenId>
        where TGenId : IEquatable<TGenId>
        where TRank : struct
        where TStatus : struct
    {
    }
}
