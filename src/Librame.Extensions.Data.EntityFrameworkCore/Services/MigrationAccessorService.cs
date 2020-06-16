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
    using Core.Compilers;
    using Core.Services;
    using Data.Accessors;
    using Data.Aspects;
    using Data.Builders;
    using Data.Compilers;
    using Data.Migrations;
    using Data.Stores;

    /// <summary>
    /// 迁移访问器服务。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class MigrationAccessorService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>
        : AbstractExtensionBuilderService<DataBuilderOptions>, IMigrationAccessorService
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId, TCreatedBy>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CoreBuilderOptions _coreOptions;

        private bool _requiredCompileAssembly = false;
        private FilePathCombiner _persistenceAssemblyPath = null;


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


        private void CompileAssembly(DbContextAccessorBase dbContextAccessor)
        {
            if (_requiredCompileAssembly)
            {
                ExtensionSettings.Preference.RunLocker(() =>
                {
                    ModelSnapshotCompiler.CompileInFile(dbContextAccessor, dbContextAccessor.Model, Options);
                    _requiredCompileAssembly = false;

                    // 移除程序集缓存
                    _memoryCache.Remove(_persistenceAssemblyPath);
                });
            }
        }


        /// <summary>
        /// 迁移。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public void Migrate(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                MigrateCore(dbContextAccessor);
        }

        /// <summary>
        /// 迁移核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void MigrateCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            _persistenceAssemblyPath = ModelSnapshotCompiler.CombineFilePath(dbContextAccessor);

            var lastModel = ResolvePersistenceModel(dbContextAccessor);
            if (lastModel.IsNotNull())
            {
                // 对比差异
                var modelDiffer = dbContextAccessor.GetService<IMigrationsModelDiffer>();
                var differences = modelDiffer.GetDifferences(lastModel, dbContextAccessor.Model);
                if (differences.IsNotEmpty())
                {
                    // 迁移结构差异
                    MigrateAspectServices(dbContextAccessor,
                        () => MigrateStructureDifference(dbContextAccessor, differences));
                }
            }
            else
            {
                // 数据库整体迁移
                MigrateAspectServices(dbContextAccessor, () =>
                {
                    dbContextAccessor.Database.Migrate();
                    _requiredCompileAssembly = true;
                });
            }

            CompileAssembly(dbContextAccessor);
        }


        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public Task MigrateAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                return MigrateCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步迁移核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual async Task MigrateCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            _persistenceAssemblyPath = ModelSnapshotCompiler.CombineFilePath(dbContextAccessor);

            var lastModel = ResolvePersistenceModel(dbContextAccessor);
            if (lastModel.IsNotNull())
            {
                // 对比差异
                var modelDiffer = dbContextAccessor.GetService<IMigrationsModelDiffer>();
                var differences = modelDiffer.GetDifferences(lastModel, dbContextAccessor.Model);
                if (differences.IsNotEmpty())
                {
                    await MigrateAspectServicesAsync(dbContextAccessor, () =>
                    {
                        // 迁移结构差异
                        MigrateStructureDifference(dbContextAccessor, differences);
                    },
                    cancellationToken).ConfigureAndWaitAsync();
                }
            }
            else
            {
                await MigrateAspectServicesAsync(dbContextAccessor, async () =>
                {
                    // 数据库整体迁移
                    await dbContextAccessor.Database.MigrateAsync().ConfigureAndWaitAsync();
                    _requiredCompileAssembly = true;
                },
                cancellationToken).ConfigureAndWaitAsync();
            }

            CompileAssembly(dbContextAccessor);
        }


        #region MigrateAspectServices

        /// <summary>
        /// 迁移截面服务集合。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="migrateStructureAction">给定的迁移结构动作。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void MigrateAspectServices(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            Action migrateStructureAction)
        {
            IServicesManager<IMigrateAccessorAspect<TGenId, TCreatedBy>> aspects = null;

            // 数据迁移支持写入连接（包括未启用读写分离的默认连接）//或启用数据同步的默认与写入连接
            if (dbContextAccessor.IsWritingConnectionString()) //|| dbContextAccessor.CurrentTenant.DataSynchronization
            {
                aspects = dbContextAccessor.GetService<IServicesManager<IMigrateAccessorAspect<TGenId, TCreatedBy>>>();

                aspects.ForEach(aspect =>
                {
                    if (aspect.Enabled)
                        aspect.PreProcess(dbContextAccessor); // 前置处理数据迁移
                });
            }

            // 结构迁移支持写入连接（包括未启用读写分离的默认连接）或启用结构同步的默认与写入连接
            if (dbContextAccessor.IsWritingConnectionString() || dbContextAccessor.CurrentTenant.StructureSynchronization)
                migrateStructureAction.Invoke();

            if (aspects.IsNotEmpty())
            {
                aspects.ForEach(aspect =>
                {
                    if (aspect.Enabled)
                        aspect.PostProcess(dbContextAccessor); // 后置处理数据迁移
                });

                // 如果需要保存更改
                if (aspects.Any(aspect => aspect.RequiredSaveChanges))
                {
                    dbContextAccessor.BaseSaveChanges();
                    aspects.ForEach(aspect => aspect.RequiredSaveChanges = false);
                }
            }
        }

        /// <summary>
        /// 异步迁移核心截面。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="migrateAction">给定的迁移动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual async Task MigrateAspectServicesAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            Action migrateAction, CancellationToken cancellationToken = default)
        {
            IServicesManager<IMigrateAccessorAspect<TGenId, TCreatedBy>> aspects = null;

            // 数据迁移支持写入连接（包括未启用读写分离的默认连接）//或启用数据同步的默认与写入连接
            if (dbContextAccessor.IsWritingConnectionString()) //|| dbContextAccessor.CurrentTenant.DataSynchronization
            {
                aspects = dbContextAccessor.GetService<IServicesManager<IMigrateAccessorAspect<TGenId, TCreatedBy>>>();

                aspects.ForEach(async aspect =>
                {
                    if (aspect.Enabled)
                        await aspect.PreProcessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync(); // 前置处理数据迁移
                });
            }

            // 结构迁移支持写入连接（包括未启用读写分离的默认连接）或启用结构同步的默认与写入连接
            if (dbContextAccessor.IsWritingConnectionString() || dbContextAccessor.CurrentTenant.StructureSynchronization)
                migrateAction.Invoke();

            if (aspects.IsNotEmpty())
            {
                aspects.ForEach(async aspect =>
                {
                    if (aspect.Enabled)
                        await aspect.PostProcessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync(); // 后置处理数据迁移
                });

                // 如果需要保存更改
                if (aspects.Any(aspect => aspect.RequiredSaveChanges))
                {
                    await dbContextAccessor.BaseSaveChangesAsync(cancellationToken).ConfigureAndWaitAsync();
                    aspects.ForEach(aspect => aspect.RequiredSaveChanges = false);
                }
            }
        }

        #endregion


        /// <summary>
        /// 迁移结构差异。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="operationDifferences">给定的迁移操作差异集合。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void MigrateStructureDifference(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
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
                // 生成操作差异的迁移命令列表
                var differenceCommands = migrationsSqlGenerator.Generate(operationDifferences, dbContextAccessor.Model);
                //.Concat(new[] { new MigrationCommand(insertCommand, _currentContext.Context, _commandLogger) })

                // 过滤需要执行的迁移命令集合（为防止命令执行过程中出错）
                var executeCommands = MigrationCommandFiltrator.Filter(_memoryCache,
                    dbContextAccessor, differenceCommands, Options, _coreOptions);
                if (executeCommands.Count > 0)
                {
                    migrationCommandExecutor.ExecuteNonQuery(executeCommands, connection);
                    _requiredCompileAssembly = true;

                    MigrationCommandFiltrator.Save(_memoryCache, dbContextAccessor, _coreOptions);
                }
            });
        }

        /// <summary>
        /// 解析持久化模型。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <returns>返回 <see cref="IModel"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected IModel ResolvePersistenceModel(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                return _memoryCache.GetOrCreate(GetCacheKey(), entry =>
                {
                    Type snapshotType = null;

                    if (_persistenceAssemblyPath.Exists())
                    {
                        // 从生成的模型快照程序集文件中加载
                        var modelAssembly = CSharpCompiler.DecompileFromFile(_persistenceAssemblyPath);
                        var modelSnapshotTypeName = ModelSnapshotCompiler.GenerateTypeName(dbContextAccessor.CurrentType);
                        snapshotType = modelAssembly.GetType(modelSnapshotTypeName, throwOnError: true, ignoreCase: false);
                    }
                    else //if (dbContextAccessor.IsWritingConnectionString())
                    {
                        // 启用写入分离后，数据库可能会主从同步，因此尝试从数据库获取迁移数据不作连接限制
                        var lastMigration = dbContextAccessor.Migrations.FirstOrDefaultByMax(s => s.CreatedTimeTicks);
                        if (lastMigration.IsNotNull() && lastMigration.ModelBody.IsNotEmpty())
                        {
                            var buffer = ModelSnapshotCompiler.RestoreAssembly(lastMigration.ModelBody);
                            var modelAssembly = Assembly.Load(buffer);
                            snapshotType = modelAssembly.GetType(lastMigration.ModelSnapshotName, throwOnError: true, ignoreCase: false);
                        }

                        _requiredCompileAssembly = true;
                    }

                    if (snapshotType.IsNotNull())
                        return snapshotType.EnsureCreate<ModelSnapshot>().Model;

                    return null;
                });
            });
        }

        /// <summary>
        /// 获取缓存键。
        /// </summary>
        /// <returns>返回字符串。</returns>
        protected string GetCacheKey()
            => $"{nameof(MigrationAccessorService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>)}:{_persistenceAssemblyPath}";

    }
}
