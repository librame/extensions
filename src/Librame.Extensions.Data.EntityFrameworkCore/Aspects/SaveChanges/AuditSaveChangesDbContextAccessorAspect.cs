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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core.Mediators;
    using Data.Accessors;
    using Data.Builders;
    using Data.Mediators;
    using Data.Stores;

    /// <summary>
    /// 审计保存更改访问器截面。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class AuditSaveChangesDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>
        : DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>, ISaveChangesAccessorAspect
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId, TCreatedBy>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个审计保存变化访问器截面。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AuditSaveChangesDbContextAccessorAspect(IStoreIdentifierGenerator identifierGenerator,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(identifierGenerator, options, loggerFactory, priority: 1)
        {
        }


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.Stores.UseDataAudit;


        /// <summary>
        /// 前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override void PreProcessCore(DataDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            var entityEntries = GetEntityEntries(dbContextAccessor.ChangeTracker);
            if (entityEntries.IsEmpty())
                return;

            var dictionary = GetAdds(dbContextAccessor, entityEntries);
            if (dictionary.IsNotNull())
            {
                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(new AuditNotification<TAudit, TAuditProperty>(dictionary))
                    .ConfigureAwaitCompleted();
            }
        }

        /// <summary>
        /// 异步前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override async Task PreProcessCoreAsync(DataDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var entityEntries = GetEntityEntries(dbContextAccessor.ChangeTracker);
            if (entityEntries.IsEmpty())
                return;

            var dictionary = GetAdds(dbContextAccessor, entityEntries);
            if (dictionary.IsNotNull())
            {
                var mediator = dbContextAccessor.GetService<IMediator>();
                await mediator.Publish(new AuditNotification<TAudit, TAuditProperty>(dictionary))
                    .ConfigureAwait();
            }
        }


        /// <summary>
        /// 获取添加的审计集合。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="entityEntries">给定的实体入口列表。</param>
        /// <returns>返回添加的审计字典。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual Dictionary<TAudit, List<TAuditProperty>> GetAdds
            (DataDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            List<EntityEntry> entityEntries)
        {
            var manager = dbContextAccessor.AuditsManager;

            Dictionary<TAudit, List<TAuditProperty>> adds = null;

            // Initialize
            var initialized = manager.TryInitializeRange(() =>
            {
                adds = entityEntries.Select(s => CreateAuditAndProperties(s))
                    .ToDictionary(ks => ks.audit, es => es.auditProperties);

                // 附加属性集合
                foreach (var values in adds.Values)
                    dbContextAccessor.AuditProperties.AddRange(values);

                return adds.Keys;
            },
            post =>
            {
                if (!dbContextAccessor.RequiredSaveChanges)
                    dbContextAccessor.RequiredSaveChanges = true;
            });

            if (initialized)
                return adds;

            // Add
            foreach (var entry in entityEntries)
            {
                var entityTableName = GetEntityTableName(entry);
                var entityId = GetEntityId(entry);
                var state = (int)entry.State;

                manager.TryAdd(p => p.TableName == entityTableName && p.EntityId == entityId && p.State == state,
                    () =>
                    {
                        (var audit, var auditProperties) = CreateAuditAndProperties(entry, entityTableName, entityId, state);

                        if (adds.IsNull())
                            adds = new Dictionary<TAudit, List<TAuditProperty>>();

                        adds.Add(audit, auditProperties);

                        // 附加属性集合
                        dbContextAccessor.AuditProperties.AddRange(auditProperties);

                        return audit;
                    },
                    addedPost =>
                    {
                        if (!dbContextAccessor.RequiredSaveChanges)
                            dbContextAccessor.RequiredSaveChanges = true;
                    });
            }

            return adds;
        }


        /// <summary>
        /// 初始化审计和审计属性集合元组。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回元组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual (TAudit audit, List<TAuditProperty> auditProperties) CreateAuditAndProperties
            (EntityEntry entry)
        {
            var entityTableName = GetEntityTableName(entry);
            var entityId = GetEntityId(entry);
            var state = (int)entry.State;

            return CreateAuditAndProperties(entry, entityTableName, entityId, state);
        }

        /// <summary>
        /// 初始化审计和审计属性集合元组。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="entityTableName">给定的实体表名。</param>
        /// <param name="entityId">给定的实体标识。</param>
        /// <param name="state">给定的状态。</param>
        /// <returns>返回元组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual (TAudit audit, List<TAuditProperty> auditProperties) CreateAuditAndProperties
            (EntityEntry entry, string entityTableName, string entityId, int state)
        {
            var audit = CreateAudit();

            audit.TableName = entityTableName;
            audit.EntityId = entityId;
            audit.State = state;

            audit.Id = DataIdentifierGenerator.GenerateAuditIdAsync().ConfigureAwaitCompleted();
            audit.EntityTypeName = StoreHelper.CreatedByTypeName(entry.Metadata.ClrType);
            audit.StateName = entry.State.ToString();

            var auditProperties = new List<TAuditProperty>();
            foreach (var property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken || property.ClrType.IsDefined<NonAuditedAttribute>())
                    continue;

                var auditProperty = CreateAuditProperty();

                auditProperty.AuditId = audit.Id;
                auditProperty.PropertyName = property.Name;
                auditProperty.PropertyTypeName = StoreHelper.CreatedByTypeName(property.ClrType);

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditProperty.NewValue = entry.Property(property.Name).CurrentValue?.ToString();
                        break;

                    case EntityState.Deleted:
                        auditProperty.OldValue = entry.Property(property.Name).OriginalValue?.ToString();
                        break;

                    case EntityState.Modified:
                        {
                            var currentValue = entry.Property(property.Name).CurrentValue?.ToString();
                            var originalValue = entry.Property(property.Name).OriginalValue?.ToString();

                            if (currentValue != originalValue)
                            {
                                auditProperty.NewValue = currentValue;
                                auditProperty.OldValue = originalValue;
                            }
                        }
                        break;
                }

                auditProperties.Add(auditProperty);
            }

            PopulateAuditCreatedTime(entry, audit);
            PopulateAuditCreatedBy(entry, audit);

            return (audit, auditProperties);
        }


        /// <summary>
        /// 填充审计创建时间。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="audit">给定的 <typeparamref name="TAudit"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual void PopulateAuditCreatedTime(EntityEntry entry, TAudit audit)
        {
            // 如果目标实体是更新
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IUpdation<TCreatedBy> updation)
                {
                    var updatedTime = updation.GetUpdatedTimeAsync().ConfigureAwaitCompleted();
                    audit.SetCreatedTimeAsync(updatedTime).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IObjectUpdation objectUpdation)
                {
                    var updatedTime = objectUpdation.GetObjectUpdatedTimeAsync().ConfigureAwaitCompleted();
                    audit.SetObjectCreatedTimeAsync(updatedTime).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IPublication<TCreatedBy> publication)
                {
                    var publishedTime = publication.GetPublishedTimeAsync().ConfigureAwaitCompleted();
                    audit.SetCreatedTimeAsync(publishedTime).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IObjectPublication objectPublication)
                {
                    var publishedTime = objectPublication.GetObjectPublishedTimeAsync().ConfigureAwaitCompleted();
                    audit.SetObjectCreatedTimeAsync(publishedTime).ConfigureAwaitCompleted();
                }
                else
                {
                    audit.CreatedTime = Clock.GetNowOffsetAsync().ConfigureAwaitCompleted();
                }
            }
            else
            {
                if (entry.Entity is ICreation<TCreatedBy> creation)
                {
                    var createdTime = creation.GetCreatedTimeAsync().ConfigureAwaitCompleted();
                    audit.SetCreatedTimeAsync(createdTime).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IObjectCreation objectCreation)
                {
                    var createdTime = objectCreation.GetObjectCreatedTimeAsync().ConfigureAwaitCompleted();
                    audit.SetObjectCreatedTimeAsync(createdTime).ConfigureAwaitCompleted();
                }
                else
                {
                    audit.CreatedTime = Clock.GetNowOffsetAsync().ConfigureAwaitCompleted();
                }
            }

            audit.CreatedTimeTicks = audit.CreatedTime.Ticks;
        }

        ///// <summary>
        ///// 异步填充审计创建时间。
        ///// </summary>
        ///// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        ///// <param name="audit">给定的 <typeparamref name="TAudit"/>。</param>
        ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        ///// <returns>返回一个异步操作。</returns>
        //[SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        //protected virtual async Task PopulateAuditCreatedTimeAsync(EntityEntry entry, TAudit audit,
        //    CancellationToken cancellationToken)
        //{
        //    // 如果目标实体是更新
        //    if (entry.State == EntityState.Modified)
        //    {
        //        if (entry.Entity is IUpdation<TCreatedBy> updation)
        //        {
        //            var updatedTime = await updation.GetUpdatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetCreatedTimeAsync(updatedTime, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IObjectUpdation objectUpdation)
        //        {
        //            var updatedTime = await objectUpdation.GetObjectUpdatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetObjectCreatedTimeAsync(updatedTime, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IPublication<TCreatedBy> publication)
        //        {
        //            var publishedTime = await publication.GetPublishedTimeAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetCreatedTimeAsync(publishedTime, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IObjectPublication objectPublication)
        //        {
        //            var publishedTime = await objectPublication.GetObjectPublishedTimeAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetObjectCreatedTimeAsync(publishedTime, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else
        //        {
        //            audit.CreatedTime = await Clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
        //                .ConfigureAndResultAsync();
        //        }
        //    }
        //    else
        //    {
        //        if (entry.Entity is ICreation<TCreatedBy> creation)
        //        {
        //            var createdTime = await creation.GetCreatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetCreatedTimeAsync(createdTime, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IObjectCreation objectCreation)
        //        {
        //            var createdTime = await objectCreation.GetObjectCreatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetObjectCreatedTimeAsync(createdTime, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else
        //        {
        //            audit.CreatedTime = await Clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
        //                .ConfigureAndResultAsync();
        //        }
        //    }

        //    audit.CreatedTimeTicks = audit.CreatedTime.Ticks;
        //}


        /// <summary>
        /// 填充审计创建者。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="audit">给定的 <typeparamref name="TAudit"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual void PopulateAuditCreatedBy(EntityEntry entry, TAudit audit)
        {
            // 如果目标实体是更新
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IUpdation<TCreatedBy> updation)
                {
                    var updatedBy = updation.GetUpdatedByAsync().ConfigureAwaitCompleted();
                    audit.SetCreatedByAsync(updatedBy).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IObjectUpdation objectUpdation)
                {
                    var updatedBy = objectUpdation.GetObjectUpdatedByAsync().ConfigureAwaitCompleted();
                    audit.SetObjectCreatedByAsync(updatedBy).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IPublication<TCreatedBy> publication)
                {
                    var publishedBy = publication.GetPublishedByAsync().ConfigureAwaitCompleted();
                    audit.SetCreatedByAsync(publishedBy).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IObjectPublication objectPublication)
                {
                    var publishedBy = objectPublication.GetObjectPublishedByAsync().ConfigureAwaitCompleted();
                    audit.SetObjectCreatedByAsync(publishedBy).ConfigureAwaitCompleted();
                }
                else
                {
                    audit.CreatedBy = GetDefaultCreatedBy();
                }
            }
            else
            {
                if (entry.Entity is ICreation<TCreatedBy> creation)
                {
                    var createdBy = creation.GetCreatedByAsync().ConfigureAwaitCompleted();
                    audit.SetCreatedByAsync(createdBy).ConfigureAwaitCompleted();
                }
                else if (entry.Entity is IObjectCreation objectCreation)
                {
                    var createdBy = objectCreation.GetObjectCreatedByAsync().ConfigureAwaitCompleted();
                    audit.SetObjectCreatedByAsync(createdBy).ConfigureAwaitCompleted();
                }
                else
                {
                    audit.CreatedBy = GetDefaultCreatedBy();
                }
            }
        }

        ///// <summary>
        ///// 异步填充审计创建时间。
        ///// </summary>
        ///// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        ///// <param name="audit">给定的 <typeparamref name="TAudit"/>。</param>
        ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        ///// <returns>返回一个异步操作。</returns>
        //[SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        //protected virtual async Task PopulateAuditCreatedByAsync(EntityEntry entry, TAudit audit,
        //    CancellationToken cancellationToken)
        //{
        //    // 如果目标实体是更新
        //    if (entry.State == EntityState.Modified)
        //    {
        //        if (entry.Entity is IUpdation<TCreatedBy> updation)
        //        {
        //            var updatedBy = await updation.GetUpdatedByAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetCreatedByAsync(updatedBy, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IObjectUpdation objectUpdation)
        //        {
        //            var updatedBy = await objectUpdation.GetObjectUpdatedByAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetObjectCreatedByAsync(updatedBy, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IPublication<TCreatedBy> publication)
        //        {
        //            var publishedBy = await publication.GetPublishedByAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetCreatedByAsync(publishedBy, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IObjectPublication objectPublication)
        //        {
        //            var publishedBy = await objectPublication.GetObjectPublishedByAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetObjectCreatedByAsync(publishedBy, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else
        //        {
        //            audit.CreatedBy = GetDefaultCreatedBy();
        //        }
        //    }
        //    else
        //    {
        //        if (entry.Entity is ICreation<TCreatedBy> creation)
        //        {
        //            var createdBy = await creation.GetCreatedByAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetCreatedByAsync(createdBy, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else if (entry.Entity is IObjectCreation objectCreation)
        //        {
        //            var createdBy = await objectCreation.GetObjectCreatedByAsync(cancellationToken).ConfigureAndResultAsync();
        //            await audit.SetObjectCreatedByAsync(createdBy, cancellationToken).ConfigureAndResultAsync();
        //        }
        //        else
        //        {
        //            audit.CreatedBy = GetDefaultCreatedBy();
        //        }
        //    }
        //}


        /// <summary>
        /// 获取实体表名。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual string GetEntityTableName(EntityEntry entry)
            => new TableDescriptor(entry.Metadata.GetTableName(), entry.Metadata.GetSchema());


        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="entityEntry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual string GetEntityId(EntityEntry entityEntry)
        {
            var property = entityEntry.CurrentValues.Properties.FirstOrDefault(p => p.IsPrimaryKey());
            if (property.IsNotNull())
                return GetEntityId(entityEntry.Property(property.Name));

            var properties = entityEntry.CurrentValues.Properties
                .Where(p => p.ClrType.IsDefined<IdentifiableAttribute>())
                .ToList();

            if (properties.IsNotEmpty())
            {
                return properties.Select(s => GetEntityId(entityEntry.Property(s.Name)))
                    .CompatibleJoinString(';');
            }

            return $"ForcedNotEmpty:{Guid.NewGuid()}";
        }

        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="propertyEntry">给定的 <see cref="PropertyEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual string GetEntityId(PropertyEntry propertyEntry)
        {
            if (propertyEntry.EntityEntry.State == EntityState.Deleted)
                return propertyEntry.OriginalValue?.ToString();

            return propertyEntry.CurrentValue?.ToString();
        }

        /// <summary>
        /// 获取默认创建者。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCreatedBy"/>。</returns>
        protected virtual TCreatedBy GetDefaultCreatedBy()
            => default;


        /// <summary>
        /// 创建审计。
        /// </summary>
        /// <returns>返回 <typeparamref name="TAudit"/>。</returns>
        protected virtual TAudit CreateAudit()
            => ObjectExtensions.EnsureCreate<TAudit>();

        /// <summary>
        /// 创建审计属性。
        /// </summary>
        /// <returns>返回 <typeparamref name="TAuditProperty"/>。</returns>
        protected virtual TAuditProperty CreateAuditProperty()
            => ObjectExtensions.EnsureCreate<TAuditProperty>();


        /// <summary>
        /// 获取实体入口列表。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <returns>返回 <see cref="List{EntityEntry}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual List<EntityEntry> GetEntityEntries(ChangeTracker changeTracker)
        {
            changeTracker.NotNull(nameof(changeTracker));

            var query = changeTracker.Entries().Where(p
                => p.Entity.IsNotNull() && !p.Metadata.ClrType.IsDefined<NonAuditedAttribute>());

            var entityStates = Options.AuditEntityStates;
            if (entityStates.IsNotEmpty())
                return query.Where(p => entityStates.Contains(p.State)).ToList();

            return query.Where(p => p.State != EntityState.Unchanged).ToList();
        }

    }
}
