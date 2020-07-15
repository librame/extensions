using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data.Accessors;
    using Data.Collections;
    using Data.Stores;
    using Models;

    public class ExampleStoreHub<TAccessor> : DataStoreHub<TAccessor>
        where TAccessor : ExampleDbContextAccessorBase<Guid, int, Guid>
    {
        public ExampleStoreHub(IStoreInitializer initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
        }


        public IList<Category<int, Guid, Guid>> GetCategories()
            => Accessor.Categories.ToList();

        public IPageable<Article<Guid, int, Guid>> GetArticles()
            => Accessor.Articles.AsDescendingPagingByIndex(1, 10);

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
