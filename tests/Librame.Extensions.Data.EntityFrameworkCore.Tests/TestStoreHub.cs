using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Accessors;
    using Collections;
    using Models;
    using Stores;

    public class TestStoreHub : StoreHub<Guid, int>
    {
        private readonly TestDbContextAccessor _currentAccessor;


        public TestStoreHub(IStoreInitializer<Guid> initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
            _currentAccessor = accessor as TestDbContextAccessor;
        }


        public IList<Category<int, Guid>> GetCategories()
            => _currentAccessor.Categories.ToList();

        public IPageable<Article<Guid, int>> GetArticles()
            => _currentAccessor.Articles.AsDescendingPagingByIndex(1, 10);

        public TestStoreHub UseWriteDbConnection()
        {
            _currentAccessor.ChangeConnectionString(t => t.WritingConnectionString);
            return this;
        }

        public TestStoreHub UseDefaultDbConnection()
        {
            _currentAccessor.ChangeConnectionString(t => t.DefaultConnectionString);
            return this;
        }

    }
}
