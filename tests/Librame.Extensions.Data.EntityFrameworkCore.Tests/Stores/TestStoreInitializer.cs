﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestStoreInitializer : StoreInitializerBase<TestDbContextAccessor, TestStoreIdentifier>
    {
        private IList<Category> _categories;


        public TestStoreInitializer(IClockService clock,
            IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(clock, identifier, loggerFactory)
        {
        }


        protected override void InitializeCore(IStoreHub<TestDbContextAccessor> stores)
        {
            base.InitializeCore(stores);

            InitializeCategories(stores.Accessor);

            InitializeArticles(stores.Accessor);
        }

        private void InitializeCategories(TestDbContextAccessor accessor)
        {
            if (!accessor.Categories.Any())
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

                accessor.Categories.AddRange(_categories);
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

                stores.Articles.AddRange(articles);
            }
        }

    }
}
