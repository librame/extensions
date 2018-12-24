using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Tests
{
    using Microsoft.Extensions.Options;
    using Models;

    public interface ITestDbContext : IDbContext<TestBuilderOptions>
    {
        DbSet<Category> Categories { get; set; }

        DbSet<Article> Articles { get; set; }
    }


    public class TestDbContext : AbstractDbContext<TestDbContext, TestBuilderOptions>, ITestDbContext
    {
        public TestDbContext(IOptions<TestBuilderOptions> builderOptions, DbContextOptions<TestDbContext> dbContextOptions)
            : base(builderOptions, dbContextOptions)
        {
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(category =>
            {
                category.ToTable(BuilderOptions.CategoryTable);

                category.HasKey(x => x.Id);

                category.Property(x => x.Id).ValueGeneratedOnAdd();
                category.Property(x => x.Name).HasMaxLength(100).IsRequired();
                category.Property(x => x.DataRank);
                category.Property(x => x.DataStatus);
                category.Property(x => x.CreateTime);
                category.Property(x => x.CreatorId);
                category.Property(x => x.UpdateTime);
                category.Property(x => x.UpdatorId);

                // 关联
                category.HasMany(x => x.Articles).WithOne(x => x.Category).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(article =>
            {
                article.ToTable(BuilderOptions.ArticleTable);

                article.HasKey(x => x.Id);

                article.Property(x => x.Id).ValueGeneratedOnAdd();
                article.Property(x => x.Title).HasMaxLength(200).IsRequired();
                article.Property(x => x.Descr).HasMaxLength(1000).IsRequired();
                article.Property(x => x.DataRank);
                article.Property(x => x.DataStatus);
                article.Property(x => x.CreateTime);
                article.Property(x => x.CreatorId);
                article.Property(x => x.UpdateTime);
                article.Property(x => x.UpdatorId);
                article.Property(x => x.CategoryId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
