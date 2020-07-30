using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Extensions.Data.Stores;
    using Extensions.Data.Validators;
    using Models;

    public class TestStoreInitializer : DataStoreInitializer<TestDbContextAccessor>
    {
        private readonly string _categoryName
            = typeof(Category<int, Guid>).GetGenericBodyName();
        private readonly string _articleName
            = typeof(Article<Guid, int, Guid>).GetGenericBodyName();

        private IList<Category<int, Guid>> _categories = null;
        private IList<Article<Guid, int, Guid>> _articles = null;


        public TestStoreInitializer(IStoreIdentityGenerator identityGenerator,
            IDataInitializationValidator validator, ILoggerFactory loggerFactory)
            : base(identityGenerator, validator, loggerFactory)
        {
        }


        private int ProgressiveIncremId(int index)
            => ++index;


        protected override void InitializeStores()
        {
            base.InitializeStores();

            InitializeCategories();

            InitializeArticles();
        }

        protected override async Task InitializeStoresAsync(CancellationToken cancellationToken)
        {
            await base.InitializeStoresAsync(cancellationToken).ConfigureAwait();

            await InitializeCategoriesAsync(cancellationToken).ConfigureAwait();

            await InitializeArticlesAsync(cancellationToken).ConfigureAwait();
        }


        private void InitializeCategories()
        {
            if (_categories.IsEmpty())
            {
                _categories = new List<Category<int, Guid>>
                {
                    new Category<int, Guid>
                    {
                        Name = $"First {_categoryName}"
                    },
                    new Category<int, Guid>
                    {
                        Name = $"Last {_categoryName}"
                    }
                };

                _categories.ForEach(category =>
                {
                    category.PopulateCreation(Clock);
                });
            }

            Accessor.CategoriesManager.TryAddRange(p => p.Equals(_categories[0]),
                () => _categories,
                addedPost =>
                {
                    if (!Accessor.RequiredSaveChanges)
                        Accessor.RequiredSaveChanges = true;
                });
        }

        private Task InitializeCategoriesAsync(CancellationToken cancellationToken)
        {
            if (_categories.IsEmpty())
            {
                _categories = new List<Category<int, Guid>>
                {
                    new Category<int, Guid>
                    {
                        Name = $"First {_categoryName}"
                    },
                    new Category<int, Guid>
                    {
                        Name = $"Last {_categoryName}"
                    }
                };

                _categories.ForEach(async category =>
                {
                    await category.PopulateCreationAsync(Clock).ConfigureAwait();
                });
            }

            return Accessor.CategoriesManager.TryAddRangeAsync(p => p.Equals(_categories[0]),
                () => _categories,
                addedPost =>
                {
                    if (!Accessor.RequiredSaveChanges)
                        Accessor.RequiredSaveChanges = true;
                },
                cancellationToken);
        }


        private void InitializeArticles()
        {
            if (_articles.IsEmpty())
            {
                _articles = new List<Article<Guid, int, Guid>>();

                var generator = IdentityGenerator as TestGuidStoreIdentityGenerator;

                for (int i = 0; i < 10; i++)
                {
                    var category = (i < 5) ? _categories[0] : _categories[_categories.Count - 1];
                    var categoryIndex = (i < 5) ? 0 : 1;

                    var article = new Article<Guid, int, Guid>
                    {
                        Id = generator.GenerateArticleId(),
                        Title = $"{_articleName} {i.FormatString(2)}",
                        Descr = $"Descr {i.FormatString(2)}",
                        CategoryId = category.Id.Equals(0)
                            ? ProgressiveIncremId(categoryIndex)
                            : category.Id
                    };

                    article.PopulateCreation(Clock);

                    _articles.Add(article);
                }
            }

            Accessor.ArticlesManager.TryAddRange(p => p.Equals(_articles[0]),
                () => _articles,
                addedPost =>
                {
                    if (!Accessor.RequiredSaveChanges)
                        Accessor.RequiredSaveChanges = true;
                });
        }

        private async Task InitializeArticlesAsync(CancellationToken cancellationToken)
        {
            if (_articles.IsEmpty())
            {
                _articles = new List<Article<Guid, int, Guid>>();

                var identifier = IdentityGenerator as TestGuidStoreIdentityGenerator;

                for (int i = 0; i < 10; i++)
                {
                    var category = (i < 5) ? _categories[0] : _categories[_categories.Count - 1];
                    var categoryIndex = (i < 5) ? 0 : 1;

                    var article = new Article<Guid, int, Guid>
                    {
                        Id = await identifier.GenerateArticleIdAsync().ConfigureAwait(),
                        Title = $"{_articleName} {i.FormatString(2)}",
                        Descr = $"Descr {i.FormatString(2)}",
                        CategoryId = category.Id.Equals(0)
                            ? ProgressiveIncremId(categoryIndex)
                            : category.Id
                    };

                    await article.PopulateCreationAsync(Clock).ConfigureAwait();

                    _articles.Add(article);
                }
            }

            await Accessor.ArticlesManager.TryAddRangeAsync(p => p.Equals(_articles[0]),
                () => _articles,
                addedPost =>
                {
                    if (!Accessor.RequiredSaveChanges)
                        Accessor.RequiredSaveChanges = true;
                },
                cancellationToken).ConfigureAwait();
        }

    }
}
