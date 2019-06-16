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
using Microsoft.EntityFrameworkCore.Metadata;
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
    internal class InternalAuditService : AbstractService<InternalAuditService>, IAuditService
    {
        private readonly IClockService _clock;
        private readonly IIdService _identification;


        /// <summary>
        /// 构造一个 <see cref="InternalAuditService"/> 实例。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identification">给定的 <see cref="IIdService"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalAuditService}"/>。</param>
        public InternalAuditService(IClockService clock, IIdService identification,
            ILogger<InternalAuditService> logger)
            : base(logger)
        {
            _clock = clock.NotNull(nameof(clock));
            _identification = identification.NotNull(nameof(identification));
        }


        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="changeEntities">给定的 <see cref="IList{EntityEntry}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="List{BaseAudit}"/> 的异步操作。</returns>
        public Task<List<BaseAudit>> GetAuditsAsync(IList<EntityEntry> changeEntities,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var audits = new List<BaseAudit>();

            if (changeEntities.IsNullOrEmpty())
                return Task.FromResult(audits);

            foreach (var entry in changeEntities)
            {
                if (entry.Metadata.ClrType.IsDefined<NotAuditedAttribute>())
                    continue; // 如果不审计，则忽略

                var audit = ToAudit(entry);
                audits.Add(audit);
            }

            return Task.FromResult(audits);
        }


        /// <summary>
        /// 转换为审计。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回审计。</returns>
        private BaseAudit ToAudit(EntityEntry entry)
        {
            var audit = new BaseAudit
            {
                Id = _identification.GetIdAsync(default).Result,
                EntityName = GetEntityName(entry.Metadata.ClrType),
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

                var auditProperty = new BaseAuditProperty()
                {
                    PropertyName = GetPropertyName(property),
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

            SetCreationOrUpdation(audit, entry);

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
        /// 获取实体名称（支持描述特性）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回字符串。</returns>
        private string GetEntityName(Type entityType)
        {
            //if (entityType.TryGetCustomAttribute(out DescriptionAttribute attribute))
            //    return attribute.Description.HasOrDefault(entityType.Name);

            return entityType.Name;
        }

        /// <summary>
        /// 获取属性名称（支持描述特性）。
        /// </summary>
        /// <param name="property">给定的 <see cref="IProperty"/>。</param>
        /// <returns>返回字符串。</returns>
        private string GetPropertyName(IProperty property)
        {
            //if (property.ClrType.TryGetCustomAttribute(out DescriptionAttribute attribute))
            //    return attribute.Description.HasOrDefault(property.Name);

            return property.Name;
        }

        /// <summary>
        /// 设置创建或更新属性。
        /// </summary>
        /// <param name="audit">给定要编辑的审计对象。</param>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        private void SetCreationOrUpdation(BaseAudit audit, EntityEntry entry)
        {
            if (entry.State == EntityState.Modified && entry.Entity is IUpdation updation)
            {
                audit.CreatedTime = ToDateTime(updation.GetUpdatedTime());
                audit.CreatedBy = ToBy(updation.GetUpdatedBy());
                return;
            }

            if (entry.Entity is ICreation creation)
            {
                audit.CreatedTime = ToDateTime(creation.GetCreatedTime());
                audit.CreatedBy = ToBy(creation.GetCreatedBy());
            }
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

            return _clock.GetUtcNowAsync(default).Result;
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
