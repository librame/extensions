using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public interface ITestStore : IBaseStore
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


    public class TestStore : AbstractBaseStore, ITestStore
    {
        public TestStore(IIdService idService, IAccessor accessor)
            : base(accessor)
        {
            Initialize(idService);
        }


        private void Initialize(IIdService idService)
        {
            if (Accessor is TestDbContextAccessor dbContextAccessor)
            {
                UseWriteDbConnection();

                Category firstCategory;
                Category lastCategory;

                if (!dbContextAccessor.Categories.Any())
                {
                    firstCategory = new Category
                    {
                        Name = "First Category"
                    };
                    lastCategory = new Category
                    {
                        Name = "Last Category"
                    };

                    dbContextAccessor.Categories.AddRange(lastCategory, firstCategory);
                }
                else
                {
                    firstCategory = dbContextAccessor.Categories.First();
                    lastCategory = dbContextAccessor.Categories.Last();
                }

                if (!dbContextAccessor.Articles.Any())
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

                    dbContextAccessor.Articles.AddRangeAsync(articles);
                }

                dbContextAccessor.SaveChanges();
            }
        }


        public IList<Category> GetCategories()
        {
            if (Accessor is TestDbContextAccessor dbContextAccessor)
                return dbContextAccessor.Categories.ToList();

            return null;
        }

        public IPagingList<Article> GetArticles()
        {
            if (Accessor is TestDbContextAccessor dbContextAccessor)
                return dbContextAccessor.Articles.AsPagingListByIndex(ordered => ordered.OrderBy(a => a.Id), 1, 10);

            return null;
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
