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

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 增量式标识符接口。
    /// </summary>
    /// <typeparam name="TIncremId">指定的增量式标识类型（如整数型标识）。</typeparam>
    public interface IIncrementalIdentifier<TIncremId> : IIdentifier<TIncremId>
        where TIncremId : IEquatable<TIncremId>
    {
    }
}
