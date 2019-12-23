using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data.Stores;
    using Models;

    public class ExampleStoreInitializer : StoreInitializer<ExampleStoreIdentifier>
    {
        private IList<Category> _categories;


        public ExampleStoreInitializer(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
        }


        protected override void InitializeCore<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
            (StoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
        {
            base.InitializeCore(stores);

            if (stores.Accessor is ExampleDbContextAccessor dbContextAccessor)
            {
                InitializeCategories(dbContextAccessor);

                InitializeArticles(dbContextAccessor);
            }
        }

        private void InitializeCategories(ExampleDbContextAccessor accessor)
        {
            if (!accessor.Categories.Any())
            {
                _categories = new List<Category>
                {
                    new Category
                    {
                        Name = $"First {nameof(Category)}",
                        CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true).ConfigureAndResult(),
                        CreatedBy = GetType().GetDisplayName()
                    },
                    new Category
                    {
                        Name = $"Last {nameof(Category)}",
                        CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true).ConfigureAndResult(),
                        CreatedBy = GetType().GetDisplayName()
                    }
                };

                _categories.ForEach(category =>
                {
                    category.CreatedTimeTicks = category.CreatedTime.Ticks.ToString(CultureInfo.InvariantCulture);
                });

                accessor.Categories.AddRange(_categories);
                RequiredSaveChanges = true;
            }
            else
            {
                _categories = accessor.Categories.ToList();
            }
        }

        private void InitializeArticles(ExampleDbContextAccessor stores)
        {
            if (!stores.Categories.Any())
            {
                var articles = new List<Article>();

                for (int i = 0; i < 10; i++)
                {
                    var article = new Article
                    {
                        Id = Identifier.GetArticleIdAsync().ConfigureAndResult(),
                        Title = $"{nameof(Article)} {i.FormatString(3)}",
                        Descr = $"{nameof(Article.Descr)} {i.FormatString(3)}",
                        Category = (i < 5) ? _categories.First() : _categories.Last(),
                        CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true).ConfigureAndResult(),
                        CreatedBy = GetType().GetDisplayName()
                    };

                    article.CreatedTimeTicks = article.CreatedTime.Ticks.ToString(CultureInfo.InvariantCulture);

                    articles.Add(article);
                }

                stores.Articles.AddRange(articles);
                RequiredSaveChanges = true;
            }
        }

    }
}
