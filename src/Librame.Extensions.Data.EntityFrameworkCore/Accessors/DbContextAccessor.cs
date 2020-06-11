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
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DbContextAccessor<TGenId, TIncremId, TCreatedBy>
        : DbContextAccessor<DataAudit<TGenId, TCreatedBy>, DataAuditProperty<TIncremId, TGenId>,
            DataEntity<TGenId, TCreatedBy>, DataMigration<TGenId, TCreatedBy>, DataTenant<TGenId, TCreatedBy>,
            TGenId, TIncremId, TCreatedBy>, IDbContextAccessor<TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
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
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>
        : DbContextAccessorBase, IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId, TCreatedBy>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
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
            => modelBuilder.ConfigureDataStores(this);


        #region SaveChangesCore (ISaveChangesDbContextAccessorAspect)

        /// <summary>
        /// 重载保存更改核心。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        protected override int SaveChangesCore(bool acceptAllChangesOnSuccess)
        {
            var aspects = GetService<IServicesManager<ISaveChangesAccessorAspect<TGenId, TCreatedBy>>>();
            aspects.ForEach(aspect =>
            {
                if (aspect.Enabled)
                    aspect.PreProcess(this); // 前置处理保存变化
            });

            // 保存更改
            var count = BaseSaveChanges(acceptAllChangesOnSuccess);

            aspects.ForEach(aspect =>
            {
                if (aspect.Enabled)
                    aspect.PostProcess(this); // 后置处理保存变化
            });

            return count;
        }

        /// <summary>
        /// 重载异步保存更改核心。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        protected override async Task<int> SaveChangesCoreAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var aspects = GetService<IServicesManager<ISaveChangesAccessorAspect<TGenId, TCreatedBy>>>();
            aspects.ForEach(async aspect =>
            {
                if (aspect.Enabled)
                    await aspect.PreProcessAsync(this, cancellationToken).ConfigureAndWaitAsync(); // 异步前置处理保存变化
            });

            // 异步保存更改
            var count = await BaseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAndResultAsync();

            aspects.ForEach(async aspect =>
            {
                if (aspect.Enabled)
                    await aspect.PostProcessAsync(this, cancellationToken).ConfigureAndWaitAsync(); // 异步后置处理保存变化
            });

            return count;
        }

        #endregion


        #region MigrateCore (IMigrationService, Invoke IMigrateDbContextAccessorAspect)

        /// <summary>
        /// 迁移。
        /// </summary>
        protected override void MigrateCore()
        {
            var migration = GetService<IMigrationAccessorService>();
            migration.Migrate(this);
        }

        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected override Task MigrateCoreAsync(CancellationToken cancellationToken = default)
        {
            var migration = GetService<IMigrationAccessorService>();
            return migration.MigrateAsync(this, cancellationToken);
        }

        #endregion


        #region SwitchTenant (ITenantService)

        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected override ITenant SwitchTenant()
        {
            var tenant = GetService<IMultiTenantAccessorService>();
            return tenant.GetSwitchTenant(this);
        }

        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含 <see cref="ITenant"/> 的异步操作。</returns>
        protected override Task<ITenant> SwitchTenantAsync(CancellationToken cancellationToken = default)
        {
            var tenant = GetService<IMultiTenantAccessorService>();
            return tenant.GetSwitchTenantAsync(this, cancellationToken);
        }

        #endregion

    }
}
