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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 内部访问器模型构建器静态扩展。
    /// </summary>
    internal static class InternalAccessorModelBuilderExtensions
    {
        /// <summary>
        /// 配置基础实体集合。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        public static void ConfigureBaseEntities(this ModelBuilder modelBuilder, DataBuilderOptions options)
        {
            if (!options.DefaultSchema.IsNullOrEmpty())
                modelBuilder.HasDefaultSchema(options.DefaultSchema);

            // 审计
            modelBuilder.Entity<BaseAudit>(audit =>
            {
                audit.ToTable(options.AuditTableFactory?.Invoke(typeof(BaseAudit)));

                audit.HasKey(k => k.Id);

                audit.Property(p => p.Id).ValueGeneratedNever();
                audit.Property(p => p.EntityName).HasMaxLength(100).IsRequired();
                audit.Property(p => p.EntityTypeName).HasMaxLength(200).IsRequired();
                audit.Property(p => p.StateName).HasMaxLength(50);
                audit.Property(p => p.CreatedBy).HasMaxLength(50);

                // 关联
                audit.HasMany(p => p.Properties).WithOne(p => p.Audit).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            // 审计属性
            modelBuilder.Entity<BaseAuditProperty>(auditProperty =>
            {
                auditProperty.ToTable(options.AuditPropertyTableFactory?.Invoke(typeof(BaseAuditProperty)));

                auditProperty.HasKey(k => k.Id);

                auditProperty.Property(p => p.Id).ValueGeneratedOnAdd();
                auditProperty.Property(p => p.PropertyName).HasMaxLength(100).IsRequired();
                auditProperty.Property(p => p.PropertyTypeName).HasMaxLength(200).IsRequired();
            });

            // 租户
            modelBuilder.Entity<BaseTenant>(tenant =>
            {
                tenant.ToTable(options.TenantTableFactory?.Invoke(typeof(BaseTenant)));

                tenant.HasKey(k => k.Id);

                tenant.HasIndex(i => new { i.Name, i.Host }).HasName($"{nameof(BaseTenant)}NameHostIndex").IsUnique();

                tenant.Property(p => p.Id).ValueGeneratedNever();
                tenant.Property(p => p.Name).HasMaxLength(100).IsRequired();
                tenant.Property(p => p.Host).HasMaxLength(200).IsRequired();
            });
        }

    }
}
