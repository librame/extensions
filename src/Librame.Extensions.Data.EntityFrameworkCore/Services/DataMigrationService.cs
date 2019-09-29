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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据迁移服务。
    /// </summary>
    public class DataMigrationService : AbstractExtensionBuilderService<DataBuilderOptions>, IDataMigrationService
    {
        private static DataMigration _cache = null;


        /// <summary>
        /// 构造一个 <see cref="DataMigrationService"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DataMigrationService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 启用服务。
        /// </summary>
        public bool Enabled
            => Options.MigrationEnabled;


        /// <summary>
        /// 迁移。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public void Migrate(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                MigrateCore(dbContextAccessor);
        }

        /// <summary>
        /// 迁移核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        protected virtual void MigrateCore(DbContextAccessor dbContextAccessor)
        {
            if (dbContextAccessor.Migrations.Exists())
            {
                var query = dbContextAccessor.Migrations.AsQueryable();

                if (_cache.IsNull())
                    _cache = query.FirstOrDefault(p => p.CreatedTime == query.Max(s => s.CreatedTime));

                // 对比差异
                var differences = GetDifferences(dbContextAccessor);
                if (differences.IsNotEmpty())
                {
                    var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateAccessorAspect>>();
                    // 前置处理数据迁移
                    aspects.ForEach(aspect => aspect.Preprocess(dbContextAccessor));

                    // 差异迁移
                    DifferenceMigration(dbContextAccessor, differences);

                    // 后置处理数据迁移
                    aspects.ForEach(aspect => aspect.Postprocess(dbContextAccessor));

                    dbContextAccessor.SaveChanges();

                    _cache = query.FirstOrDefault(p => p.CreatedTime == query.Max(s => s.CreatedTime));
                }
            }
            else
            {
                var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateAccessorAspect>>();
                // 前置处理数据迁移
                aspects.ForEach(aspect => aspect.Preprocess(dbContextAccessor));

                // 初次进行系统整体性迁移
                dbContextAccessor.Database.Migrate();

                // 后置处理数据迁移
                aspects.ForEach(aspect => aspect.Postprocess(dbContextAccessor));

                dbContextAccessor.SaveChanges();
            }
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

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                return MigrateCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步迁移核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual async Task MigrateCoreAsync(DbContextAccessor dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            if (await dbContextAccessor.Migrations.ExistsAsync(cancellationToken).ConfigureAndResultAsync())
            {
                var query = dbContextAccessor.Migrations.AsQueryable();

                if (_cache.IsNull())
                {
                    _cache = await query.FirstOrDefaultAsync(p => p.CreatedTime == query.Max(s => s.CreatedTime),
                        cancellationToken).ConfigureAndResultAsync();
                }

                // 对比差异
                var differences = GetDifferences(dbContextAccessor);
                if (differences.IsNotEmpty())
                {
                    var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateAccessorAspect>>();
                    // 前置处理数据迁移
                    aspects.ForEach(async aspect => await aspect.PreprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                    // 差异迁移
                    DifferenceMigration(dbContextAccessor, differences);

                    // 后置处理数据迁移
                    aspects.ForEach(async aspect => await aspect.PostprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                    await dbContextAccessor.SaveChangesAsync(cancellationToken).ConfigureAndResultAsync();

                    _cache = await query.FirstOrDefaultAsync(p => p.CreatedTime == query.Max(s => s.CreatedTime),
                        cancellationToken).ConfigureAndResultAsync();
                }
            }
            else
            {
                var aspects = dbContextAccessor.ServiceFactory.GetRequiredService<IServicesManager<IMigrateAccessorAspect>>();
                // 前置处理数据迁移
                aspects.ForEach(async aspect => await aspect.PreprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                // 初次进行系统整体性迁移
                await dbContextAccessor.Database.MigrateAsync().ConfigureAndWaitAsync();

                // 后置处理数据迁移
                aspects.ForEach(async aspect => await aspect.PostprocessAsync(dbContextAccessor, cancellationToken).ConfigureAndWaitAsync());

                await dbContextAccessor.SaveChangesAsync(cancellationToken).ConfigureAndResultAsync();
            }
        }


        /// <summary>
        /// 差异迁移。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="differences">给定的迁移操作集合。</param>
        protected virtual void DifferenceMigration(DbContextAccessor dbContextAccessor, IReadOnlyList<MigrationOperation> differences)
        {
            //var rawSqlCommandBuilder = dbContextAccessor.ServiceProvider.GetRequiredService<IRawSqlCommandBuilder>();
            var migrationsSqlGenerator = dbContextAccessor.ServiceProvider.GetRequiredService<IMigrationsSqlGenerator>();
            var migrationCommandExecutor = dbContextAccessor.ServiceProvider.GetRequiredService<IMigrationCommandExecutor>();
            var connection = dbContextAccessor.ServiceProvider.GetRequiredService<IRelationalConnection>();
            //var historyRepository = dbContextAccessor.ServiceProvider.GetRequiredService<IHistoryRepository>();
            //var insertCommand = rawSqlCommandBuilder.Build(historyRepository.GetInsertScript(new HistoryRow(migration.GetId(), ProductInfo.GetVersion())));

            var commands = migrationsSqlGenerator
                .Generate(differences, dbContextAccessor.Model)
                //.Concat(new[] { new MigrationCommand(insertCommand, _currentContext.Context, _commandLogger) })
                .ToList();

            migrationCommandExecutor.ExecuteNonQuery(commands, connection);
        }

        /// <summary>
        /// 获取差异迁移操作。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationOperation}"/>。</returns>
        protected virtual IReadOnlyList<MigrationOperation> GetDifferences(DbContextAccessor dbContextAccessor)
        {
            var lastModel = ResolveModel();
            var typeMappingSource = dbContextAccessor.ServiceProvider.GetRequiredService<IRelationalTypeMappingSource>();
            var migrationsAnnotations = dbContextAccessor.ServiceProvider.GetRequiredService<IMigrationsAnnotationProvider>();
            var changeDetector = dbContextAccessor.ServiceProvider.GetRequiredService<IChangeDetector>();
            var updateAdapterFactory = dbContextAccessor.ServiceProvider.GetRequiredService<IUpdateAdapterFactory>();
            var commandBatchPreparerDependencies = dbContextAccessor.ServiceProvider.GetRequiredService<CommandBatchPreparerDependencies>();

            var modelDiffer = new ResetMigrationsModelDiffer(typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory,
                commandBatchPreparerDependencies);

            return modelDiffer.GetDifferences(lastModel, dbContextAccessor.Model);
        }

        /// <summary>
        /// 解析模型。
        /// </summary>
        /// <returns>返回 <see cref="IModel"/>。</returns>
        protected IModel ResolveModel()
        {
            var assembly = Assembly.Load(_cache.ModelBody.Decompress());
            var snapshotType = assembly.GetType(_cache.ModelSnapshotName, throwOnError: true, ignoreCase: false);

            //var dbContextAttribute = snapshotType.GetCustomAttribute<DbContextAttribute>();
            var snapshot = snapshotType.EnsureCreate<ModelSnapshot>();
            return snapshot.Model;
        }

    }
}
