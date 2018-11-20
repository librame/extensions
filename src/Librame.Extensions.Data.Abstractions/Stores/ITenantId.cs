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
    /// 租户标识接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface ITenantId<TId> : ITenantId
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 租户标识。
        /// </summary>
        TId TenantId { get; set; }
    }


    /// <summary>
    /// 租户标识接口。
    /// </summary>
    public interface ITenantId
    {
    }

}
