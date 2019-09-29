using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestStoreHub : StoreHubBase<TestDbContextAccessor>
    {
        public TestStoreHub(IAccessor accessor, IStoreInitializer<TestDbContextAccessor> initializer)
            : base(accessor, initializer)
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

        public TestStoreHub UseWriteDbConnection()
        {
            Accessor.SwitchTenant(t => t.WritingConnectionString);
            return this;
        }

        public TestStoreHub UseDefaultDbConnection()
        {
            Accessor.SwitchTenant(t => t.DefaultConnectionString);
            return this;
        }

    }
}
