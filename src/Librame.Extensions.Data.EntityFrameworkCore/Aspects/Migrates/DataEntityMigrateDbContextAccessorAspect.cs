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
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据实体迁移数据库上下文访问器截面。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class DataEntityMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        : MigrateDbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        private List<TEntity> _cache = new List<TEntity>();


        /// <summary>
        /// 构造一个数据实体迁移数据库上下文访问器截面。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DataEntityMigrateDbContextAccessorAspect(IClockService clock, IStoreIdentifier identifier,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(clock, identifier, options, loggerFactory)
        {
        }


        /// <summary>
        /// 数据实体缓存。
        /// </summary>
        public IReadOnlyList<TEntity> Cache
            => _cache.AsReadOnlyList();


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.EntityEnabled;


        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected override void PostprocessCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var diff = GetDifference(dbContextAccessor);
            if (diff.IsNotEmpty())
            {
                dbContextAccessor.Entities.AddRange(diff);
                _cache.AddRange(diff);
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
                mediator.Publish(new EntityNotification<TEntity, TGenId> { Entities = diff }).ConfigureAndWait();
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
            var diff = GetDifference(dbContextAccessor, cancellationToken);
            if (diff.IsNotEmpty())
            {
                await dbContextAccessor.Entities.AddRangeAsync(diff, cancellationToken).ConfigureAndWaitAsync();
                _cache.AddRange(diff);
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
                await mediator.Publish(new EntityNotification<TEntity, TGenId> { Entities = diff }).ConfigureAndWaitAsync();
            }
        }

        /// <summary>
        /// 获取差异的数据实体。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="List{DataEntity}"/>。</returns>
        protected virtual List<TEntity> GetDifference(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            if (_cache.IsEmpty())
            {
                var entitis = dbContextAccessor.Entities.ToList();
                if (entitis.IsNotEmpty())
                    _cache.AddRange(entitis);
            }

            var diff = dbContextAccessor.Model.GetEntityTypes()
                .Select(s => GetEntity(s, cancellationToken))
                .ToList();

            if (_cache.IsNotEmpty())
                diff = diff.Except(_cache).ToList();

            return diff;
        }

        /// <summary>
        /// 获取数据实体。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
        protected virtual TEntity GetEntity(IEntityType entityType, CancellationToken cancellationToken)
        {
            var entity = typeof(TEntity).EnsureCreate<TEntity>();
            entity.Id = GetEntityId(cancellationToken);
            entity.EntityName = entityType.ClrType.GetSimpleFullName();
            entity.AssemblyName = entityType.ClrType.GetSimpleAssemblyName();
            entity.CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true, cancellationToken).ConfigureAndResult();
            entity.CreatedBy = GetType().GetBodyName();

            if (entityType.ClrType.TryGetCustomAttribute(out DescriptionAttribute descr)
                && descr.Description.IsNotEmpty())
            {
                entity.Description = descr.Description;
            }

            entity.Name = entityType.GetTableName();
            entity.Schema = entityType.GetSchema();

            if (entity.Name.IsEmpty())
                SetDefaultTableName(entity, entityType);

            if (entity.Schema.IsEmpty())
                SetDefaultTableSchema(entity, entityType);

            return entity;
        }

        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TGenId"/>。</returns>
        protected virtual TGenId GetEntityId(CancellationToken cancellationToken)
        {
            var entityId = Identifier.GetEntityIdAsync(cancellationToken).ConfigureAndResult();
            return entityId.CastTo<string, TGenId>(nameof(entityId));
        }

        /// <summary>
        /// 设置默认表名。
        /// </summary>
        /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void SetDefaultTableName(TEntity entity, IEntityType entityType)
            => entity.Name = entityType.ClrType.Name;

        /// <summary>
        /// 设置默认表架构。
        /// </summary>
        /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void SetDefaultTableSchema(TEntity entity, IEntityType entityType)
            => entity.Schema = Options.Tables.DefaultSchema.NotEmptyOrDefault("dbo");


        /// <summary>
        /// 释放实体表缓存。
        /// </summary>
        protected override void DisposeCore()
            => _cache.Clear();
    }
}
