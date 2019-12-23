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

namespace Librame.Extensions.Data.Schemas
{
    /// <summary>
    /// 表名架构选项。
    /// </summary>
    public class TableNameSchemaOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 默认连接符。
        /// </summary>
        public string DefaultConnector { get; set; }


        /// <summary>
        /// 内部表名前缀（如：Internal）。
        /// </summary>
        public string InternalPrefix { get; set; }
            = "Internal";

        /// <summary>
        /// 私有表名前缀（如：_）。
        /// </summary>
        public string PrivatePrefix { get; set; }
            = "_";


        /// <summary>
        /// 审计工厂方法。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<TableNameDescriptor, TableNameSchema> AuditFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 审计属性工厂方法。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<TableNameDescriptor, TableNameSchema> AuditPropertyFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 实体工厂方法。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<TableNameDescriptor, TableNameSchema> EntityFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 迁移工厂方法。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<TableNameDescriptor, TableNameSchema> MigrationFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 租户工厂方法。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<TableNameDescriptor, TableNameSchema> TenantFactory { get; set; }
            = descr => descr.AsSchema();
    }
}
