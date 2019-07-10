using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestInitializerService : InitializerServiceBase<TestDbContextAccessor>
    {
        public TestInitializerService(IIdentifierService identifierService, ILoggerFactory loggerFactory)
            : base(identifierService, loggerFactory)
        {
        }


        public override void Initialize(IStoreHub<TestDbContextAccessor> storeHub)
        {
            base.Initialize(storeHub);


        }

        protected virtual void InitializeCategories(IStoreHub<TestDbContextAccessor> storeHubBase)
        {
            var firstCategory = new Category
            {
                Name = "First Category"
            };

            var lastCategory = new Category
            {
                Name = "Last Category"
            };
        }

        protected virtual void InitializeArticles(IStoreHub<TestDbContextAccessor> storeHubBase)
        {
            var articles = new List<Article>();

            for (int i = 0; i < 100; i++)
            {
                var articleId = identifierService.GetArticleIdAsync(default).Result;

                articles.Add(new Article
                {
                    Id = articleId,
                    Title = "Article " + i.ToString(),
                    Descr = "Descr " + i.ToString(),
                    Category = (i < 50) ? firstCategory : lastCategory
                });
            }

            Accessor.Articles.AddRange(articles);
        }

    }
}
