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

namespace Librame.Extensions.Data.Accessors
{
    using Builders;
    using Stores;

    /// <summary>
    /// 数据库上下文访问器接口。
    /// </summary>
    public interface IDbContextAccessor : IDbContextAccessor<DataAudit<string>, DataAuditProperty<int, string>, DataEntity<string>, DataMigration<string>, DataTenant<string>>
    {
    }


    /// <summary>
    /// 数据库上下文访问器接口。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> : IAccessor
        where TAudit : class
        where TAuditProperty : class
        where TEntity : class
        where TMigration : class
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
        /// 实体数据集。
        /// </summary>
        DbSet<TEntity> Entities { get; set; }

        /// <summary>
        /// 迁移数据集。
        /// </summary>
        DbSet<TMigration> Migrations { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        DbSet<TTenant> Tenants { get; set; }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        DataBuilderOptions BuilderOptions { get; }
    }
}
