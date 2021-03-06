﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;

    /// <summary>
    /// 审计通知。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    public class AuditNotification<TAudit, TAuditProperty> : INotificationIndication
        where TAudit : class
        where TAuditProperty : class
    {
        /// <summary>
        /// 构造一个审计通知。
        /// </summary>
        /// <param name="audits">给定的审计集合。</param>
        public AuditNotification(IDictionary<TAudit, List<TAuditProperty>> audits)
        {
            Audits = audits.NotEmpty(nameof(audits));
        }


        /// <summary>
        /// 审计集合。
        /// </summary>
        public IDictionary<TAudit, List<TAuditProperty>> Audits { get; }
    }
}
