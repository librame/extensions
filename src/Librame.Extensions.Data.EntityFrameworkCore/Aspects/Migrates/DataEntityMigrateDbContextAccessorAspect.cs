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
    using Core.Services;
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
        private readonly string _createdBy;

        private List<TEntity> _cache = new List<TEntity>();


        /// <summary>
        /// 构造一个数据实体迁移数据库上下文访问器截面。
        /// </summary>
        /// <param name="dependencies">给定的 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。</param>
        public DataEntityMigrateDbContextAccessorAspect(DbContextAccessorAspectDependencies<TGenId> dependencies)
            : base(dependencies, priority: 2)
        {
            _createdBy = EntityPopulator.FormatTypeName(GetType());
        }


        /// <summary>
        /// 需要保存更改。
        /// </summary>
        public bool RequiredSaveChanges { get; set; }


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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "dbContextAccessor")]
        protected override void PostprocessCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var notification = new EntityNotification<TEntity, TGenId>();
            var result = GetDifference(dbContextAccessor);

            if (result.Adds.IsNotEmpty())
            {
                dbContextAccessor.Entities.AddRangeAsync(result.Adds);
                _cache.AddRange(result.Adds);

                RequiredSaveChanges = true;
                notification.Adds = result.Adds;
            }

            if (result.Updates.IsNotEmpty())
            {
                dbContextAccessor.Entities.UpdateRange(result.Updates);
                result.Updates.ForEach(item => _cache.Remove(item));
                _cache.AddRange(result.Updates);

                RequiredSaveChanges = true;
                notification.Updates = result.Updates;
            }

            if (result.Removes.IsNotEmpty())
            {
                dbContextAccessor.Entities.RemoveRange(result.Removes);
                result.Removes.ForEach(item => _cache.Remove(item));

                RequiredSaveChanges = true;
                notification.Removes = result.Removes;
            }

            if (RequiredSaveChanges)
            {
                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
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
        protected override async Task PostprocessCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var notification = new EntityNotification<TEntity, TGenId>();
            var result = GetDifference(dbContextAccessor, cancellationToken);

            if (result.Adds.IsNotEmpty())
            {
                await dbContextAccessor.Entities.AddRangeAsync(result.Adds, cancellationToken).ConfigureAndWaitAsync();
                _cache.AddRange(result.Adds);

                RequiredSaveChanges = true;
                notification.Adds = result.Adds;
            }

            if (result.Updates.IsNotEmpty())
            {
                dbContextAccessor.Entities.UpdateRange(result.Updates);
                result.Updates.ForEach(item => _cache.Remove(item));
                _cache.AddRange(result.Updates);

                RequiredSaveChanges = true;
                notification.Updates = result.Updates;
            }

            if (result.Removes.IsNotEmpty())
            {
                dbContextAccessor.Entities.RemoveRange(result.Removes);
                result.Removes.ForEach(item => _cache.Remove(item));

                RequiredSaveChanges = true;
                notification.Removes = result.Removes;
            }

            if (RequiredSaveChanges)
            {
                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
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
        protected virtual (List<TEntity> Adds, List<TEntity> Updates, List<TEntity> Removes) GetDifference(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            if (_cache.IsEmpty())
            {
                var dbEntitis = dbContextAccessor.Entities.ToList();
                if (dbEntitis.IsNotEmpty())
                    _cache.AddRange(dbEntitis);
            }

            var modelEntities = dbContextAccessor.Model.GetEntityTypes()
                .Select(s => GenerateEntity(s, cancellationToken)).ToList();

            // 如果缓存不为空且全等于模型实体集合，则直接返回
            if (_cache.IsNotEmpty() && modelEntities.SequencePropertyValuesEquals(_cache))
            {
                return (null, null, null);
            }

            //if (_cache.IsNotEmpty())
            //    modelEntities = modelEntities.Except(_cache).ToList();

            // 双向同步
            var adds = new List<TEntity>();
            var updates = new List<TEntity>();
            var removes = new List<TEntity>();

            foreach (var entity in modelEntities.Union(_cache))
            {
                // 查找唯一索引
                var cacheEntity = _cache.FirstOrDefault(p => p.Equals(entity));
                var modelEntity = modelEntities.FirstOrDefault(p => p.Equals(entity));

                if (cacheEntity.IsNull())
                {
                    // 缓存不存在则表示添加
                    adds.Add(entity);
                    continue;
                }
                else if (modelEntity.IsNull() && !entity.IsSharding)
                {
                    // 生成实体不存在且实体未分表，则表示删除
                    removes.Add(entity);
                    continue;
                }
                else if (!cacheEntity.PropertyValuesEquals(modelEntity))
                {
                    // 缓存存在但属性值集合不全等，则表示更新
                    modelEntity.Id = cacheEntity.Id;
                    modelEntity.CreatedTime = cacheEntity.CreatedTime;

                    updates.Add(modelEntity); // 以模型生成的实体为主体
                    continue;
                }
                else
                {
                }
            }

            return (adds, updates, removes);
        }

        /// <summary>
        /// 生成数据实体。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "entityType")]
        protected virtual TEntity GenerateEntity(IEntityType entityType, CancellationToken cancellationToken)
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
            entity.IsSharding = entityType.ClrType.TryGetCustomAttribute(out ShardingTableAttribute attribute)
                && attribute.Mode == ShardingTableMode.Create;

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
    }
}
