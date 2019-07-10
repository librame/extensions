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
    /// 内部审计服务。
    /// </summary>
    internal class InternalAuditService : AbstractService, IAuditService
    {
        private readonly IClockService _clockService;
        private readonly IIdentifierService _identifierService;


        /// <summary>
        /// 构造一个 <see cref="InternalAuditService"/> 实例。
        /// </summary>
        /// <param name="clockService">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifierService">给定的 <see cref="IIdentifierService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalAuditService(IClockService clockService, IIdentifierService identifierService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _clockService = clockService.NotNull(nameof(clockService));
            _identifierService = identifierService.NotNull(nameof(identifierService));
        }


        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="changeEntities">给定的 <see cref="IList{EntityEntry}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="List{BaseAudit}"/> 的异步操作。</returns>
        public Task<List<Audit>> GetAuditsAsync(IList<EntityEntry> changeEntities,
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
        private Audit ToAudit(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            var audit = new Audit
            {
                Id = _identifierService.GetAuditIdAsync(cancellationToken).Result,
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
                    Id = _identifierService.GetAuditPropertyIdAsync(cancellationToken).Result,
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
                audit.CreatedTime = _clockService.GetUtcNowAsync(default).Result;
            }

            return audit;
        }

        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="property">给定的 <see cref="PropertyEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        private string GetEntityId(PropertyEntry property)
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
        private DateTimeOffset ToDateTime(object obj)
        {
            if (obj is DateTimeOffset dateTimeOffset)
                return dateTimeOffset;

            if (obj is DateTime dateTime)
                return new DateTimeOffset(dateTime);

            return _clockService.GetUtcNowAsync(default).Result;
        }

        /// <summary>
        /// 转换为人员内容。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        private string ToBy(object obj)
        {
            if (obj is int i)
                return i.ToString();

            if (obj is long l)
                return l.ToString();

            if (obj is string str)
                return str;

            return obj?.ToString();
        }

    }
}
