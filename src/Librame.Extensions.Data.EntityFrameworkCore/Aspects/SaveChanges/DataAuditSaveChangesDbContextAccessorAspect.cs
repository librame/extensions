﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    /// 数据审计保存变化访问器截面。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class DataAuditSaveChangesDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        : DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        , ISaveChangesDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个数据审计保存变化访问器截面。
        /// </summary>
        /// <param name="dependencies">给定的 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。</param>
        public DataAuditSaveChangesDbContextAccessorAspect(DbContextAccessorAspectDependencies<TGenId> dependencies)
            : base(dependencies, priority: 1)
        {
        }


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.AuditEnabled;


        /// <summary>
        /// 前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override void PreProcessCore
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            var dictionary = GetAuditPropertiesDictionary(dbContextAccessor.ChangeTracker);
            if (dictionary.IsNotEmpty())
            {
                dbContextAccessor.Audits.AddRange(dictionary.Keys);

                foreach (var properties in dictionary.Values)
                    dbContextAccessor.AuditProperties.AddRange(properties);

                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(new AuditNotification<TAudit, TAuditProperty>(dictionary)).ConfigureAndWait();
            }
        }

        /// <summary>
        /// 异步前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override async Task PreProcessCoreAsync
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var dictionary = GetAuditPropertiesDictionary(dbContextAccessor.ChangeTracker, cancellationToken);
            if (dictionary.IsNotEmpty())
            {
                await dbContextAccessor.Audits.AddRangeAsync(dictionary.Keys, cancellationToken).ConfigureAndWaitAsync();

                foreach (var properties in dictionary.Values)
                    await dbContextAccessor.AuditProperties.AddRangeAsync(properties, cancellationToken).ConfigureAndWaitAsync();

                var mediator = dbContextAccessor.GetService<IMediator>();
                await mediator.Publish(new AuditNotification<TAudit, TAuditProperty>(dictionary)).ConfigureAndWaitAsync();
            }
        }


        /// <summary>
        /// 获取数据审计集合。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回字典集合。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual IDictionary<TAudit, List<TAuditProperty>> GetAuditPropertiesDictionary
            (ChangeTracker changeTracker, CancellationToken cancellationToken = default)
        {
            var entityStates = Options.AuditEntityStates;

            // 得到需要审计的实体集合
            var entityEntries = changeTracker.Entries()
                .Where(m => m.Entity.IsNotNull()
                    && !m.Metadata.ClrType.IsDefined<NonAuditedAttribute>()
                    && entityStates.Contains(m.State)).ToList();

            var dictionary = new Dictionary<TAudit, List<TAuditProperty>>();
            foreach (var entry in entityEntries)
            {
                var pair = GetAuditProperties(entry, cancellationToken);
                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// 获取审计属性集合。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回键值对。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual KeyValuePair<TAudit, List<TAuditProperty>> GetAuditProperties
            (EntityEntry entry, CancellationToken cancellationToken)
        {
            var audit = typeof(TAudit).EnsureCreate<TAudit>();
            audit.Id = GetAuditId(cancellationToken);
            audit.TableName = GetTableName(entry);
            audit.EntityTypeName = EntityPopulator.FormatTypeName(entry.Metadata.ClrType);
            audit.State = (int)entry.State;
            audit.StateName = entry.State.ToString();

            var auditProperties = new List<TAuditProperty>();
            foreach (var property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken || property.ClrType.IsDefined<NonAuditedAttribute>())
                    continue;

                if (property.IsPrimaryKey())
                    audit.EntityId = GetEntityId(entry.Property(property.Name));

                var auditProperty = typeof(TAuditProperty).EnsureCreate<TAuditProperty>();
                auditProperty.AuditId = audit.Id;
                auditProperty.PropertyName = property.Name;
                auditProperty.PropertyTypeName = EntityPopulator.FormatTypeName(property.ClrType);

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

            if (entry.State == EntityState.Modified && entry.Entity is IUpdation updation)
            {
                // 使用实体更新时间
                audit.CreatedTime = ToDateTime(updation.GetCustomUpdatedTime(), cancellationToken);
                audit.CreatedBy = ToBy(updation.GetCustomUpdatedBy());
            }
            else if (entry.Entity is ICreation creation)
            {
                // 使用实体创建时间
                audit.CreatedTime = ToDateTime(creation.GetCustomCreatedTime(), cancellationToken);
                audit.CreatedBy = ToBy(creation.GetCustomCreatedBy());
            }
            else
            {
                // 使用当前时间
                audit.CreatedTime = Dependencies.Clock.GetOffsetNowAsync(cancellationToken: cancellationToken).ConfigureAndResult();
            }

            audit.CreatedTimeTicks = audit.CreatedTime.Ticks;

            return new KeyValuePair<TAudit, List<TAuditProperty>>(audit, auditProperties);
        }

        /// <summary>
        /// 获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <typeparamref name="TGenId"/>。</returns>
        protected virtual TGenId GetAuditId(CancellationToken cancellationToken)
            => Dependencies.Identifier.GetAuditIdAsync().ConfigureAndResult();

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
        /// 转换为日期时间格式。
        /// </summary>
        /// <param name="time">给定的日期对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        protected virtual DateTimeOffset ToDateTime(object time, CancellationToken cancellationToken)
        {
            if (time.IsNull())
            {
                return Dependencies.Clock.GetOffsetNowAsync(cancellationToken: cancellationToken).ConfigureAndResult();
            }

            if (time is DateTimeOffset dateTimeOffset)
                return dateTimeOffset;

            if (time is DateTime dateTime)
                return new DateTimeOffset(dateTime);

            return DateTimeOffset.Parse(time.ToString(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 转换为人员内容。
        /// </summary>
        /// <param name="by">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string ToBy(object by)
        {
            if (by is string str)
                return str;

            return by?.ToString();
        }

    }
}
