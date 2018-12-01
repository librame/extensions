#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Builders;
using Librame.Extensions;
using Librame.Extensions.Data;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 模型构建器静态扩展。
    /// </summary>
    public static class DataModelBuilderExtensions
    {

        /// <summary>
        /// 配置抽象数据库上下文。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="builderOptions">给定的构建器选项（可选）。</param>
        public static void ConfigureAbstractDbContext<TBuilderOptions>(this ModelBuilder modelBuilder, TBuilderOptions builderOptions = null)
            where TBuilderOptions : DataBuilderOptions, new()
        {
            if (builderOptions.IsDefault())
                builderOptions = new TBuilderOptions();

            if (builderOptions.DefaultSchema.IsNotEmpty())
                modelBuilder.HasDefaultSchema(builderOptions.DefaultSchema);

            // 审计
            modelBuilder.Entity<Audit>(audit =>
            {
                audit.ToTable(builderOptions.AuditTable ?? new TableSchema<Audit>());

                audit.HasKey(x => x.Id);

                audit.Property(x => x.Id).ValueGeneratedOnAdd();
                audit.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
                audit.Property(x => x.EntityTypeName).HasMaxLength(200).IsRequired();
                audit.Property(x => x.State);
                audit.Property(x => x.StateName).HasMaxLength(50);
                audit.Property(x => x.CreatedBy).HasMaxLength(50);
                audit.Property(x => x.CreatedTime);

                // 关联
                audit.HasMany(x => x.Properties).WithOne(x => x.Audit).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            // 审计属性
            modelBuilder.Entity<AuditProperty>(auditProperty =>
            {
                auditProperty.ToShardingTable(builderOptions.AuditPropertyTable ?? new EveryWeekShardingSchema());

                auditProperty.HasKey(x => x.Id);

                auditProperty.Property(x => x.Id).ValueGeneratedOnAdd();
                auditProperty.Property(x => x.PropertyName).HasMaxLength(100).IsRequired();
                auditProperty.Property(x => x.PropertyTypeName).HasMaxLength(200).IsRequired();
                auditProperty.Property(x => x.OldValue);
                auditProperty.Property(x => x.NewValue);
                auditProperty.Property(x => x.AuditId);
            });

            // 租户
            modelBuilder.Entity<Tenant>(tenant =>
            {
                tenant.ToTable(builderOptions.TenantTable ?? new TableSchema<Tenant>());

                tenant.HasKey(x => x.Id);

                tenant.Property(x => x.Id).ValueGeneratedOnAdd();
                tenant.Property(x => x.Name).HasMaxLength(100).IsRequired();
                tenant.Property(x => x.Host).HasMaxLength(200).IsRequired();
            });
        }

    }
}
