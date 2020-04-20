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
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core.Mediators;
    using Data.Accessors;
    using Data.Mediators;
    using Data.Stores;

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
        private readonly object _locker = new object();
        private readonly string _createdBy;


        /// <summary>
        /// 构造一个数据实体迁移数据库上下文访问器截面。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="dependencies">给定的 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。</param>
        public DataEntityMigrateDbContextAccessorAspect(IMemoryCache memoryCache,
            DbContextAccessorAspectDependencies<TGenId> dependencies)
            : base(dependencies, priority: 2)
        {
            _createdBy = EntityPopulator.FormatTypeName(GetType());

            MemoryCache = memoryCache.NotNull(nameof(memoryCache));
        }


        /// <summary>
        /// 内存缓存。
        /// </summary>
        public IMemoryCache MemoryCache { get; }

        /// <summary>
        /// 需要保存更改。
        /// </summary>
        public bool RequiredSaveChanges { get; set; }


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.EntityEnabled;


        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected override void PostProcessCore
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var notification = new EntityNotification<TEntity, TGenId>();
            var result = GetDifference(dbContextAccessor);

            if (result.Adds.IsNotEmpty())
            {
                dbContextAccessor.Entities.AddRange(result.Adds);

                RequiredSaveChanges = true;
                notification.Adds = result.Adds;
            }

            if (result.Updates.IsNotEmpty())
            {
                dbContextAccessor.Entities.UpdateRange(result.Updates);

                RequiredSaveChanges = true;
                notification.Updates = result.Updates;
            }

            if (RequiredSaveChanges)
            {
                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(notification).ConfigureAndWait();
            }
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected override async Task PostProcessCoreAsync
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var notification = new EntityNotification<TEntity, TGenId>();
            var result = GetDifference(dbContextAccessor, cancellationToken);

            if (result.Adds.IsNotEmpty())
            {
                await dbContextAccessor.Entities.AddRangeAsync(result.Adds, cancellationToken)
                    .ConfigureAndWaitAsync();

                RequiredSaveChanges = true;
                notification.Adds = result.Adds;
            }

            if (result.Updates.IsNotEmpty())
            {
                dbContextAccessor.Entities.UpdateRange(result.Updates);

                RequiredSaveChanges = true;
                notification.Updates = result.Updates;
            }

            if (RequiredSaveChanges)
            {
                var mediator = dbContextAccessor.GetService<IMediator>();
                await mediator.Publish(notification).ConfigureAndWaitAsync();
            }
        }

        /// <summary>
        /// 获取差异的数据实体。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含添加、更新以及删除的 <see cref="List{DataEntity}"/> 元组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected virtual (List<TEntity> Adds, List<TEntity> Updates) GetDifference
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            lock (_locker)
            {
                var cacheEntities = MemoryCache.GetOrCreate(GetCacheKey(dbContextAccessor),
                entry => dbContextAccessor.Entities.ToList() ?? new List<TEntity>());

                var modelEntities = dbContextAccessor.Model.GetEntityTypes()
                    .Select(s => CreateEntity(s, cancellationToken)).ToList();

                var adds = new List<TEntity>();
                var updates = new List<TEntity>();

                foreach (var model in modelEntities)
                {
                    var cache = cacheEntities.FirstOrDefault(p => p.Equals(model));
                    if (cache.IsNull())
                    {
                        adds.Add(model);
                    }
                    else if (UpdateCache(model, cache))
                    {
                        updates.Add(cache);
                    }
                    else
                    {
                        // 暂不考虑删除实体
                    }
                }

                // 如果有新增实体，则添加到缓存中
                if (adds.Count > 0)
                    cacheEntities.AddRange(adds);

                return (adds, updates);
            }
        }

        /// <summary>
        /// 更新缓存实体。
        /// </summary>
        /// <param name="modelEntity">给定的模型实体。</param>
        /// <param name="cacheEntity">给定的缓存实体。</param>
        /// <returns>返回是否更新的布尔值。</returns>
        protected virtual bool UpdateCache(TEntity modelEntity, TEntity cacheEntity)
        {
            var isUpdated = false;

            if (modelEntity?.EntityName != cacheEntity?.EntityName)
            {
                cacheEntity.EntityName = modelEntity.EntityName;
                isUpdated = true;
            }

            if (modelEntity.AssemblyName != cacheEntity.AssemblyName)
            {
                cacheEntity.AssemblyName = modelEntity.AssemblyName;
                isUpdated = true;
            }

            if (modelEntity.Description != cacheEntity.Description)
            {
                cacheEntity.Description = modelEntity.Description;
                isUpdated = true;
            }

            if (modelEntity.IsSharding != cacheEntity.IsSharding)
            {
                cacheEntity.IsSharding = modelEntity.IsSharding;
                isUpdated = true;
            }

            return isUpdated;
        }

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "entityType")]
        protected virtual TEntity CreateEntity(IEntityType entityType, CancellationToken cancellationToken)
        {
            var entity = typeof(TEntity).EnsureCreate<TEntity>();

            entity.Id = GetEntityId(cancellationToken);
            entity.EntityName = entityType.ClrType.GetDisplayNameWithNamespace();
            entity.AssemblyName = entityType.ClrType.GetAssemblyDisplayName();
            entity.CreatedTime = Dependencies.Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true, cancellationToken).ConfigureAndResult();
            entity.CreatedTimeTicks = entity.CreatedTime.Ticks;
            entity.CreatedBy = _createdBy;

            if (entityType.ClrType.TryGetCustomAttribute(out DescriptionAttribute descr)
                && descr.Description.IsNotEmpty())
            {
                entity.Description = descr.Description;
            }

            entity.Name = entityType.GetTableName();
            entity.Schema = entityType.GetSchema();
            entity.IsSharding = entityType.ClrType.TryGetCustomAttribute(out ShardingAttribute _);

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
            => Dependencies.Identifier.GetEntityIdAsync(cancellationToken).ConfigureAndResult();

        /// <summary>
        /// 设置默认表名。
        /// </summary>
        /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void SetDefaultTableName(TEntity entity, IEntityType entityType)
            => entity.Name = entityType.ClrType.Name;

        /// <summary>
        /// 设置默认表架构。
        /// </summary>
        /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "entity")]
        protected virtual void SetDefaultTableSchema(TEntity entity, IEntityType entityType)
            => entity.Schema = Options.Tables.DefaultSchema.NotEmptyOrDefault("dbo");

        /// <summary>
        /// 获取缓存键。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetCacheKey(DbContextAccessorBase accessor)
            => $"{nameof(DataEntityMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>)}{accessor?.CurrentConnectionString}";
    }
}
