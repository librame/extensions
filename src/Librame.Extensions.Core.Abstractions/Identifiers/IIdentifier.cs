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
    /// 标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
    public interface IIdentifier<TId> : IObjectIdentifier
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识。
        /// </summary>
        TId Id { get; set; }
    }
}
