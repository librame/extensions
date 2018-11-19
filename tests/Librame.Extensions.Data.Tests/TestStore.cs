using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;
    
    public interface ITestStore : IStore<DataBuilderOptions>
    {
        IList<Category> GetCategories();

        IPagingList<Article> GetArticles();


        ITestStore UseDefaultStore();

        ITestStore UseWriteStore();
    }


    public class TestStore : AbstractStore<ITestDbContext, DataBuilderOptions>, ITestStore
    {
        public TestStore(ITestDbContext dbContext)
            : base(dbContext)
        {
            Initialize();
        }

        private void Initialize()
        {
            UseWriteStore();

            var hasCategories = DbContext.Categories.IsNotEmpty();
            var hasArticles = DbContext.Articles.IsNotEmpty();

            if (hasCategories && hasCategories)
            {
                UseDefaultStore();
                return;
            }

            Category firstCategory;
            Category lastCategory;

            if (!hasCategories)
            {
                firstCategory = new Category
                {
                    Name = "First Category"
                };
                lastCategory = new Category
                {
                    Name = "Last Category"
                };

                DbContext.Categories.AddRange(lastCategory, firstCategory);
            }
            else
            {
                firstCategory = DbContext.Categories.First();
                lastCategory = DbContext.Categories.Last();
            }

            if (!hasArticles)
            {
                var articles = new List<Article>();

                for (int i = 0; i < 100; i++)
                {
                    articles.Add(new Article
                    {
                        Title = "Article " + i.ToString(),
                        Descr = "Descr " + i.ToString(),
                        Category = (i < 50) ? firstCategory : lastCategory
                    });
                }

                DbContext.Articles.AddRange(articles);
            }

            DbContext.SaveChanges();
        }


        public IList<Category> GetCategories()
        {
            return DbContext.Categories.ToList();
        }

        public IPagingList<Article> GetArticles()
        {
            return DbContext.Articles.AsPagingByIndex(order => order.OrderBy(a => a.Id), 1, 10);
        }


        public ITestStore UseDefaultStore()
        {
            DbContext.TrySwitchConnection(options => options.DefaultString);

            return this;
        }

        public ITestStore UseWriteStore()
        {
            DbContext.TrySwitchConnection(options => options.WriteString);

            return this;
        }
    }
}
