using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data.Stores;
    using Models;

    public class ExampleStoreInitializer<TAccessor> : GuidDataStoreInitializer<TAccessor, int>
        where TAccessor : ExampleDbContextAccessorBase<Guid, int, Guid>
    {
        private readonly string _categoryName
            = typeof(Category<int, Guid, Guid>).GetGenericBodyName();
        private readonly string _articleName
            = typeof(Article<Guid, int, Guid>).GetGenericBodyName();

        private IList<Category<int, Guid, Guid>> _categories;
        private IList<Article<Guid, int, Guid>> _articles = null;


        public ExampleStoreInitializer(IStoreIdentifierGenerator identifierGenerator,
            IStoreInitializationValidator validator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, validator, loggerFactory)
        {
        }


        protected override void InitializeCore(IStoreHub stores)
        {
            base.InitializeCore(stores);

            InitializeCategories(Accessor);

            InitializeArticles(Accessor);
        }


        private void InitializeCategories(TAccessor accessor)
        {
            if (_categories.IsEmpty())
            {
                _categories = new List<Category<int, Guid, Guid>>
                {
                    new Category<int, Guid, Guid>
                    {
                        Name = $"First {_categoryName}"
                    },
                    new Category<int, Guid, Guid>
                    {
                        Name = $"Last {_categoryName}"
                    }
                };

                _categories.ForEach(category =>
                {
                    category.PopulateCreationAsync(Clock).ConfigureAwaitCompleted();
                });
            }

            // 如果当前分类未存储，则初始化保存
            accessor.Categories.TryCreateIfLocalOrDbNotAny
                (predicate: p => p.Equals(_categories.First()),
                addFactory: () => _categories,
                postAction: (_, dbExists) =>
                {
                    // 如果数据表不存在则需要保存更改
                    if (!dbExists)
                        accessor.RequiredSaveChanges = true;
                });
        }

        private void InitializeArticles(TAccessor accessor)
        {
            if (_articles.IsEmpty())
            {
                _articles = new List<Article<Guid, int, Guid>>();

                var identifier = IdentifierGenerator as ExampleStoreIdentifierGenerator;

                for (int i = 0; i < 100; i++)
                {
                    var article = new Article<Guid, int, Guid>
                    {
                        Id = identifier.GetArticleIdAsync().ConfigureAwaitCompleted(),
                        Title = $"{_articleName} {i.FormatString(3)}",
                        Descr = $"Descr {i.FormatString(3)}",
                        Category = (i < 50) ? _categories.First() : _categories.Last()
                    };

                    article.PopulateCreationAsync(Clock).ConfigureAwaitCompleted();

                    _articles.Add(article);
                }
            }

            // 如果当前文章未存储，则初始化保存
            accessor.Articles.TryCreateIfLocalOrDbNotAny
                (predicate: p => p.Equals(_articles.First()),
                addFactory: () => _articles,
                postAction: (_, dbExists) =>
                {
                    // 如果数据表不存在则需要保存更改
                    if (!dbExists)
                        accessor.RequiredSaveChanges = true;
                });
        }

    }
}
