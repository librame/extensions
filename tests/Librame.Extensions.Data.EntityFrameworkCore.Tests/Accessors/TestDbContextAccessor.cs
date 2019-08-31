using Microsoft.EntityFrameworkCore;

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
                category.ToTable(type => type.AsTableSchema());

                category.HasKey(x => x.Id);

                category.Property(x => x.Id).ValueGeneratedOnAdd();
                category.Property(x => x.Name).HasMaxLength(256).IsRequired();

                // 关联
                category.HasMany(x => x.Articles).WithOne(x => x.Category).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(article =>
            {
                article.ToTable(type => type.AsDateTimeTableSchema(now => now.ToString("yy")));

                article.HasKey(x => x.Id);

                article.Property(x => x.Id).HasMaxLength(256);
                article.Property(x => x.Title).HasMaxLength(256).IsRequired();
            });
        }

    }
}
