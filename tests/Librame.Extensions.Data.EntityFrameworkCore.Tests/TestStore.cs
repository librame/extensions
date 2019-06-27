using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public interface ITestStore : IBaseStore<TestDbContextAccessor>
    {
        IList<Category> GetCategories();

        IPagingList<Article> GetArticles();

        /// <summary>
        /// 供手动切换读取写入库测试。
        /// </summary>
        /// <returns></returns>
        ITestStore UseWriteDbConnection();

        /// <summary>
        /// 供手动切换读取写入库测试。
        /// </summary>
        /// <returns></returns>
        ITestStore UseDefaultDbConnection();
    }


    public class TestStore : AbstractBaseStore<TestDbContextAccessor>, ITestStore
    {
        public TestStore(IIdentifierService idService, IAccessor accessor)
            : base(accessor)
        {
            Initialize(idService);
        }


        private void Initialize(IIdentifierService idService)
        {
            UseWriteDbConnection();

            Category firstCategory;
            Category lastCategory;

            if (!RealAccessor.Categories.Any())
            {
                firstCategory = new Category
                {
                    Name = "First Category"
                };
                lastCategory = new Category
                {
                    Name = "Last Category"
                };

                RealAccessor.Categories.AddRange(lastCategory, firstCategory);
            }
            else
            {
                firstCategory = RealAccessor.Categories.First();
                lastCategory = RealAccessor.Categories.Last();
            }

            if (!RealAccessor.Articles.Any())
            {
                var articles = new List<Article>();

                for (int i = 0; i < 100; i++)
                {
                    articles.Add(new Article
                    {
                        Id = idService.GetIdAsync(default).Result,
                        Title = "Article " + i.ToString(),
                        Descr = "Descr " + i.ToString(),
                        Category = (i < 50) ? firstCategory : lastCategory
                    });
                }

                RealAccessor.Articles.AddRangeAsync(articles);
            }

            RealAccessor.SaveChanges();
        }


        public IList<Category> GetCategories()
        {
            return RealAccessor.Categories.ToList();
        }

        public IPagingList<Article> GetArticles()
        {
            return RealAccessor.Articles.AsPagingListByIndex(ordered => ordered.OrderBy(a => a.Id), 1, 10);
        }

        public ITestStore UseWriteDbConnection()
        {
            Accessor.ChangeDbConnection(t => t.WriteConnectionString);
            return this;
        }

        public ITestStore UseDefaultDbConnection()
        {
            Accessor.ChangeDbConnection(t => t.DefaultConnectionString);
            return this;
        }

        protected override Type GetDisposableType()
        {
            return GetType();
        }
    }
}
