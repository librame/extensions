using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestStoreHub : StoreHubBase<TestDbContextAccessor>, ITestStoreHub
    {
        public TestStoreHub(IAccessor accessor)
            : base(accessor)
        {
        }


        public IList<Category> GetCategories()
        {
            return Accessor.Categories.ToList();
        }

        public IPageable<Article> GetArticles()
        {
            return Accessor.Articles.AsDescendingPagingByIndex(1, 10);
        }

        public ITestStoreHub UseWriteDbConnection()
        {
            Accessor.TryChangeDbConnection(t => t.WritingConnectionString);
            return this;
        }

        public ITestStoreHub UseDefaultDbConnection()
        {
            Accessor.TryChangeDbConnection(t => t.DefaultConnectionString);
            return this;
        }

    }


    public interface ITestStoreHub : IStoreHub<TestDbContextAccessor>
    {
        IList<Category> GetCategories();

        IPageable<Article> GetArticles();

        /// <summary>
        /// 供手动切换读取写入库测试。
        /// </summary>
        /// <returns></returns>
        ITestStoreHub UseWriteDbConnection();

        /// <summary>
        /// 供手动切换读取写入库测试。
        /// </summary>
        /// <returns></returns>
        ITestStoreHub UseDefaultDbConnection();
    }

}
