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
    /// 标识接口。
    /// </summary>
    public interface IId : IId<string>
    {
    }


    /// <summary>
    /// 标识接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IId<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识。
        /// </summary>
        TId Id { get; set; }
    }
}
