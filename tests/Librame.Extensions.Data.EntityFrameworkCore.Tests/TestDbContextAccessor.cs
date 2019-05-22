using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestDbContextAccessor : DbContextAccessor
    {
        public TestDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(category =>
            {
                category.ToTable(new TableSchema<Category>());

                category.HasKey(x => x.Id);

                category.Property(x => x.Id).ValueGeneratedOnAdd();
                category.Property(x => x.Name).HasMaxLength(100).IsRequired();
                category.Property(x => x.Rank);
                category.Property(x => x.Status);

                // 关联
                category.HasMany(x => x.Articles).WithOne(x => x.Category).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(article =>
            {
                article.ToTable(TableSchema<Article>.BuildEveryYear(DateTime.Now));

                article.HasKey(x => x.Id);

                article.Property(x => x.Id).ValueGeneratedNever();
                article.Property(x => x.Title).HasMaxLength(200).IsRequired();
                article.Property(x => x.Descr).HasMaxLength(1000).IsRequired();
                article.Property(x => x.Rank);
                article.Property(x => x.Status);
            });
        }

    }
}
