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
using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 审计。
    /// </summary>
    [NotAudited]
    public class Audit : AbstractId<int>
    {
        /// <summary>
        /// 实体标识。
        /// </summary>
        public virtual string EntityId { get; set; }

        /// <summary>
        /// 实体名。
        /// </summary>
        public virtual string EntityName { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public virtual int State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        public virtual string StateName { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }

        /// <summary>
        /// 实体属性集合。
        /// </summary>
        public virtual IList<AuditProperty> Properties { get; set; }
            = new List<AuditProperty>();
    }
}
