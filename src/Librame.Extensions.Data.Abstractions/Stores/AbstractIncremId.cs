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
    /// 抽象增量式标识（默认标识类型为 <see cref="int"/>）。
    /// </summary>
    public abstract class AbstractIncremId : AbstractIncremId<int>, IIncremId
    {
    }


    /// <summary>
    /// 抽象增量式标识。
    /// </summary>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public abstract class AbstractIncremId<TIncremId> : AbstractId<TIncremId>, IIncremId<TIncremId>
        where TIncremId : IEquatable<TIncremId>
    {
    }
}
