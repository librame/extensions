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
    /// 增量式标识接口（默认标识类型为 <see cref="int"/>）。
    /// </summary>
    public interface IIncremId : IIncremId<int>
    {
    }


    /// <summary>
    /// 增量式标识接口。
    /// </summary>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public interface IIncremId<TIncremId> : IId<TIncremId>
        where TIncremId : IEquatable<TIncremId>
    {
    }
}
