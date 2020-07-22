using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Tests
{
    using Extensions.Data.Accessors;
    using Models;

    public class TestDbContextAccessor : DataDbContextAccessor
    {
        public TestDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Category<int, Guid>> Categories { get; set; }

        public DbSet<Article<Guid, int, Guid>> Articles { get; set; }


        public DbSetManager<Category<int, Guid>> CategoriesManager
            => Categories.AsManager();

        public DbSetManager<Article<Guid, int, Guid>> ArticlesManager
            => Articles.AsManager();


        protected override void OnModelCreatingCore(ModelBuilder modelBuilder)
        {
            base.OnModelCreatingCore(modelBuilder);

            var mapRelationship = Dependency.Options.Stores.MapRelationship;
            var maxLength = Dependency.Options.Stores.MaxLengthForProperties;

            modelBuilder.Entity<Category<int, Guid>>(b =>
            {
                b.ToTable();

                b.HasIndex(i => i.Name).HasName().IsUnique();

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).ValueGeneratedOnAdd();

                b.Property(x => x.Name).HasMaxLength(50).IsRequired();

                if (mapRelationship)
                {
                    b.HasMany<Article<Guid, int, Guid>>().WithOne().HasForeignKey(fk => fk.CategoryId).IsRequired();
                }
            });

            modelBuilder.Entity<Article<Guid, int, Guid>>(b =>
            {
                b.ToTable(table =>
                {
                    // 使用年份进行分表（注：需要在 Article 做 [Shardable] 标识）
                    table.AppendYearSuffix(CurrentTimestamp); // .AddYears(1)
                });

                b.HasIndex(i => new { i.CategoryId, i.Title }).HasName().IsUnique();

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(x => x.Title).HasMaxLength(256).IsRequired();

                if (maxLength > 0)
                {
                    b.Property(p => p.Descr).HasMaxLength(maxLength);
                }

                if (mapRelationship)
                {
                    b.HasMany<Category<int, Guid>>().WithOne().HasForeignKey(fk => fk.Id).IsRequired();
                }
            });
        }

    }
}
