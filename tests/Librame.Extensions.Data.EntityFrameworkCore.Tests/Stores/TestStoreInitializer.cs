using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestStoreInitializer : StoreInitializerBase<TestDbContextAccessor, TestStoreIdentifier>
    {
        private IList<Category> _categories;


        public TestStoreInitializer(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
        }


        protected override void InitializeCore(IStoreHub<TestDbContextAccessor> stores)
        {
            base.InitializeCore(stores);

            InitializeCategories(stores);

            InitializeArticles(stores);
        }

        private void InitializeCategories(IStoreHub<TestDbContextAccessor> stores)
        {
            if (!stores.Accessor.Categories.Any())
            {
                _categories = new List<Category>
                {
                    new Category
                    {
                        Name = "First Category"
                    },
                    new Category
                    {
                        Name = "Last Category"
                    }
                };

                stores.Accessor.Categories.AddRange(_categories);
            }
            else
            {
                _categories = stores.Accessor.Categories.ToList();
            }
        }

        private void InitializeArticles(IStoreHub<TestDbContextAccessor> stores)
        {
            if (!stores.Accessor.Categories.Any())
            {
                var articles = new List<Article>();

                for (int i = 0; i < 100; i++)
                {
                    var articleId = Identifier.GetArticleIdAsync().Result;

                    articles.Add(new Article
                    {
                        Id = articleId,
                        Title = "Article " + i.ToString(),
                        Descr = "Descr " + i.ToString(),
                        Category = (i < 50) ? _categories.First() : _categories.Last()
                    });
                }

                stores.Accessor.Articles.AddRange(articles);
            }
        }

    }
}
