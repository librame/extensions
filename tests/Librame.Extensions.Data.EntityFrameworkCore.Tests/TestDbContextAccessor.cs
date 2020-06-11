using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Tests
{
    using Accessors;
    using Models;

    public class TestDbContextAccessor : TestDbContextAccessor<Guid, int, Guid>
    {
        public TestDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }
    }

    public class TestDbContextAccessor<TGenId, TIncremId, TCreatedBy> : DbContextAccessor<TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        public TestDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Category<TIncremId, TGenId, TCreatedBy>> Categories { get; set; }

        public DbSet<Article<TGenId, TIncremId, TCreatedBy>> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var maxLength = Dependency.Options.Stores.MaxLengthForProperties;

            modelBuilder.Entity<Category<TIncremId, TGenId, Guid>>(b =>
            {
                b.ToTable();

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).ValueGeneratedOnAdd();

                if (maxLength > 0)
                {
                    b.Property(x => x.Name).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                }

                // 关联
                if (Dependency.Options.Stores.MapRelationship)
                {
                    b.HasMany(x => x.Articles).WithOne(x => x.Category)
                        .IsRequired().OnDelete(DeleteBehavior.Cascade);
                }
            });

            modelBuilder.Entity<Article<TGenId, TIncremId, Guid>>(b =>
            {
                b.ToTable(table =>
                {
                    // 使用年份进行分表（注：需要在 Article 做 [ShardingTable] 标识）
                    table.AppendYearSuffix(CurrentTimestamp);
                });

                b.HasKey(x => x.Id);

                if (maxLength > 0)
                {
                    b.Property(x => x.Id).HasMaxLength(maxLength);
                    b.Property(x => x.Title).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                }

                // MaxLength
                b.Property(p => p.Descr);
            });
        }

    }
}
