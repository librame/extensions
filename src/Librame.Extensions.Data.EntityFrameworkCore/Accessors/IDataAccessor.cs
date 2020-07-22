#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Accessors
{
    using Data.Builders;
    using Data.Stores;

    /// <summary>
    /// 数据访问器接口。
    /// </summary>
    public interface IDataAccessor : IDataAccessor<Guid, int, Guid>
    {
    }


    /// <summary>
    /// 数据访问器接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public interface IDataAccessor<TGenId, TIncremId, TCreatedBy>
        : IDataAccessor<DataAudit<TGenId, TCreatedBy>,
            DataAuditProperty<TIncremId, TGenId>,
            DataMigration<TGenId, TCreatedBy>,
            DataTabulation<TGenId, TCreatedBy>,
            DataTenant<TGenId, TCreatedBy>>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
    }


    /// <summary>
    /// 数据访问器接口。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的实体类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface IDataAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant> : IAccessor
        where TAudit : class
        where TAuditProperty : class
        where TMigration : class
        where TTabulation : class
        where TTenant : class
    {
        /// <summary>
        /// 审计数据集。
        /// </summary>
        DbSet<TAudit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        DbSet<TAuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 迁移数据集。
        /// </summary>
        DbSet<TMigration> Migrations { get; set; }

        /// <summary>
        /// 表格数据集。
        /// </summary>
        DbSet<TTabulation> Tabulations { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        DbSet<TTenant> Tenants { get; set; }


        /// <summary>
        /// 审计数据集管理器。
        /// </summary>
        DbSetManager<TAudit> AuditsManager { get; }

        /// <summary>
        /// 审计属性数据集管理器。
        /// </summary>
        DbSetManager<TAuditProperty> AuditPropertiesManager { get; }

        /// <summary>
        /// 迁移数据集管理器。
        /// </summary>
        DbSetManager<TMigration> MigrationsManager { get; }

        /// <summary>
        /// 表格数据集管理器。
        /// </summary>
        DbSetManager<TTabulation> TabulationsManager { get; }

        /// <summary>
        /// 租户数据集管理器。
        /// </summary>
        DbSetManager<TTenant> TenantsManager { get; }


        /// <summary>
        /// 构建器依赖。
        /// </summary>
        DataBuilderDependency Dependency { get; }
    }
}
