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
    /// 表架构选项。
    /// </summary>
    public class TableSchemaOptions
    {
        /// <summary>
        /// 审计工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> AuditFactory { get; set; }
            = type => type.AsInternalTableSchema();

        /// <summary>
        /// 审计属性工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> AuditPropertyFactory { get; set; }
            = type => type.AsInternalTableSchema();

        /// <summary>
        /// 租户工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> TenantFactory { get; set; }
            = type => type.AsInternalTableSchema();
    }
}
