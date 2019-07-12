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
    /// 抽象增量式标识实体（默认标识类型为 <see cref="int"/>、排序类型为 <see cref="float"/>、状态类型为 <see cref="DataStatus"/>）。
    /// </summary>
    public abstract class AbstractEntityWithIncremId : AbstractEntity<int>, IIncremId
    {
    }


    /// <summary>
    /// 抽象增量式标识实体（默认标识类型为 <see cref="int"/>）。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntityWithIncremId<TRank, TStatus> : AbstractEntity<int, TRank, TStatus>, IIncremId
        where TRank : struct
        where TStatus : struct
    {
    }


    /// <summary>
    /// 抽象增量式标识实体。
    /// </summary>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntityWithIncremId<TIncremId, TRank, TStatus>
        : AbstractEntity<TIncremId, TRank, TStatus>
        , IIncremId<TIncremId>
        where TIncremId : IEquatable<TIncremId>
        where TRank : struct
        where TStatus : struct
    {
    }
}
