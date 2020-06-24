using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Accessors;
    using Collections;
    using Models;
    using Stores;

    public class TestStoreHub : DataStoreHub<TestDbContextAccessor, Guid, int, Guid>
    {
        public TestStoreHub(IStoreInitializer initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
        }


        public IList<Category<int, Guid, Guid>> GetCategories()
            => Accessor.Categories.ToList();

        public IPageable<Article<Guid, int, Guid>> GetArticles()
            => Accessor.Articles.AsDescendingPagingByIndex(1, 10);

        public TestStoreHub UseWriteDbConnection()
        {
            Accessor.ChangeConnectionString(t => t.WritingConnectionString);
            return this;
        }

        public TestStoreHub UseDefaultDbConnection()
        {
            Accessor.ChangeConnectionString(t => t.DefaultConnectionString);
            return this;
        }

    }
}
