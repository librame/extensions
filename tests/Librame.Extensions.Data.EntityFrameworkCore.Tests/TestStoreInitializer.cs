using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Models;
    using Stores;
    using Validators;

    public class TestStoreInitializer : DataStoreInitializer<TestDbContextAccessor>
    {
        private readonly string _categoryName
            = typeof(Category<int, Guid, Guid>).GetGenericBodyName();
        private readonly string _articleName
            = typeof(Article<Guid, int, Guid>).GetGenericBodyName();

        private IList<Category<int, Guid, Guid>> _categories = null;
        private IList<Article<Guid, int, Guid>> _articles = null;


        public TestStoreInitializer(IStoreIdentifierGenerator identifierGenerator,
            IDataInitializationValidator validator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, validator, loggerFactory)
        {
        }


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

            Accessor.CategoriesManager.TryAddRange(p => p.Equals(_categories.First()),
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

                _categories.ForEach(async category =>
                {
                    await category.PopulateCreationAsync(Clock).ConfigureAwait();
                });
            }

            return Accessor.CategoriesManager.TryAddRangeAsync(p => p.Equals(_categories.First()),
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

                var identifier = IdentifierGenerator as TestGuidStoreIdentifierGenerator;

                for (int i = 0; i < 50; i++)
                {
                    var article = new Article<Guid, int, Guid>
                    {
                        Id = identifier.GetArticleIdAsync().ConfigureAwaitCompleted(),
                        Title = $"{_articleName} {i.FormatString(3)}",
                        Descr = $"Descr {i.FormatString(3)}",
                        Category = (i < 25) ? _categories.First() : _categories.Last()
                    };

                    article.PopulateCreationAsync(Clock).ConfigureAwaitCompleted();

                    _articles.Add(article);
                }
            }

            Accessor.ArticlesManager.TryAddRange(p => p.Equals(_articles.First()),
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

                var identifier = IdentifierGenerator as TestGuidStoreIdentifierGenerator;

                for (int i = 0; i < 50; i++)
                {
                    var article = new Article<Guid, int, Guid>
                    {
                        Id = await identifier.GetArticleIdAsync().ConfigureAwait(),
                        Title = $"{_articleName} {i.FormatString(3)}",
                        Descr = $"Descr {i.FormatString(3)}",
                        Category = (i < 25) ? _categories.First() : _categories.Last()
                    };

                    await article.PopulateCreationAsync(Clock).ConfigureAwait();

                    _articles.Add(article);
                }
            }

            await Accessor.ArticlesManager.TryAddRangeAsync(p => p.Equals(_articles.First()),
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
