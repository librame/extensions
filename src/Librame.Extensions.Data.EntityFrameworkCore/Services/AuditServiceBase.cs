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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 审计服务基类。
    /// </summary>
    public class AuditServiceBase : AbstractService, IAuditService
    {
        /// <summary>
        /// 构造一个 <see cref="AuditServiceBase"/> 实例。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IIdentifierService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AuditServiceBase(IClockService clock, IIdentifierService identifier,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
            Identifier = identifier.NotNull(nameof(identifier));
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        protected IClockService Clock { get; }

        /// <summary>
        /// 标识符服务。
        /// </summary>
        protected IIdentifierService Identifier { get; }


        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="changeEntities">给定的 <see cref="IList{EntityEntry}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="List{BaseAudit}"/> 的异步操作。</returns>
        public virtual Task<List<Audit>> GetAuditsAsync(IList<EntityEntry> changeEntities,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var audits = new List<Audit>();

                if (changeEntities.IsNullOrEmpty())
                    return audits;

                foreach (var entry in changeEntities)
                {
                    if (entry.Metadata.ClrType.IsDefined<NotAuditedAttribute>())
                        continue; // 如果不审计，则忽略

                    var audit = ToAudit(entry, cancellationToken);
                    audits.Add(audit);
                }

                return audits;
            });
        }


        /// <summary>
        /// 转换为审计。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回审计。</returns>
        protected virtual Audit ToAudit(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            var audit = new Audit
            {
                Id = Identifier.GetAuditIdAsync(cancellationToken).Result,
                EntityName = entry.Metadata.ClrType.Name,
                EntityTypeName = entry.Metadata.ClrType.FullName,
                State = (int)entry.State,
                StateName = entry.State.ToString()
            };

            foreach (var property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken)
                    continue;

                if (property.IsPrimaryKey())
                    audit.EntityId = GetEntityId(entry.Property(property.Name));

                var auditProperty = new AuditProperty()
                {
                    Id = Identifier.GetAuditPropertyIdAsync(cancellationToken).Result,
                    PropertyName = property.Name,
                    PropertyTypeName = property.ClrType.FullName
                };

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

                audit.Properties.Add(auditProperty);
            }

            if (entry.State == EntityState.Modified && entry.Entity is IUpdation updation)
            {
                audit.CreatedTime = ToDateTime(updation.GetUpdatedTime());
                audit.CreatedBy = ToBy(updation.GetUpdatedBy());
            }
            else if (entry.Entity is ICreation creation)
            {
                audit.CreatedTime = ToDateTime(creation.GetCreatedTime());
                audit.CreatedBy = ToBy(creation.GetCreatedBy());
            }
            else
            {
                audit.CreatedTime = Clock.GetUtcNowAsync(default).Result;
            }

            return audit;
        }

        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="property">给定的 <see cref="PropertyEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetEntityId(PropertyEntry property)
        {
            if (property.EntityEntry.State == EntityState.Deleted)
                return property.OriginalValue?.ToString();

            return property.CurrentValue?.ToString();
        }

        /// <summary>
        /// 转换为日期时间格式。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        protected virtual DateTimeOffset ToDateTime(object obj)
        {
            if (obj.IsNull())
                return Clock.GetUtcNowAsync(default).Result;

            if (obj is DateTimeOffset dateTimeOffset)
                return dateTimeOffset;

            if (obj is DateTime dateTime)
                return new DateTimeOffset(dateTime);

            return DateTimeOffset.Parse(obj.ToString());
        }

        /// <summary>
        /// 转换为人员内容。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string ToBy(object obj)
        {
            if (obj is string str)
                return str;

            return obj?.ToString();
        }

    }
}
