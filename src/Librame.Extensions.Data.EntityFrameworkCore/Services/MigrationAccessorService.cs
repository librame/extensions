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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Services
{
    using Core.Builders;
    using Core.Combiners;
    using Core.Mediators;
    using Core.Services;
    using Data.Accessors;
    using Data.Aspects;
    using Data.Builders;
    using Data.Compilers;
    using Data.Mediators;
    using Data.Migrations;
    using Data.Stores;

    /// <summary>
    /// 迁移访问器服务。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class MigrationAccessorService<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
        : AbstractExtensionBuilderService<DataBuilderOptions>, IMigrationAccessorService
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTabulation : DataTabulation<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CoreBuilderOptions _coreOptions;


        /// <summary>
        /// 构造一个迁移服务。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="dependency">给定的 <see cref="DataBuilderDependency"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public MigrationAccessorService(IMemoryCache memoryCache, DataBuilderDependency dependency,
            ILoggerFactory loggerFactory)
            : base(dependency?.Options, loggerFactory)
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
            _coreOptions = dependency.GetRequiredDependency<CoreBuilderDependency>().Options;
        }


        /// <summary>
        /// 迁移访问器。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public virtual void Migrate(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                MigrateCore(dbContextAccessor);
        }

        /// <summary>
        /// 异步迁移访问器。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task MigrateAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                return MigrateCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }


        /// <summary>
        /// 迁移访问器核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void MigrateCore(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            var lastModel = ResolvePersistentModel(dbContextAccessor);
            if (lastModel.IsNotNull())
            {
                // 对比差异
                var modelDiffer = dbContextAccessor.GetService<IMigrationsModelDiffer>();
                var differences = modelDiffer.GetDifferences(lastModel, dbContextAccessor.Model);
                if (differences.IsNotEmpty())
                {
                    AddMigration(dbContextAccessor);

                    // 执行差异迁移
                    MigrateAspectServices(dbContextAccessor,
                        () => ExecuteMigrationCommands(dbContextAccessor, differences));
                }
            }
            else
            {
                // 数据库整体迁移
                MigrateAspectServices(dbContextAccessor, () =>
                {
                    dbContextAccessor.Database.Migrate();

                    AddMigration(dbContextAccessor);
                });
            }
        }

        /// <summary>
        /// 异步迁移访问器核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual async Task MigrateCoreAsync(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken)
        {
            var lastModel = ResolvePersistentModel(dbContextAccessor);
            if (lastModel.IsNotNull())
            {
                // 对比差异
                var modelDiffer = dbContextAccessor.GetService<IMigrationsModelDiffer>();
                var differences = modelDiffer.GetDifferences(lastModel, dbContextAccessor.Model);
                if (differences.IsNotEmpty())
                {
                    AddMigration(dbContextAccessor);

                    await MigrateAspectServicesAsync(dbContextAccessor, () =>
                    {
                        // 执行差异迁移
                        ExecuteMigrationCommands(dbContextAccessor, differences);
                    },
                    cancellationToken).ConfigureAwait();
                }
            }
            else
            {
                await MigrateAspectServicesAsync(dbContextAccessor, async () =>
                {
                    // 初始化整体迁移
                    await dbContextAccessor.Database.MigrateAsync().ConfigureAwait();

                    AddMigration(dbContextAccessor);
                },
                cancellationToken).ConfigureAwait();
            }
        }


        /// <summary>
        /// 迁移截面服务集合（支持写入分离与数据同步）。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="migrateStructureAction">给定的迁移结构动作。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void MigrateAspectServices(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            Action migrateStructureAction)
        {
            IServicesManager<IMigrateAccessorAspect> aspects = null;

            // 数据迁移支持写入连接（包括未启用读写分离的默认连接）// 或启用数据同步的默认与写入连接（数据同步改为在 AccessorBatchExecutor 底层实现）
            if (dbContextAccessor.IsWritingConnectionString()) // || dbContextAccessor.CurrentTenant.DataSynchronization
            {
                aspects = dbContextAccessor.GetService<IServicesManager<IMigrateAccessorAspect>>();
                aspects.ForEach(aspect =>
                {
                    if (aspect.Enabled)
                        aspect.PreProcess(dbContextAccessor); // 前置处理数据迁移
                });
            }

            // 结构迁移支持写入连接（包括未启用读写分离的默认连接）或启用结构同步的默认与写入连接
            if (dbContextAccessor.IsWritingConnectionString() || dbContextAccessor.CurrentTenant.StructureSynchronization)
                migrateStructureAction.Invoke();

            if (aspects.IsNotNull())
            {
                aspects.ForEach(aspect =>
                {
                    if (aspect.Enabled)
                        aspect.PostProcess(dbContextAccessor); // 后置处理数据迁移
                });
            }
        }

        /// <summary>
        /// 异步迁移核心截面（支持写入分离与数据同步）。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="migrateAction">给定的迁移动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual Task MigrateAspectServicesAsync(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            Action migrateAction, CancellationToken cancellationToken)
        {
            IServicesManager<IMigrateAccessorAspect> aspects = null;

            // 数据迁移支持写入连接（包括未启用读写分离的默认连接）// 或启用数据同步的默认与写入连接（数据同步改为在 AccessorBatchExecutor 底层实现）
            if (dbContextAccessor.IsWritingConnectionString()) // || dbContextAccessor.CurrentTenant.DataSynchronization
            {
                aspects = dbContextAccessor.GetService<IServicesManager<IMigrateAccessorAspect>>();
                aspects.ForEach(async aspect =>
                {
                    if (aspect.Enabled)
                        await aspect.PreProcessAsync(dbContextAccessor, cancellationToken).ConfigureAwait(); // 前置处理数据迁移
                });
            }

            // 结构迁移支持写入连接（包括未启用读写分离的默认连接）或启用结构同步的默认与写入连接
            if (dbContextAccessor.IsWritingConnectionString() || dbContextAccessor.CurrentTenant.StructureSynchronization)
                migrateAction.Invoke();

            if (aspects.IsNotNull())
            {
                aspects.ForEach(async aspect =>
                {
                    if (aspect.Enabled)
                        await aspect.PostProcessAsync(dbContextAccessor, cancellationToken).ConfigureAwait(); // 后置处理数据迁移
                });
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// 执行迁移命令集合。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="operationDifferences">给定的迁移操作差异集合。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void ExecuteMigrationCommands(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            IReadOnlyList<MigrationOperation> operationDifferences)
        {
            //var rawSqlCommandBuilder = dbContextAccessor.GetService<IRawSqlCommandBuilder>();
            var migrationsSqlGenerator = dbContextAccessor.GetService<IMigrationsSqlGenerator>();
            var migrationCommandExecutor = dbContextAccessor.GetService<IMigrationCommandExecutor>();
            var connection = dbContextAccessor.GetService<IRelationalConnection>();
            //var historyRepository = dbContextAccessor.GetService<IHistoryRepository>();
            //var insertCommand = rawSqlCommandBuilder.Build(historyRepository.GetInsertScript(new HistoryRow(migration.GetId(), ProductInfo.GetVersion())));

            ExtensionSettings.Preference.RunLocker(() =>
            {
                operationDifferences = FilterInvalidOperationDifferences(dbContextAccessor, operationDifferences);

                // 生成操作差异的迁移命令列表
                var differenceCommands = migrationsSqlGenerator.Generate(operationDifferences, dbContextAccessor.Model);
                //.Concat(new[] { new MigrationCommand(insertCommand, _currentContext.Context, _commandLogger) })

                // 过滤需要执行的迁移命令集合（为防止命令执行过程中出错）
                var executeCommands = MigrationCommandFiltrator.Filter(_memoryCache,
                    dbContextAccessor, differenceCommands, _coreOptions);

                if (executeCommands.Count > 0)
                {
                    migrationCommandExecutor.ExecuteNonQuery(executeCommands, connection);
                    MigrationCommandFiltrator.Save(_memoryCache, dbContextAccessor, _coreOptions);
                }
            });
        }

        /// <summary>
        /// 过滤无效的操作差异。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="operationDifferences">给定的迁移操作差异集合。</param>
        /// <returns>返回迁移操作差异集合。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual IReadOnlyList<MigrationOperation> FilterInvalidOperationDifferences
            (DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            IReadOnlyList<MigrationOperation> operationDifferences)
        {
            if (!operationDifferences.Any(p => p is CreateIndexOperation))
                return operationDifferences;

            // 过滤分表创建时，同时创建已存在索引的情况
            var shardingTables = dbContextAccessor.Model.GetEntityTypes()
                .Where(p => p.ClrType.IsDefined<ShardableAttribute>())
                .Select(s => s.GetTableDescriptor().ToString())
                .ToList();

            return operationDifferences.Where(p =>
            {
                // 如果当前是创建索引迁移操作且属于分表索引
                if (p is CreateIndexOperation create && shardingTables.Contains(create.Table))
                    return false; // FALSE 表示过滤此操作

                return true;
            })
            .ToList();
        }


        /// <summary>
        /// 解析持久化模型。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <returns>返回 <see cref="IModel"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual IModel ResolvePersistentModel
            (DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                var cacheKey = DbContextAccessorHelper.GetMigrationCacheKey(dbContextAccessor);

                return _memoryCache.GetOrCreate(cacheKey, entry =>
                {
                    Type snapshotType = null;

                    // 启用写入分离后，数据库可能会主从同步，因此尝试从数据库获取迁移数据不作连接限制
                    var lastMigration = dbContextAccessor.Migrations
                        .FirstOrDefaultByMax(s => s.CreatedTimeTicks);

                    if (lastMigration.IsNotNull())
                    {
                        var buffer = ModelSnapshotCompiler.RestoreAssembly(lastMigration.ModelBody);
                        var modelAssembly = Assembly.Load(buffer);
                        snapshotType = modelAssembly.GetType(lastMigration.ModelSnapshotName,
                            throwOnError: true, ignoreCase: false);
                    }

                    if (snapshotType.IsNotNull())
                        return snapshotType.EnsureCreate<ModelSnapshot>().Model;

                    return null;
                });
            });
        }


        /// <summary>
        /// 添加迁移实体。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual void AddMigration
            (DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            if (!dbContextAccessor.IsWritingConnectionString())
                return;
            
            (var body, var hash) = CreateModelSnapshot(dbContextAccessor, out var typeName);

            dbContextAccessor.MigrationsManager.TryAdd(p => p.ModelHash == hash,
                () =>
                {
                    var identifierGenerator = (IDataStoreIdentityGenerator<TGenId>)dbContextAccessor
                        .GetService<IStoreIdentityGenerator>();

                    var migration = ObjectExtensions.EnsureCreate<TMigration>();

                    migration.Id = identifierGenerator.GenerateMigrationId();

                    migration.PopulateCreation(identifierGenerator.Clock);

                    migration.AccessorName = dbContextAccessor.CurrentType.GetDisplayNameWithNamespace();
                    migration.ModelSnapshotName = typeName;
                    migration.ModelBody = body;
                    migration.ModelHash = hash;

                    return migration;
                },
                addedPost =>
                {
                    if (!dbContextAccessor.RequiredSaveChanges)
                        dbContextAccessor.RequiredSaveChanges = true;

                    // 移除当前缓存
                    var cacheKey = DbContextAccessorHelper.GetMigrationCacheKey(dbContextAccessor);
                    _memoryCache.Remove(cacheKey);

                    // 发送迁移通知
                    var mediator = dbContextAccessor.GetService<IMediator>();
                    mediator.Publish(new MigrationNotification<TMigration>
                    {
                        Migration = addedPost.Entity
                    })
                    .ConfigureAwaitCompleted();
                });
        }

        /// <summary>
        /// 创建模型快照。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="modelSnapshotTypeName">输出模型快照类型名称。</param>
        /// <returns>返回包含字节数组与哈希的元组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual (byte[] body, string hash) CreateModelSnapshot(DbContextAccessorBase dbContextAccessor,
            out TypeNameCombiner modelSnapshotTypeName)
        {
            var typeName = ModelSnapshotCompiler.GenerateTypeName(dbContextAccessor.CurrentType);

            var result = ExtensionSettings.Preference.RunLocker(() =>
            {
                return ModelSnapshotCompiler.CompileInMemory(dbContextAccessor,
                    dbContextAccessor.Model, typeName);
            });

            modelSnapshotTypeName = typeName;

            return result;
        }

    }
}
