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
    /// 租户接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface ITenant<TId> : ITenant, IId<TId>
        where TId : IEquatable<TId>
    {
    }


    /// <summary>
    /// 租户接口。
    /// </summary>
    public interface ITenant : IConnectionStrings
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 主机。
        /// </summary>
        string Host { get; set; }
    }
}
