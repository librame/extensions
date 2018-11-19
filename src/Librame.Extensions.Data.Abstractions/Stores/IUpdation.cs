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
    /// 更新接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface IUpdation<TId, TDateTime>
        where TId : IEquatable<TId>
        where TDateTime : struct
    {
        /// <summary>
        /// 更新时间。
        /// </summary>
        TDateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新者标识。
        /// </summary>
        TId UpdatorId { get; set; }
    }
}
