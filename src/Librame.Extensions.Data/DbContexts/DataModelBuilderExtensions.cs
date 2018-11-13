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

namespace Librame.Extensions
{
    using Data;

    /// <summary>
    /// 模型构建器静态扩展。
    /// </summary>
    public static class DataModelBuilderExtensions
    {

        /// <summary>
        /// 配置抽象数据库上下文。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        public static void ConfigureAbstractDbContext(this ModelBuilder modelBuilder, DataBuilderOptions builderOptions)
        {
            if (builderOptions.DefaultSchema.IsNotEmpty())
                modelBuilder.HasDefaultSchema(builderOptions.DefaultSchema);

            modelBuilder.Entity<Audit>(audit =>
            {
                audit.ToTable(builderOptions.AuditTable);

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

            modelBuilder.Entity<AuditProperty>(auditProperty =>
            {
                auditProperty.ToShardingTable(builderOptions.AuditPropertyTable);

                auditProperty.HasKey(x => x.Id);

                auditProperty.Property(x => x.Id).ValueGeneratedOnAdd();
                auditProperty.Property(x => x.PropertyName).HasMaxLength(100).IsRequired();
                auditProperty.Property(x => x.PropertyTypeName).HasMaxLength(200).IsRequired();
                auditProperty.Property(x => x.OldValue);
                auditProperty.Property(x => x.NewValue);
                auditProperty.Property(x => x.AuditId);
            });
        }

    }
}
