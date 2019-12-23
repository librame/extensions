using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data.Accessors;
    using Data.Collections;
    using Data.Stores;
    using Models;

    public class ExampleStoreHub : StoreHub<ExampleDbContextAccessor, ExampleStoreInitializer>
    {
        public ExampleStoreHub(IStoreInitializer initializer, IAccessor accessor)
            : base(initializer, accessor)
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

        public ExampleStoreHub UseWriteDbConnection()
        {
            Accessor.ChangeDbConnection(t => t.WritingConnectionString);
            return this;
        }

        public ExampleStoreHub UseDefaultDbConnection()
        {
            Accessor.ChangeDbConnection(t => t.DefaultConnectionString);
            return this;
        }

    }
}
