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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Librame.Extensions.Data
{
    using Builders;
    using Services;

    /// <summary>
    /// 数据库上下文接口。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public interface IDbContext<TBuilderOptions> : IDbContext<Tenant, TBuilderOptions>
        where TBuilderOptions : class, IBuilderOptions, new()
    {
    }
    /// <summary>
    /// 数据库上下文接口。
    /// </summary>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public interface IDbContext<TTenant, TBuilderOptions> : IDbContext, IService<TBuilderOptions>
        where TTenant : class, ITenant
        where TBuilderOptions : class, IBuilderOptions, new()
    {
        /// <summary>
        /// 租户上下文。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ITenantContext{TTenant}"/>。
        /// </value>
        ITenantContext<TTenant> TenantContext { get; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{Tenant}"/>。
        /// </value>
        DbSet<TTenant> Tenants { get; }
    }


    /// <summary>
    /// 数据库上下文接口。
    /// </summary>
    public interface IDbContext : IDbProvider
    {
        /// <summary>
        /// 数据库。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DatabaseFacade"/>。
        /// </value>
        DatabaseFacade Database { get; }

        /// <summary>
        /// 变化跟踪器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker"/>。
        /// </value>
        ChangeTracker ChangeTracker { get; }

        /// <summary>
        /// 模型。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IModel"/>。
        /// </value>
        IModel Model { get; }


        /// <summary>
        /// 变化跟踪器上下文。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IChangeTrackerContext"/>。
        /// </value>
        IChangeTrackerContext TrackerContext { get; }


        /// <summary>
        /// 审计数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{Audit}"/>。
        /// </value>
        DbSet<Audit> Audits { get; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{AuditProperty}"/>。
        /// </value>
        DbSet<AuditProperty> AuditProperties { get; }

        /// <summary>
        /// 迁移审计数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{MigrationAudit}"/>。
        /// </value>
        DbSet<MigrationAudit> MigrationAudits { get; set; }


        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>
        /// 返回 <see cref="Tenant"/>。
        /// </value>
        ITenant CurrentTenant { get; }
    }
}
