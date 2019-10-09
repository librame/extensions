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
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 迁移服务。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class MigrationService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : AbstractExtensionBuilderService<DataBuilderOptions>
        , IMigrationService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        private static ConcurrentDictionary<string, List<string>> _migrationCommands
            = new ConcurrentDictionary<string, List<string>>();

        private static TMigration _lastMigration = null;


        /// <summary>
        /// 构造一个迁移服务。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public MigrationService(IMemoryLocker locker, IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            Locker = locker.NotNull(nameof(locker));
        }


        /// <summary>
        /// 内存锁定器。
        /// </summary>
        protected IMemoryLocker Locker { get; }


        /// <summary>
        /// 启用服务。
        /// </summary>
        public bool Enabled
            => Options.MigrationEnabled;


        /// <summary>
        /// 迁移。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        public void Migrate(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                MigrateCore(dbContextAccessor);
        }

        /// <summary>
        /// 迁移核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected virtual void MigrateCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            if (dbContextAccessor.Migrations.Exists())
            {
                if (_lastMigration.IsNull())
                    UpdateLastMigration(dbContextAccessor);

                // 对比差异
                var differences = GetDifferences(dbContextAccessor);
                if (differences.IsNotEmpty())
                {
                    var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
                    // 前置处理数据迁移
                    aspects.ForEach(aspect => aspect.Preprocess(dbContextAccessor));

                    // 差异迁移
                    MigrateDifference(dbContextAccessor, differences);

                    // 后置处理数据迁移
                    aspects.ForEach(aspect => aspect.Postprocess(dbContextAccessor));

                    // 如果需要保存更改
                    if (aspects.Any(aspect => aspect.RequiredSaveChanges))
                    {
                        dbContextAccessor.SaveChanges();
                        aspects.ForEach(aspect => aspect.RequiredSaveChanges = false);

                        UpdateLastMigration(dbContextAccessor);
                    }
                }
            }
            else
            {
                var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
                // 前置处理数据迁移
                aspects.ForEach(aspect => aspect.Preprocess(dbContextAccessor));

                // 初次进行系统整体性迁移
                dbContextAccessor.Database.Migrate();

                // 后置处理数据迁移
                aspects.ForEach(aspect => aspect.Postprocess(dbContextAccessor));

                // 如果需要保存更改
                if (aspects.Any(aspect => aspect.RequiredSaveChanges))
                {
                    dbContextAccessor.SaveChanges();
                    aspects.ForEach(aspect => aspect.RequiredSaveChanges = false);
                }
            }
        }

        /// <summary>
        /// 更新最后一次迁移。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected virtual void UpdateLastMigration(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var entityType = dbContextAccessor.Model.FindEntityType(typeof(TMigration));
            var tableName = new TableNameSchema(entityType.GetTableName(), entityType.GetSchema());

            var migration = dbContextAccessor.Migrations.FirstOrDefaultByMax(s => s.CreatedTime, tableName);

            // 防止默认数据库为空
            if (migration.IsNotNull())
                _lastMigration = migration;
        }


        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public Task MigrateAsync(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                return MigrateCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步迁移核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual async Task MigrateCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            if (await dbContextAccessor.Migrations.ExistsAsync(cancellationToken).ConfigureAndResultAsync())
            {
                if (_lastMigration.IsNull())
                    UpdateLastMigration(dbContextAccessor);

                // 对比差异
                var differences = GetDifferences(dbContextAccessor);
                if (differences.IsNotEmpty())
                {
                    var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
                    // 前置处理数据迁移
                    aspects.ForEach(async aspect => await aspect.PreprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                    // 差异迁移
                    MigrateDifference(dbContextAccessor, differences);

                    // 后置处理数据迁移
                    aspects.ForEach(async aspect => await aspect.PostprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                    // 如果需要保存更改
                    if (aspects.Any(aspect => aspect.RequiredSaveChanges))
                    {
                        await dbContextAccessor.SaveChangesAsync(cancellationToken).ConfigureAndResultAsync();
                        aspects.ForEach(aspect => aspect.RequiredSaveChanges = false);

                        UpdateLastMigration(dbContextAccessor);
                    }
                }
            }
            else
            {
                var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
                // 前置处理数据迁移
                aspects.ForEach(async aspect => await aspect.PreprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                // 初次进行系统整体性迁移
                await dbContextAccessor.Database.MigrateAsync().ConfigureAndWaitAsync();

                // 后置处理数据迁移
                aspects.ForEach(async aspect => await aspect.PostprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                // 如果需要保存更改
                if (aspects.Any(aspect => aspect.RequiredSaveChanges))
                {
                    await dbContextAccessor.SaveChangesAsync(cancellationToken).ConfigureAndResultAsync();
                    aspects.ForEach(aspect => aspect.RequiredSaveChanges = false);
                }
            }
        }


        /// <summary>
        /// 迁移差异。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="operationDifferences">给定的迁移操作差异集合。</param>
        protected virtual void MigrateDifference(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor, IReadOnlyList<MigrationOperation> operationDifferences)
        {
            //var rawSqlCommandBuilder = dbContextAccessor.ServiceProvider.GetRequiredService<IRawSqlCommandBuilder>();
            var migrationsSqlGenerator = dbContextAccessor.InternalServiceProvider.GetRequiredService<IMigrationsSqlGenerator>();
            var migrationCommandExecutor = dbContextAccessor.InternalServiceProvider.GetRequiredService<IMigrationCommandExecutor>();
            var connection = dbContextAccessor.InternalServiceProvider.GetRequiredService<IRelationalConnection>();
            //var historyRepository = dbContextAccessor.ServiceProvider.GetRequiredService<IHistoryRepository>();
            //var insertCommand = rawSqlCommandBuilder.Build(historyRepository.GetInsertScript(new HistoryRow(migration.GetId(), ProductInfo.GetVersion())));

            var readOnlyCommands = migrationsSqlGenerator.Generate(operationDifferences, dbContextAccessor.Model);
            //.Concat(new[] { new MigrationCommand(insertCommand, _currentContext.Context, _commandLogger) })
            var executeCommands = new List<MigrationCommand>(readOnlyCommands);

            for (var i = 0; i < readOnlyCommands.Count; i++)
            {
                var command = readOnlyCommands[i];
                var commandKey = GetMigrationCommandKey(command);

                if (_migrationCommands.TryGetValue(commandKey, out List<string> connectionStrings)
                    && connectionStrings.Contains(dbContextAccessor.CurrentConnectionString))
                {
                    // 每个数据连接只执行一次相同命令
                    executeCommands.Remove(command);
                }
            }

            if (executeCommands.Count > 0)
            {
                migrationCommandExecutor.ExecuteNonQuery(executeCommands, connection);

                executeCommands.ForEach(command =>
                {
                    var commandKey = GetMigrationCommandKey(command);

                    _migrationCommands.AddOrUpdate(commandKey, key =>
                    {
                        return new List<string>
                        {
                            dbContextAccessor.CurrentConnectionString
                        };
                    },
                    (key, value) =>
                    {
                        value.Add(dbContextAccessor.CurrentConnectionString);
                        return value;
                    });
                });
            }
        }

        /// <summary>
        /// 获取迁移命令键名。
        /// </summary>
        /// <param name="command">给定的 <see cref="MigrationCommand"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetMigrationCommandKey(MigrationCommand command)
            => command.CommandText.Sha256Base64String();

        /// <summary>
        /// 获取差异迁移操作。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationOperation}"/>。</returns>
        protected virtual IReadOnlyList<MigrationOperation> GetDifferences(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            return Locker.WaitFactory(() =>
            {
                var lastModel = ResolveModel();
                var typeMappingSource = dbContextAccessor.InternalServiceProvider.GetRequiredService<IRelationalTypeMappingSource>();
                var migrationsAnnotations = dbContextAccessor.InternalServiceProvider.GetRequiredService<IMigrationsAnnotationProvider>();
                var changeDetector = dbContextAccessor.InternalServiceProvider.GetRequiredService<IChangeDetector>();
                var updateAdapterFactory = dbContextAccessor.InternalServiceProvider.GetRequiredService<IUpdateAdapterFactory>();
                var commandBatchPreparerDependencies = dbContextAccessor.InternalServiceProvider.GetRequiredService<CommandBatchPreparerDependencies>();

                var modelDiffer = new ResetMigrationsModelDiffer(typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory,
                    commandBatchPreparerDependencies);

                return modelDiffer.GetDifferences(lastModel, dbContextAccessor.Model);
            });
        }

        /// <summary>
        /// 解析模型。
        /// </summary>
        /// <returns>返回 <see cref="IModel"/>。</returns>
        protected IModel ResolveModel()
        {
            var assembly = Assembly.Load(_lastMigration.ModelBody.Decompress());
            var snapshotType = assembly.GetType(_lastMigration.ModelSnapshotName, throwOnError: true, ignoreCase: false);

            //var dbContextAttribute = snapshotType.GetCustomAttribute<DbContextAttribute>();
            var snapshot = snapshotType.EnsureCreate<ModelSnapshot>();
            return snapshot?.Model;
        }

    }
}
