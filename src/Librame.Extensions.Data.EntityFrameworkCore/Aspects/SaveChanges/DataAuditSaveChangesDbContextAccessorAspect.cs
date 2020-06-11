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
    using Core.Services;
    using Data.Accessors;
    using Data.Builders;
    using Data.Mediators;
    using Data.Stores;
    using Data.ValueGenerators;

    /// <summary>
    /// 数据审计保存变化访问器截面。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataAuditSaveChangesDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>
        : DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>,
            ISaveChangesAccessorAspect<TGenId, TCreatedBy>
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
        /// 构造一个数据审计保存变化访问器截面。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</param>
        /// <param name="createdByGenerator">给定的 <see cref="IDefaultValueGenerator{TValue}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DataAuditSaveChangesDbContextAccessorAspect(IClockService clock,
            IStoreIdentifierGenerator<TGenId> identifierGenerator,
            IDefaultValueGenerator<TCreatedBy> createdByGenerator,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(clock, identifierGenerator, createdByGenerator, options, loggerFactory, priority: 1)
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
        protected override void PreProcessCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            var entityEntries = GetEntityEntries(dbContextAccessor.ChangeTracker);
            if (entityEntries.IsEmpty())
                return;

            var dictionary = new Dictionary<TAudit, List<TAuditProperty>>();
            foreach (var entry in entityEntries)
            {
                var pair = InitializeAuditPair(entry);

                pair.Key.Id = IdentifierGenerator.GenerateAuditIdAsync().ConfigureAndResult();
                pair.Value.ForEach(p => p.AuditId = pair.Key.Id);

                PopulateAuditCreatedTime(entry, pair.Key);
                PopulateAuditCreatedBy(entry, pair.Key);

                dictionary.Add(pair.Key, pair.Value);
            }

            if (dictionary.Count > 0)
            {
                dbContextAccessor.Audits.AddRange(dictionary.Keys);

                foreach (var value in dictionary.Values)
                    dbContextAccessor.AuditProperties.AddRange(value);

                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(new AuditNotification<TAudit, TAuditProperty>(dictionary)).ConfigureAndWait();
            }
        }

        /// <summary>
        /// 异步前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override async Task PreProcessCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var entityEntries = GetEntityEntries(dbContextAccessor.ChangeTracker);
            if (entityEntries.IsEmpty())
                return;

            var dictionary = new Dictionary<TAudit, List<TAuditProperty>>();
            foreach (var entry in entityEntries)
            {
                var pair = InitializeAuditPair(entry);

                pair.Key.Id = await IdentifierGenerator.GenerateAuditIdAsync(cancellationToken).ConfigureAndResultAsync();
                pair.Value.ForEach(p => p.AuditId = pair.Key.Id);

                await PopulateAuditCreatedTimeAsync(entry, pair.Key, cancellationToken).ConfigureAndWaitAsync();
                await PopulateAuditCreatedByAsync(entry, pair.Key, cancellationToken).ConfigureAndWaitAsync();

                dictionary.Add(pair.Key, pair.Value);
            }

            if (dictionary.Count > 0)
            {
                dbContextAccessor.Audits.AddRange(dictionary.Keys);

                foreach (var value in dictionary.Values)
                    dbContextAccessor.AuditProperties.AddRange(value);

                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(new AuditNotification<TAudit, TAuditProperty>(dictionary)).ConfigureAndWait();
            }
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
                    var updatedTime = updation.GetUpdatedTimeAsync().ConfigureAndResult();
                    audit.SetCreatedTimeAsync(updatedTime).ConfigureAndResult();
                }
                else if (entry.Entity is IObjectUpdation objectUpdation)
                {
                    var updatedTime = objectUpdation.GetObjectUpdatedTimeAsync().ConfigureAndResult();
                    audit.SetObjectCreatedTimeAsync(updatedTime).ConfigureAndResult();
                }
                else
                {
                    audit.CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult();
                }
            }
            else
            {
                if (entry.Entity is ICreation<TCreatedBy> creation)
                {
                    var createdTime = creation.GetCreatedTimeAsync().ConfigureAndResult();
                    audit.SetCreatedTimeAsync(createdTime).ConfigureAndResult();
                }
                else if (entry.Entity is IObjectCreation objectCreation)
                {
                    var createdTime = objectCreation.GetObjectCreatedTimeAsync().ConfigureAndResult();
                    audit.SetObjectCreatedTimeAsync(createdTime).ConfigureAndResult();
                }
                else
                {
                    audit.CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult();
                }
            }

            audit.CreatedTimeTicks = audit.CreatedTime.Ticks;
        }

        /// <summary>
        /// 异步填充审计创建时间。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="audit">给定的 <typeparamref name="TAudit"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual async Task PopulateAuditCreatedTimeAsync(EntityEntry entry, TAudit audit,
            CancellationToken cancellationToken)
        {
            // 如果目标实体是更新
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IUpdation<TCreatedBy> updation)
                {
                    var updatedTime = await updation.GetUpdatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetCreatedTimeAsync(updatedTime, cancellationToken).ConfigureAndResultAsync();
                }
                else if (entry.Entity is IObjectUpdation objectUpdation)
                {
                    var updatedTime = await objectUpdation.GetObjectUpdatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetObjectCreatedTimeAsync(updatedTime, cancellationToken).ConfigureAndResultAsync();
                }
                else
                {
                    audit.CreatedTime = await Clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                        .ConfigureAndResultAsync();
                }
            }
            else
            {
                if (entry.Entity is ICreation<TCreatedBy> creation)
                {
                    var createdTime = await creation.GetCreatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetCreatedTimeAsync(createdTime, cancellationToken).ConfigureAndResultAsync();
                }
                else if (entry.Entity is IObjectCreation objectCreation)
                {
                    var createdTime = await objectCreation.GetObjectCreatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetObjectCreatedTimeAsync(createdTime, cancellationToken).ConfigureAndResultAsync();
                }
                else
                {
                    audit.CreatedTime = await Clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                        .ConfigureAndResultAsync();
                }
            }

            audit.CreatedTimeTicks = audit.CreatedTime.Ticks;
        }


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
                    var updatedBy = updation.GetUpdatedByAsync().ConfigureAndResult();
                    audit.SetCreatedByAsync(updatedBy).ConfigureAndResult();
                }
                else if (entry.Entity is IObjectUpdation objectUpdation)
                {
                    var updatedBy = objectUpdation.GetObjectUpdatedByAsync().ConfigureAndResult();
                    audit.SetObjectCreatedByAsync(updatedBy).ConfigureAndResult();
                }
                else
                {
                    audit.CreatedBy = CreatedByGenerator.GetValueAsync(GetType())
                        .ConfigureAndResult();
                }
            }
            else
            {
                if (entry.Entity is ICreation<TCreatedBy> creation)
                {
                    var createdBy = creation.GetCreatedByAsync().ConfigureAndResult();
                    audit.SetCreatedByAsync(createdBy).ConfigureAndResult();
                }
                else if (entry.Entity is IObjectCreation objectCreation)
                {
                    var createdBy = objectCreation.GetObjectCreatedByAsync().ConfigureAndResult();
                    audit.SetObjectCreatedByAsync(createdBy).ConfigureAndResult();
                }
                else
                {
                    audit.CreatedBy = CreatedByGenerator.GetValueAsync(GetType())
                        .ConfigureAndResult();
                }
            }
        }

        /// <summary>
        /// 异步填充审计创建时间。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="audit">给定的 <typeparamref name="TAudit"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual async Task PopulateAuditCreatedByAsync(EntityEntry entry, TAudit audit,
            CancellationToken cancellationToken)
        {
            // 如果目标实体是更新
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IUpdation<TCreatedBy> updation)
                {
                    var updatedBy = await updation.GetUpdatedByAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetCreatedByAsync(updatedBy, cancellationToken).ConfigureAndResultAsync();
                }
                else if (entry.Entity is IObjectUpdation objectUpdation)
                {
                    var updatedBy = await objectUpdation.GetObjectUpdatedByAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetObjectCreatedByAsync(updatedBy, cancellationToken).ConfigureAndResultAsync();
                }
                else
                {
                    audit.CreatedBy = await CreatedByGenerator.GetValueAsync(GetType(), cancellationToken)
                        .ConfigureAndResultAsync();
                }
            }
            else
            {
                if (entry.Entity is ICreation<TCreatedBy> creation)
                {
                    var createdBy = await creation.GetCreatedByAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetCreatedByAsync(createdBy, cancellationToken).ConfigureAndResultAsync();
                }
                else if (entry.Entity is IObjectCreation objectCreation)
                {
                    var createdBy = await objectCreation.GetObjectCreatedByAsync(cancellationToken).ConfigureAndResultAsync();
                    await audit.SetObjectCreatedByAsync(createdBy, cancellationToken).ConfigureAndResultAsync();
                }
                else
                {
                    audit.CreatedBy = await CreatedByGenerator.GetValueAsync(GetType(), cancellationToken)
                        .ConfigureAndResultAsync();
                }
            }
        }


        /// <summary>
        /// 初始化审计键值对。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回包含审计与审计属性列表的键值对。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual KeyValuePair<TAudit, List<TAuditProperty>> InitializeAuditPair(EntityEntry entry)
        {
            var audit = CreateAudit();

            audit.TableName = GetTableName(entry);
            audit.EntityTypeName = StoreHelper.CreatedByTypeName(entry.Metadata.ClrType);
            audit.State = (int)entry.State;
            audit.StateName = entry.State.ToString();

            var auditProperties = new List<TAuditProperty>();
            foreach (var property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken || property.ClrType.IsDefined<NonAuditedAttribute>())
                    continue;

                if (property.IsPrimaryKey())
                    audit.EntityId = GetEntityId(entry.Property(property.Name));

                var auditProperty = CreateAuditProperty();

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

            return new KeyValuePair<TAudit, List<TAuditProperty>>(audit, auditProperties);
        }


        /// <summary>
        /// 获取表名。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual string GetTableName(EntityEntry entry)
            => new TableDescriptor(entry.Metadata.GetTableName(), entry.Metadata.GetSchema());

        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="propertyEntry">给定的 <see cref="PropertyEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual string GetEntityId(PropertyEntry propertyEntry)
        {
            if (propertyEntry.EntityEntry.State == EntityState.Deleted)
                return propertyEntry.OriginalValue?.ToString();

            return propertyEntry.CurrentValue?.ToString();
        }


        /// <summary>
        /// 创建审计。
        /// </summary>
        /// <returns>返回 <typeparamref name="TAudit"/>。</returns>
        protected virtual TAudit CreateAudit()
            => typeof(TAudit).EnsureCreate<TAudit>();

        /// <summary>
        /// 创建审计属性。
        /// </summary>
        /// <returns>返回 <typeparamref name="TAuditProperty"/>。</returns>
        protected virtual TAuditProperty CreateAuditProperty()
            => typeof(TAuditProperty).EnsureCreate<TAuditProperty>();


        /// <summary>
        /// 获取实体入口列表。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <returns>返回 <see cref="List{EntityEntry}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected virtual List<EntityEntry> GetEntityEntries(ChangeTracker changeTracker)
        {
            changeTracker.NotNull(nameof(changeTracker));

            var entityStates = Options.AuditEntityStates;

            return changeTracker.Entries()
                .Where(m => m.Entity.IsNotNull()
                    && !m.Metadata.ClrType.IsDefined<NonAuditedAttribute>()
                    && entityStates.Contains(m.State)).ToList();
        }

    }
}
