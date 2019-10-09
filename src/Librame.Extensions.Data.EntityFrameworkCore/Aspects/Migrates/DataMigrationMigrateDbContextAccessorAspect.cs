#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

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
        : MigrateDbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
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
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DataMigrationMigrateDbContextAccessorAspect(IClockService clock, IStoreIdentifier identifier,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(clock, identifier, options, loggerFactory)
        {
        }


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.MigrationEnabled;


        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected override void PostprocessCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var migration = GetMigration(dbContextAccessor, default);

            var uniqueExpression = StoreExpression.GetMigrationUniqueIndexExpression<TMigration, TGenId>(migration.ModelHash);
            if (!dbContextAccessor.Migrations.Exists(uniqueExpression))
            {
                dbContextAccessor.Migrations.Add(migration);
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
                mediator.Publish(new MigrationNotification<TMigration, TGenId> { Migration = migration }).ConfigureAndWait();
            }
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected override async Task PostprocessCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var migration = GetMigration(dbContextAccessor, cancellationToken);

            var uniqueExpression = StoreExpression.GetMigrationUniqueIndexExpression<TMigration, TGenId>(migration.ModelHash);
            if (!await dbContextAccessor.Migrations.ExistsAsync(uniqueExpression, cancellationToken).ConfigureAndResultAsync())
            {
                await dbContextAccessor.Migrations.AddAsync(migration, cancellationToken).ConfigureAndResultAsync();
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
                await mediator.Publish(new MigrationNotification<TMigration, TGenId> { Migration = migration }).ConfigureAndWaitAsync();
            }
        }

        /// <summary>
        /// 获取数据迁移。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TMigration"/>。</returns>
        protected virtual TMigration GetMigration(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken)
        {
            var accessorType = dbContextAccessor.GetType();
            var modelSnapshotTypeName = new TypeNameCombiner(accessorType.Namespace, $"{accessorType.GetBodyName()}{nameof(ModelSnapshot)}");

            var generator = dbContextAccessor.ServiceFactory.GetRequiredService<IMigrationsCodeGenerator>();
            var dependencyOptions = dbContextAccessor.ServiceFactory.GetRequiredService<DataBuilderDependencyOptions>();

            // 生成模型快照
            var modelSnapshot = GenerateModelSnapshot(dbContextAccessor.Model, modelSnapshotTypeName,
                accessorType, generator, dependencyOptions);

            var migration = typeof(TMigration).EnsureCreate<TMigration>();
            migration.Id = GetMigrationId(cancellationToken);
            migration.AccessorName = accessorType.GetSimpleFullName();
            migration.ModelSnapshotName = modelSnapshotTypeName;
            migration.ModelBody = modelSnapshot.Bytes;
            migration.ModelHash = modelSnapshot.Hash;
            migration.CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true, cancellationToken).ConfigureAndResult();
            migration.CreatedBy = GetType().GetBodyName();

            return migration;
        }

        /// <summary>
        /// 获取迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TGenId"/>。</returns>
        protected virtual TGenId GetMigrationId(CancellationToken cancellationToken)
        {
            var migrationId = Identifier.GetEntityIdAsync(cancellationToken).ConfigureAndResult();
            return migrationId.CastTo<string, TGenId>(nameof(migrationId));
        }

        /// <summary>
        /// 生成模型快照。
        /// </summary>
        /// <param name="model">给定的 <see cref="IModel"/>。</param>
        /// <param name="modelSnapshotTypeName">给定的模型快照类型名。</param>
        /// <param name="dbContextAccessorType">给定的数据库上下文访问器类型。</param>
        /// <param name="generator">给定的 <see cref="IMigrationsCodeGenerator"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="DataBuilderDependencyOptions"/>。</param>
        /// <returns>返回字节数组。</returns>
        protected (byte[] Bytes, string Hash) GenerateModelSnapshot(IModel model, TypeNameCombiner modelSnapshotTypeName,
            Type dbContextAccessorType, IMigrationsCodeGenerator generator, DataBuilderDependencyOptions dependencyOptions)
        {
            var modelSnapshotCode = generator.GenerateSnapshot(modelSnapshotTypeName.Namespace, dbContextAccessorType,
                modelSnapshotTypeName.Name, model);

            var references = GetAssemblyReferences(dbContextAccessorType);

            if (Options.ExportModelSnapshotFilePath.IsNotEmpty())
            {
                FilePathCombiner filePath = Options.ExportModelSnapshotFilePath;
                filePath.ChangeBasePathIfEmpty(dependencyOptions.BaseDirectory);

                // 导出包含模型快照的程序集文件
                ModelSnapshotCompiler.CompileInFile(filePath, references, modelSnapshotCode);
            }

            var bytes = ModelSnapshotCompiler.CompileInMemory(references, modelSnapshotCode);
            return (bytes.Compress(), modelSnapshotCode.Sha256Base64String());
        }

        /// <summary>
        /// 获取程序集引用集合。
        /// </summary>
        /// <param name="dbContextAccessorType">给定的数据库上下文访问器类型。</param>
        /// <returns>返回 <see cref="IReadOnlyList{AssemblyReference}"/>。</returns>
        protected virtual IReadOnlyList<AssemblyReference> GetAssemblyReferences(Type dbContextAccessorType)
        {
            var references = new List<AssemblyReference>(Options.MigrationAssemblyReferences);

            // Add DbContextAccessor AssemblyReference
            var dbContextAccessorReference = AssemblyReference.ByAssembly(dbContextAccessorType.Assembly);
            if (!references.Contains(dbContextAccessorReference))
                references.Add(dbContextAccessorReference);

            references.Sort();

            //// Add NetStardand AssemblyReference
            //if (Options.NetStardandAssemblyReference.IsNotEmpty())
            //{
            //    AssemblyReference netStandardReference;

            //    if (File.Exists(Options.NetStardandAssemblyReference))
            //        netStandardReference = AssemblyReference.ByPath(Options.NetStardandAssemblyReference);
            //    else
            //        netStandardReference = AssemblyReference.ByName(Options.NetStardandAssemblyReference);

            //    references.Add(netStandardReference);
            //}

            return references.AsReadOnlyList();
        }


        ///// <summary>
        ///// 模型快照程序集引用集合。
        ///// </summary>
        //public static IList<AssemblyReference> ModelSnapshotAssemblyReferences = new List<AssemblyReference>
        //{
        //    AssemblyReference.ByName("Microsoft.EntityFrameworkCore"),
        //    AssemblyReference.ByName("Microsoft.EntityFrameworkCore.Relational"),
        //    AssemblyReference.ByName("Librame.Extensions.Data.Abstractions"),
        //    AssemblyReference.ByName("Librame.Extensions.Data.EntityFrameworkCore")
        //};

    }
}
