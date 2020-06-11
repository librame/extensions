using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;
    using Stores;
    using ValueGenerators;

    public class TestStoreInitializer : GuidDataStoreInitializer<int>
    {
        private readonly string _categoryName
            = typeof(Category<int, Guid, Guid>).GetGenericBodyName();
        private readonly string _articleName
            = typeof(Article<Guid, int, Guid>).GetGenericBodyName();

        private IList<Category<int, Guid, Guid>> _categories;


        public TestStoreInitializer(IDefaultValueGenerator<Guid> createdByGenerator,
            IStoreIdentifierGenerator<Guid> identifierGenerator,
            ILoggerFactory loggerFactory)
            : base(createdByGenerator, identifierGenerator, loggerFactory)
        {
        }


        protected override void InitializeData(IDataStoreHub<DataAudit<Guid, Guid>,
            DataAuditProperty<int, Guid>, DataEntity<Guid, Guid>, DataMigration<Guid, Guid>,
            DataTenant<Guid, Guid>, Guid> dataStores)
        {
            base.InitializeData(dataStores);

            if (dataStores.Accessor is TestDbContextAccessor dbContextAccessor)
            {
                InitializeCategories(dbContextAccessor);

                InitializeArticles(dbContextAccessor);
            }
        }

        private void InitializeCategories(TestDbContextAccessor accessor)
        {
            if (!accessor.Categories.Any())
            {
                _categories = new List<Category<int, Guid, Guid>>
                {
                    new Category<int, Guid, Guid>
                    {
                        Name = $"First {_categoryName}",
                        CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult(),
                        CreatedBy = CreatedByGenerator.GetValueAsync(GetType()).ConfigureAndResult()
                    },
                    new Category<int, Guid, Guid>
                    {
                        Name = $"Last {_categoryName}",
                        CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult(),
                        CreatedBy = CreatedByGenerator.GetValueAsync(GetType()).ConfigureAndResult()
                    }
                };

                _categories.ForEach(category =>
                {
                    category.CreatedTimeTicks = category.CreatedTime.Ticks;
                });

                accessor.Categories.AddRange(_categories);
                RequiredSaveChanges = true;
            }
            else
            {
                _categories = accessor.Categories.ToList();
            }
        }

        private void InitializeArticles(TestDbContextAccessor stores)
        {
            if (!stores.Categories.Any())
            {
                var articles = new List<Article<Guid, int, Guid>>();
                var identifier = IdentifierGenerator as TestGuidStoreIdentifierGenerator;

                for (int i = 0; i < 100; i++)
                {
                    var article = new Article<Guid, int, Guid>
                    {
                        Id = identifier.GetArticleIdAsync().ConfigureAndResult(),
                        Title = $"{_articleName} {i.FormatString(3)}",
                        Descr = $"Descr {i.FormatString(3)}",
                        Category = (i < 50) ? _categories.First() : _categories.Last(),
                        CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult(),
                        CreatedBy = CreatedByGenerator.GetValueAsync(GetType()).ConfigureAndResult()
                    };

                    article.CreatedTimeTicks = article.CreatedTime.Ticks;

                    articles.Add(article);
                }

                stores.Articles.AddRange(articles);
                RequiredSaveChanges = true;
            }
        }

    }
}
