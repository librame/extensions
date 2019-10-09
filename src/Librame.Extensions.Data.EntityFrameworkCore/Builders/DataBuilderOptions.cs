#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : DataBuilderOptionsBase
    {
        /// <summary>
        /// 是否创建数据库（如果数据库不存在；默认已启用）。
        /// </summary>
        public bool IsCreateDatabase { get; set; }
            = true;

        /// <summary>
        /// 已创建数据库动作。
        /// </summary>
        public Action<IAccessor> DatabaseCreatedAction { get; set; }


        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; }
            = true;

        /// <summary>
        /// 审计实体状态数组（默认对实体的增加、修改、删除状态进行审核）。
        /// </summary>
        public EntityState[] AuditEntityStates { get; set; }
            = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };


        /// <summary>
        /// 启用实体注册（默认已启用）。
        /// </summary>
        public bool EntityEnabled { get; set; }
            = true;


        /// <summary>
        /// 启用迁移（默认已启用）。
        /// </summary>
        public bool MigrationEnabled { get; set; }
            = true;

        /// <summary>
        /// 迁移程序集引用列表。
        /// </summary>
        public List<AssemblyReference> MigrationAssemblyReferences { get; }
            = new List<AssemblyReference>
            {
                AssemblyReference.ByName("Librame.Extensions.Data.Abstractions"),
                AssemblyReference.ByName("Librame.Extensions.Data.EntityFrameworkCore"),
                AssemblyReference.ByName("Microsoft.EntityFrameworkCore"),
                AssemblyReference.ByName("Microsoft.EntityFrameworkCore.Relational"),
                AssemblyReference.ByPath(@"C:\Program Files\dotnet\packs\NETStandard.Library.Ref\2.1.0\ref\netstandard2.1\netstandard.dll")
            };

        /// <summary>
        /// 导出模型快照文件路径（不能位于 BIN 目录，否则会抛出被另一进程占用的异常）。
        /// </summary>
        public string ExportModelSnapshotFilePath { get; set; }
            = @"Librame.Extensions.Data.EntityFrameworkCore.ModelSnapshot.dll";


        /// <summary>
        /// 数据租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; }
            = true;

        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }
            = new DataTenant<string>
            {
                Name = "DefaultTenant",
                Host = "localhost",
                DefaultConnectionString = "librame_data_default",
                WritingConnectionString = "librame_data_writing",
            };
    }
}
