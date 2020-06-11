#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 访问器泛型类型参数集合。
    /// </summary>
    public class AccessorGenericTypeArguments
    {
        /// <summary>
        /// 审计类型。
        /// </summary>
        public Type AuditType { get; set; }

        /// <summary>
        /// 审计属性类型。
        /// </summary>
        public Type AuditPropertyType { get; set; }

        /// <summary>
        /// 实体类型。
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 迁移类型。
        /// </summary>
        public Type MigrationType { get; set; }

        /// <summary>
        /// 租户类型。
        /// </summary>
        public Type TenantType { get; set; }


        /// <summary>
        /// 生成式标识类型。
        /// </summary>
        public Type GenIdType { get; set; }

        /// <summary>
        /// 增量式标识类型。
        /// </summary>
        public Type IncremIdType { get; set; }

        /// <summary>
        /// 创建者类型（同更新者、发表者类型）。
        /// </summary>
        public Type CreatedByType { get; set; }

        /// <summary>
        /// 创建日期类型（同更新日期、发表日期类型）。
        /// </summary>
        public Type CreatedTimeType { get; set; }
    }
}
