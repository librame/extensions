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
using System.Linq;

namespace Librame.Extensions.Data.Accessors
{
    using Data.Stores;

    /// <summary>
    /// 集成数据存储的数据库上下文访问器。
    /// </summary>
    public class DataDbContextAccessor : DataDbContextAccessor<Guid, int, Guid>, IDataAccessor
    {
        /// <summary>
        /// 构造一个集成数据存储的数据库上下文访问器。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        public DataDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }

    }


    /// <summary>
    /// 集成数据存储的数据库上下文访问器。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataDbContextAccessor<TGenId, TIncremId, TCreatedBy>
        : DataDbContextAccessor<DataAudit<TGenId, TCreatedBy>,
            DataAuditProperty<TIncremId, TGenId>,
            DataMigration<TGenId, TCreatedBy>,
            DataTabulation<TGenId, TCreatedBy>,
            DataTenant<TGenId, TCreatedBy>,
            TGenId, TIncremId, TCreatedBy>,
        IDataAccessor<TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个集成数据存储的数据库上下文访问器。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        protected DataDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }

    }


    /// <summary>
    /// 集成数据存储的数据库上下文访问器。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的实体类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
        : DbContextAccessorBase, IDataAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant>
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTabulation : DataTabulation<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个集成数据存储的数据库上下文访问器。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        protected DataDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }


        /// <summary>
        /// 存在任何数据集集合。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public override bool AnySets()
            => Audits.Any() && Migrations.Any() && Tabulations.Any() && Tenants.Any();


        /// <summary>
        /// 审计数据集。
        /// </summary>
        public DbSet<TAudit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        public DbSet<TAuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 迁移数据集。
        /// </summary>
        public DbSet<TMigration> Migrations { get; set; }

        /// <summary>
        /// 表格数据集。
        /// </summary>
        public DbSet<TTabulation> Tabulations { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        public DbSet<TTenant> Tenants { get; set; }


        /// <summary>
        /// 审计数据集。
        /// </summary>
        public DbSetManager<TAudit> AuditsManager
            => Audits.AsManager();

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        public DbSetManager<TAuditProperty> AuditPropertiesManager
            => AuditProperties.AsManager();

        /// <summary>
        /// 迁移数据集。
        /// </summary>
        public DbSetManager<TMigration> MigrationsManager
            => Migrations.AsManager();

        /// <summary>
        /// 表格数据集。
        /// </summary>
        public DbSetManager<TTabulation> TabulationsManager
            => Tabulations.AsManager();

        /// <summary>
        /// 租户数据集。
        /// </summary>
        public DbSetManager<TTenant> TenantsManager
            => Tenants.AsManager();


        /// <summary>
        /// 配置模型构建器核心。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected override void OnModelCreatingCore(ModelBuilder modelBuilder)
            => modelBuilder.ConfigureDataStores(this);

    }
}
