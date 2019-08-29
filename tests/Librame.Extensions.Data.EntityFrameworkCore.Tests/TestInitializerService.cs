using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestInitializerService<TAccessor> : InitializerService<TAccessor, TestIdentifierService>
        where TAccessor : TestDbContextAccessor
    {
        private IList<Category> _categories;


        public TestInitializerService(IIdentifierService identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
        }


        protected override void InitializeStores(IStoreHub<TAccessor> stores)
        {
            base.InitializeStores(stores);

            InitializeCategories(stores);

            InitializeArticles(stores);
        }

        private void InitializeCategories(IStoreHub<TAccessor> stores)
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

        private void InitializeArticles(IStoreHub<TAccessor> stores)
        {
            if (!stores.Accessor.Categories.Any())
            {
                var articles = new List<Article>();

                for (int i = 0; i < 100; i++)
                {
                    articles.Add(new Article
                    {
                        Id = Identifier.GetArticleIdAsync(default).Result,
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
