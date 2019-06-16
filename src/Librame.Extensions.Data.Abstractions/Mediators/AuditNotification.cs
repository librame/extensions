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
    using Core;

    /// <summary>
    /// 审计通知。
    /// </summary>
    public class AuditNotification : INotification
    {
        /// <summary>
        /// 审计集合。
        /// </summary>
        public List<BaseAudit> Audits { get; set; }
    }
}
