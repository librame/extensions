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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Services
{
    using Accessors;
    using Aspects;
    using Builders;
    using Compilers;
    using Core.Builders;
    using Core.Combiners;
    using Core.Services;
    using Core.Threads;
    using Migrations;
    using Stores;

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
        private static IModel _defaultModel = null;

        private readonly CoreBuilderOptions _coreOptions;


        /// <summary>
        /// 构造一个迁移服务。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public MigrationService(IMemoryLocker locker, IOptions<CoreBuilderOptions> coreOptions,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            Locker = locker.NotNull(nameof(locker));
            _coreOptions = coreOptions.NotNull(nameof(coreOptions)).Value;
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected virtual void MigrateCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var lastModel = ResolveLastModel(dbContextAccessor);
            if (lastModel.IsNotNull())
            {
                // 对比差异
                var differences = GetDifferences(dbContextAccessor.InternalServiceProvider, lastModel, dbContextAccessor.Model);
                if (differences.IsNotEmpty())
                {
                    // 差异迁移
                    MigrateCoreAspect(dbContextAccessor, () => MigrateDifference(dbContextAccessor, differences));
                }
            }
            else
            {
                // 初次进行系统整体性迁移
                MigrateCoreAspect(dbContextAccessor, dbContextAccessor.Database.Migrate);
            }
        }

        /// <summary>
        /// 迁移核心截面。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="migrateAction">给定的迁移动作。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected virtual void MigrateCoreAspect(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            Action migrateAction)
        {
            var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
            // 前置处理数据迁移
            aspects.ForEach(aspect => aspect.Preprocess(dbContextAccessor));

            // 迁移动作
            migrateAction?.Invoke();

            // 后置处理数据迁移
            aspects.ForEach(aspect => aspect.Postprocess(dbContextAccessor));

            // 如果需要保存更改
            if (aspects.Any(aspect => aspect.RequireSaving))
            {
                dbContextAccessor.SaveChanges();
                aspects.ForEach(aspect => aspect.RequireSaving = false);

                if (dbContextAccessor.BuilderOptions.ExportMigrationAssembly)
                    ModelSnapshotCompiler.CompileInFile(dbContextAccessor, dbContextAccessor.Model, Options);

                HasLastMigration(dbContextAccessor);
            }
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected virtual async Task MigrateCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var lastModel = ResolveLastModel(dbContextAccessor);
            if (lastModel.IsNotNull())
            {
                // 对比差异
                var differences = GetDifferences(dbContextAccessor.InternalServiceProvider, lastModel, dbContextAccessor.Model);
                if (differences.IsNotEmpty())
                {
                    await MigrateCoreAspectAsync(dbContextAccessor, () =>
                    {
                        // 迁移差异
                        Locker.WaitAction(() => MigrateDifference(dbContextAccessor, differences));
                    },
                    cancellationToken).ConfigureAndWaitAsync();
                }
            }
            else
            {
                await MigrateCoreAspectAsync(dbContextAccessor, async () =>
                {
                    // 初次进行系统整体性迁移
                    await dbContextAccessor.Database.MigrateAsync().ConfigureAndWaitAsync();
                },
                cancellationToken).ConfigureAndWaitAsync();
            }
        }

        /// <summary>
        /// 异步迁移核心截面。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="migrateAction">给定的迁移动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected virtual async Task MigrateCoreAspectAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            Action migrateAction, CancellationToken cancellationToken = default)
        {
            migrateAction.NotNull(nameof(migrateAction));

            var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>>>();
            // 前置处理数据迁移
            aspects.ForEach(async aspect => await aspect.PreprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

            // 迁移动作
            migrateAction?.Invoke();

            // 后置处理数据迁移
            aspects.ForEach(async aspect => await aspect.PostprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

            // 如果需要保存更改
            if (aspects.Any(aspect => aspect.RequireSaving))
            {
                await dbContextAccessor.SaveChangesAsync(cancellationToken).ConfigureAndResultAsync();
                aspects.ForEach(aspect => aspect.RequireSaving = false);

                if (dbContextAccessor.BuilderOptions.ExportMigrationAssembly)
                    ModelSnapshotCompiler.CompileInFile(dbContextAccessor, dbContextAccessor.Model, Options);

                HasLastMigration(dbContextAccessor);
            }
        }


        #region MigrateDifference

        /// <summary>
        /// 迁移差异。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="operationDifferences">给定的迁移操作差异集合。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
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

                if (!dbContextAccessor.IsWritingRequest() && _defaultModel.IsNotNull())
                    _defaultModel = null; // 如果默认请求已迁移表结构，则清空默认模型
            }
        }

        /// <summary>
        /// 获取迁移命令键名。
        /// </summary>
        /// <param name="command">给定的 <see cref="MigrationCommand"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "command")]
        protected virtual string GetMigrationCommandKey(MigrationCommand command)
            => command.CommandText.Sha256Base64String(_coreOptions.Encoding.Source);

        /// <summary>
        /// 获取差异迁移操作。
        /// </summary>
        /// <param name="internalServiceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="lastModel">给定的上一次 <see cref="IModel"/>。</param>
        /// <param name="currentModel">给定的当前 <see cref="IModel"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationOperation}"/>。</returns>
        protected virtual IReadOnlyList<MigrationOperation> GetDifferences(IServiceProvider internalServiceProvider, IModel lastModel, IModel currentModel)
        {
            var typeMappingSource = internalServiceProvider.GetRequiredService<IRelationalTypeMappingSource>();
            var migrationsAnnotations = internalServiceProvider.GetRequiredService<IMigrationsAnnotationProvider>();
            var changeDetector = internalServiceProvider.GetRequiredService<IChangeDetector>();
            var updateAdapterFactory = internalServiceProvider.GetRequiredService<IUpdateAdapterFactory>();
            var commandBatchPreparerDependencies = internalServiceProvider.GetRequiredService<CommandBatchPreparerDependencies>();

            var modelDiffer = new ResetMigrationsModelDiffer(typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory,
                commandBatchPreparerDependencies);

            return modelDiffer.GetDifferences(lastModel, currentModel);
        }

        /// <summary>
        /// 解析上一次模型。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回 <see cref="IModel"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected IModel ResolveLastModel(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            if (!dbContextAccessor.IsWritingRequest() && _defaultModel.IsNotNull())
                return _defaultModel; // 如果当前为默认请求且默认模型不为空，则直接返回用于迁移表结构

            Type snapshotType = null;

            // 读取数据不作写入请求限制
            if (HasLastMigration(dbContextAccessor))
            {
                // 从实体解析
                var buffer = ModelSnapshotCompiler.RestoreAssembly(_lastMigration.ModelBody);
                var modelAssembly = Assembly.Load(buffer);
                snapshotType = modelAssembly.GetType(_lastMigration.ModelSnapshotName, throwOnError: true, ignoreCase: false);
            }
            else
            {
                // 从程序集文件
                var dependencyOptions = dbContextAccessor.ServiceFactory.GetRequiredService<DataBuilderDependency>();
                var assemblyPath = ModelSnapshotCompiler.ExportFilePath(dbContextAccessor.GetType(), dependencyOptions.ExportDirectory);
                if (assemblyPath.Exists())
                {
                    var modelAssembly = Assembly.LoadFile(assemblyPath);
                    var modelSnapshotTypeName = ModelSnapshotCompiler.GenerateTypeName(dbContextAccessor.GetType());
                    snapshotType = modelAssembly.GetType(modelSnapshotTypeName, throwOnError: true, ignoreCase: false);
                }
            }

            if (snapshotType.IsNotNull())
            {
                var model = snapshotType.EnsureCreate<ModelSnapshot>().Model;

                if (dbContextAccessor.IsWritingRequest() && _defaultModel.IsNull())
                    _defaultModel = model; // 缓存当前模型用于默认库迁移表结构

                return model;
            }

            return null;
        }

        /// <summary>
        /// 是否有上次迁移数据。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected virtual bool HasLastMigration(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            if (_lastMigration.IsNull())
                _lastMigration = dbContextAccessor.Migrations.FirstOrDefaultByMax(s => s.CreatedTimeTicks);

            return _lastMigration.IsNotNull();
        }

        #endregion

    }
}
