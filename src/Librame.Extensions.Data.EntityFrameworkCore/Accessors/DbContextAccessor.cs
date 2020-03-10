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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    using Core.Services;
    using Data.Aspects;
    using Data.Services;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器。
    /// </summary>
    public class DbContextAccessor : DbContextAccessor<DataAudit<string>, DataAuditProperty<int, string>, DataEntity<string>, DataMigration<string>, DataTenant<string>, string, int>, IDbContextAccessor
    {
        /// <summary>
        /// 构造一个数据库上下文访问器。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        public DbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }
    }


    /// <summary>
    /// 数据库上下文访问器。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : DbContextAccessorBase, IDbContextAccessorFlag
        , IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个数据库上下文访问器。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        public DbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }


        #region Entities

        /// <summary>
        /// 审计数据集。
        /// </summary>
        public DbSet<TAudit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        public DbSet<TAuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 实体数据集。
        /// </summary>
        public DbSet<TEntity> Entities { get; set; }

        /// <summary>
        /// 迁移数据集。
        /// </summary>
        public DbSet<TMigration> Migrations { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        public DbSet<TTenant> Tenants { get; set; }

        #endregion


        /// <summary>
        /// 开始模型创建。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>(BuilderOptions);
        }


        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // 改变为写入数据库连接
            var isChanged = ChangeDbConnection(tenant => tenant.WritingConnectionString);

            var aspects = ServiceFactory.GetRequiredService<IServicesManager<ISaveChangesDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
            // 前置处理保存变化
            aspects.ForEach(aspect => aspect.Preprocess(this));

            // 保存变化
            var count = base.SaveChanges(acceptAllChangesOnSuccess);

            // 后置处理保存变化
            aspects.ForEach(aspect => aspect.Postprocess(this));

            // 还原为默认数据库连接
            if (isChanged)
                ChangeDbConnection(tenant => tenant.DefaultConnectionString);

            return count;
        }

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            // 改变为写入数据库连接
            var isChanged = await ChangeDbConnectionAsync(tenant => tenant.WritingConnectionString, cancellationToken).ConfigureAndResultAsync();

            var aspects = ServiceFactory.GetRequiredService<IServicesManager<ISaveChangesDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
            // 前置处理保存变化
            aspects.ForEach(async aspect => await aspect.PreprocessAsync(this, cancellationToken).ConfigureAndWaitAsync());

            // 保存变化
            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAndResultAsync();

            // 后置处理保存变化
            aspects.ForEach(async aspect => await aspect.PostprocessAsync(this, cancellationToken).ConfigureAndWaitAsync());

            // 还原为默认数据库连接
            if (isChanged)
                await ChangeDbConnectionAsync(tenant => tenant.DefaultConnectionString, cancellationToken: cancellationToken).ConfigureAndResultAsync();

            return count;
        }


        /// <summary>
        /// 迁移。
        /// </summary>
        public override void Migrate()
        {
            var migrationService = this.GetService<IMigrationService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>();
            migrationService.Migrate(this);
        }

        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public override Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            var migrationService = this.GetService<IMigrationService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>();
            return migrationService.MigrateAsync(this, cancellationToken);
        }


        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected override ITenant GetSwitchTenant()
        {
            var tenantService = this.GetService<ITenantService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>();
            return tenantService.GetSwitchTenant(this);
        }

        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含 <see cref="ITenant"/> 的异步操作。</returns>
        protected override Task<ITenant> GetSwitchTenantAsync(CancellationToken cancellationToken = default)
        {
            var tenantService = this.GetService<ITenantService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>();
            return tenantService.GetSwitchTenantAsync(this, cancellationToken);
        }

    }
}
