using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data.Stores;
    using Models;

    public class ExampleStoreInitializer<TAccessor> : GuidStoreInitializer
        where TAccessor : ExampleDbContextAccessorBase<Guid, int>
    {
        private readonly string _categoryName
            = typeof(Category<int, Guid>).GetGenericBodyName();
        private readonly string _articleName
            = typeof(Article<Guid, int>).GetGenericBodyName();

        private readonly string _createdBy;

        private IList<Category<int, Guid>> _categories;


        public ExampleStoreInitializer(IStoreIdentifierGenerator<Guid> identifierGenerator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, loggerFactory)
        {
            _createdBy = EntityPopulator.FormatTypeName(GetType());
        }


        protected override void InitializeCore<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TIncremId>
            (IStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, Guid, TIncremId> stores)
        {
            base.InitializeCore(stores);

            if (stores.Accessor is TAccessor dbContextAccessor)
            {
                InitializeCategories(dbContextAccessor);

                InitializeArticles(dbContextAccessor);
            }
        }

        private void InitializeCategories(TAccessor accessor)
        {
            if (!accessor.Categories.Any())
            {
                _categories = new List<Category<int, Guid>>
                {
                    new Category<int, Guid>
                    {
                        Name = $"First {_categoryName}",
                        CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult(),
                        CreatedBy = _createdBy
                    },
                    new Category<int, Guid>
                    {
                        Name = $"Last {_categoryName}",
                        CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult(),
                        CreatedBy = _createdBy
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

        private void InitializeArticles(TAccessor stores)
        {
            if (!stores.Categories.Any())
            {
                var articles = new List<Article<Guid, int>>();
                var identifier = IdentifierGenerator as ExampleStoreIdentifierGenerator;

                for (int i = 0; i < 100; i++)
                {
                    var article = new Article<Guid, int>
                    {
                        Id = identifier.GetArticleIdAsync().ConfigureAndResult(),
                        Title = $"{_articleName} {i.FormatString(3)}",
                        Descr = $"Descr {i.FormatString(3)}",
                        Category = (i < 50) ? _categories.First() : _categories.Last(),
                        CreatedTime = Clock.GetNowOffsetAsync().ConfigureAndResult(),
                        CreatedBy = GetType().GetDisplayName()
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
