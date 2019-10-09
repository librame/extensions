using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Examples
{
    using Data;
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
            Accessor.SwitchTenant(t => t.WritingConnectionString);
            return this;
        }

        public ExampleStoreHub UseDefaultDbConnection()
        {
            Accessor.SwitchTenant(t => t.DefaultConnectionString);
            return this;
        }

    }
}
