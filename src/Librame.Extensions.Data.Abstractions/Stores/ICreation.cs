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
    /// 创建接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface ICreation<TId, TDateTime>
        where TId : IEquatable<TId>
        where TDateTime : struct
    {
        /// <summary>
        /// 创建时间。
        /// </summary>
        TDateTime CreateTime { get; set; }

        /// <summary>
        /// 创建者标识。
        /// </summary>
        TId CreatorId { get; set; }
    }
}
