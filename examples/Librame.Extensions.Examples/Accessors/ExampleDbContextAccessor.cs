﻿using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Examples
{
    using Data;
    using Models;

    public class ExampleDbContextAccessor : DbContextAccessor
    {
        public ExampleDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable();

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).HasMaxLength(256).IsRequired();

                // 关联
                b.HasMany(x => x.Articles).WithOne(x => x.Category)
                    .IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(b =>
            {
                // 使用年份进行分表（注：需要在 Article 做 [ShardingTable] 标识）
                b.ToTable(descr => descr.ChangeDateOffsetSuffixByYear());

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).HasMaxLength(256);
                b.Property(x => x.Title).HasMaxLength(256).IsRequired();
            });
        }

    }
}