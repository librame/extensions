using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data.Collections;
    using Data.Stores;
    using Models;

    public class ExampleStoreHub<TAccessor> : StoreHub<Guid, int>
        where TAccessor : ExampleDbContextAccessorBase<Guid, int>
    {
        private readonly TAccessor _currentAccessor;


        public ExampleStoreHub(IStoreInitializer<Guid> initializer, TAccessor accessor)
            : base(initializer, accessor)
        {
            _currentAccessor = accessor;
        }


        public IList<Category<int, Guid>> GetCategories()
            => _currentAccessor.Categories.ToList();

        public IPageable<Article<Guid, int>> GetArticles()
            => _currentAccessor.Articles.AsDescendingPagingByIndex(1, 10);

        public ExampleStoreHub<TAccessor> UseWriteDbConnection()
        {
            Accessor.ChangeConnectionString(t => t.WritingConnectionString);
            return this;
        }

        public ExampleStoreHub<TAccessor> UseDefaultDbConnection()
        {
            Accessor.ChangeConnectionString(t => t.DefaultConnectionString);
            return this;
        }

    }
}
