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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core.Mediators;
    using Data.Accessors;
    using Data.Compilers;
    using Data.Mediators;
    using Data.Stores;

    /// <summary>
    /// 数据迁移迁移数据库上下文访问器截面。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class DataMigrationMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        : DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        , IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个数据迁移迁移数据库上下文访问器截面。
        /// </summary>
        /// <param name="dependencies">给定的 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。</param>
        public DataMigrationMigrateDbContextAccessorAspect(DbContextAccessorAspectDependencies<TGenId> dependencies)
            : base(dependencies, priority: 1) // 迁移优先级最高
        {
        }


        /// <summary>
        /// 需要保存更改。
        /// </summary>
        public bool RequiredSaveChanges { get; set; }


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.MigrationEnabled;


        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override void PostProcessCore
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var currentMigration = GenerateMigration(dbContextAccessor);
            var lastMigration = dbContextAccessor.Migrations.FirstOrDefaultByMax(s => s.CreatedTimeTicks);

            if (lastMigration.IsNull() || !currentMigration.Equals(lastMigration))
            {
                dbContextAccessor.Migrations.Add(currentMigration);
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(new MigrationNotification<TMigration, TGenId> { Migration = currentMigration }).ConfigureAndWait();
            }
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override async Task PostProcessCoreAsync
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var currentMigration = GenerateMigration(dbContextAccessor, cancellationToken);
            var lastMigration = await dbContextAccessor.Migrations.FirstOrDefaultByMaxAsync(s => s.CreatedTimeTicks).ConfigureAndResultAsync();

            if (lastMigration.IsNull() || !currentMigration.Equals(lastMigration))
            {
                await dbContextAccessor.Migrations.AddAsync(currentMigration, cancellationToken).ConfigureAndResultAsync();
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.GetService<IMediator>();
                await mediator.Publish(new MigrationNotification<TMigration, TGenId> { Migration = currentMigration }).ConfigureAndWaitAsync();
            }
        }


        /// <summary>
        /// 生成数据迁移。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <typeparamref name="TMigration"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual TMigration GenerateMigration
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var createdTime = Dependencies.Clock.GetOffsetNowAsync(cancellationToken: cancellationToken).ConfigureAndResult();
            var id = Dependencies.Identifier.GetMigrationIdAsync(cancellationToken).ConfigureAndResult();

            return ExtensionSettings.Current.RunLockerResult(() =>
            {
                var modelSnapshotTypeName = ModelSnapshotCompiler.GenerateTypeName(dbContextAccessor.CurrentType);
                var modelSnapshot = ModelSnapshotCompiler.CompileInMemory(dbContextAccessor,
                    dbContextAccessor.Model, Options, modelSnapshotTypeName);

                var migration = typeof(TMigration).EnsureCreate<TMigration>();
                migration.Id = id;
                migration.AccessorName = dbContextAccessor.CurrentType.GetDisplayNameWithNamespace();
                migration.ModelSnapshotName = modelSnapshotTypeName;
                migration.ModelBody = modelSnapshot.Body;
                migration.ModelHash = modelSnapshot.Hash;
                migration.CreatedTime = createdTime;
                migration.CreatedTimeTicks = migration.CreatedTime.Ticks;
                migration.CreatedBy = EntityPopulator.FormatTypeName(GetType());

                return migration;
            });
        }

    }
}
