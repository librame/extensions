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
    /// 租户变化处理程序接口。
    /// </summary>
    public interface ITenantChangeHandler : IChangeHandler
    {
        /// <summary>
        /// 设置租户。
        /// </summary>
        ITenant SetTenant { set; }
    }
}
