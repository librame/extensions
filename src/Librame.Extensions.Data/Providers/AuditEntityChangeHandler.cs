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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 审计实体变化处理程序。
    /// </summary>
    public class AuditEntityChangeHandler : IEntityChangeHandler
    {
        /// <summary>
        /// 创建者名工厂方法。
        /// </summary>
        public Func<IEnumerable<PropertyEntry>, string> CreatorNameFactory { get; set; }
            = properties => properties.FirstOrDefault(p => p.Metadata.Name == "CreatorId")?.CurrentValue?.ToString();

        /// <summary>
        /// 创建时间属性工厂方法。
        /// </summary>
        public Func<IEnumerable<PropertyEntry>, PropertyEntry> CreateTimePropertyFactory { get; set; }
            = properties => properties.FirstOrDefault(p => p.Metadata.Name == "CreateTime");

        /// <summary>
        /// 更新者名工厂方法。
        /// </summary>
        public Func<IEnumerable<PropertyEntry>, string> UpdatorNameFactory { get; set; }
            = properties => properties.FirstOrDefault(p => p.Metadata.Name == "UpdatorId")?.CurrentValue?.ToString();

        /// <summary>
        /// 更新时间属性工厂方法。
        /// </summary>
        public Func<IEnumerable<PropertyEntry>, PropertyEntry> UpdateTimePropertyFactory { get; set; }
            = properties => properties.FirstOrDefault(p => p.Metadata.Name == "UpdateTime");


        /// <summary>
        /// 变化的审计实体列表。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IList{Audit}"/>。
        /// </value>
        public IList<Audit> ChangeAudits { get; protected set; }


        /// <summary>
        /// 处理变化。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        public virtual void Process(EntityEntry entry)
        {
            ChangeAudits = new List<Audit>();

            // 如果不审计，则忽略
            if (!entry.Metadata.ClrType.HasAttribute<NotAuditedAttribute>())
            {
                var audit = ToAudit(entry);
                ChangeAudits.Add(audit);
            }
        }


        /// <summary>
        /// 转换为审计。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回 <see cref="Audit"/>。</returns>
        protected virtual Audit ToAudit(EntityEntry entry)
        {
            var audit = new Audit
            {
                EntityName = TryGetEntityName(entry.Metadata.ClrType),
                EntityTypeName = entry.Metadata.ClrType.FullName,
                State = (int)entry.State,
                StateName = entry.State.ToString()
            };

            foreach (var property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken)
                    continue;

                if (property.IsPrimaryKey())
                    audit.EntityId = TryGetEntityId(entry.Property(property.Name));

                var auditProperty = new AuditProperty()
                {
                    PropertyName = TryGetPropertyName(property),
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

            var creation = TryGetCreation(entry);
            audit.CreatedBy = creation.Creator;
            audit.CreatedTime = creation.CreatedTime;

            return audit;
        }

        /// <summary>
        /// 尝试获取实体标识。
        /// </summary>
        /// <param name="property">给定的 <see cref="PropertyEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        private string TryGetEntityId(PropertyEntry property)
        {
            if (property.EntityEntry.State == EntityState.Deleted)
                return property.OriginalValue?.ToString();

            return property.CurrentValue?.ToString();
        }

        /// <summary>
        /// 尝试获取实体名称（支持描述特性）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回字符串。</returns>
        private string TryGetEntityName(Type entityType)
        {
            if (entityType.HasAttribute(out DescriptionAttribute attribute))
                return attribute.Description.AsValueOrDefault(entityType.Name);

            return entityType.Name;
        }

        /// <summary>
        /// 尝试获取属性名称（支持描述特性）。
        /// </summary>
        /// <param name="property">给定的 <see cref="IProperty"/>。</param>
        /// <returns>返回字符串。</returns>
        private string TryGetPropertyName(IProperty property)
        {
            if (property.ClrType.HasAttribute(out DescriptionAttribute attribute))
                return attribute.Description.AsValueOrDefault(property.Name);

            return property.Name;
        }

        /// <summary>
        /// 尝试获取创建信息。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回创建信息。</returns>
        private (string Creator, DateTime CreatedTime) TryGetCreation(EntityEntry entry)
        {
            var creator = string.Empty;
            var createdTime = string.Empty;

            PropertyEntry createdTimeProperty;

            if (entry.State == EntityState.Modified)
            {
                // Updator
                creator = UpdatorNameFactory?.Invoke(entry.Properties);

                // UpdateTime
                createdTimeProperty = UpdateTimePropertyFactory?.Invoke(entry.Properties);

                if (createdTimeProperty.IsNotDefault() && createdTimeProperty.CurrentValue.IsNotDefault()
                    && createdTimeProperty.Metadata.ClrType == typeof(DateTimeOffset))
                {
                    var dto = (DateTimeOffset)createdTimeProperty.CurrentValue;
                    createdTime = dto.DateTime.ToString();
                }
                else
                {
                    createdTime = createdTimeProperty?.CurrentValue?.ToString();
                }
            }
            else
            {
                // Creator
                creator = CreatorNameFactory?.Invoke(entry.Properties);

                // CreateTime
                createdTimeProperty = CreateTimePropertyFactory?.Invoke(entry.Properties);

                if (createdTimeProperty.IsNotDefault() && createdTimeProperty.OriginalValue.IsNotDefault()
                    && createdTimeProperty.Metadata.ClrType == typeof(DateTimeOffset))
                {
                    var dto = (DateTimeOffset)createdTimeProperty.OriginalValue;
                    createdTime = dto.DateTime.ToString();
                }
                else
                {
                    createdTime = createdTimeProperty?.OriginalValue?.ToString();
                }
            }

            return (creator, createdTime.IsNotEmpty() ? DateTime.Parse(createdTime) : DateTime.Now);
        }

    }

}
