#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 租户标识接口。
    /// </summary>
    public interface ITenantId
    {
        /// <summary>
        /// 租户标识。
        /// </summary>
        string TenantId { get; set; }
    }
}
