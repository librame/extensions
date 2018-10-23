using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestStore : ITestStore
    {
        private readonly ITestDbContext _dbContext;

        public TestStore(ITestDbContext dbContext)
        {
            _dbContext = dbContext;

            Initialize();
        }

        private void Initialize()
        {
            UseWriteStore();

            var hasCategories = _dbContext.Categories.IsNotEmpty();
            var hasArticles = _dbContext.Articles.IsNotEmpty();

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

                _dbContext.Categories.AddRange(lastCategory, firstCategory);
            }
            else
            {
                firstCategory = _dbContext.Categories.First();
                lastCategory = _dbContext.Categories.Last();
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
                
                _dbContext.Articles.AddRange(articles);
            }

            _dbContext.SaveChanges();
        }


        public IList<Category> GetCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public IPagingList<Article> GetArticles()
        {
            return _dbContext.Articles.AsPagingByIndex(order => order.OrderBy(a => a.Id), 1, 10);
        }


        public ITestStore UseDefaultStore()
        {
            _dbContext.TrySwitchConnection(options => options.DefaultString);

            return this;
        }

        public ITestStore UseWriteStore()
        {
            _dbContext.TrySwitchConnection(options => options.WriteString);

            return this;
        }
    }


    public interface ITestStore
    {
        IList<Category> GetCategories();

        IPagingList<Article> GetArticles();


        ITestStore UseDefaultStore();

        ITestStore UseWriteStore();
    }
}
