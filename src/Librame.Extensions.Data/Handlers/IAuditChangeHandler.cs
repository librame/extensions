#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 审计变化处理程序接口。
    /// </summary>
    public interface IAuditChangeHandler : IChangeHandler
    {
        /// <summary>
        /// 变化的审计实体列表。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IList{Audit}"/>。
        /// </value>
        IList<Audit> ChangeAudits { get; }
    }
}
