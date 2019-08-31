using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DbContextAccessorTests
    {
        [Fact]
        public void AllTest()
        {
            using (var stores = TestServiceProvider.Current.GetRequiredService<TestStoreHub>())
            {
                stores.Initializer.Initialize(stores);

                var categories = stores.GetCategories();
                Assert.Empty(categories);

                categories = stores.UseWriteDbConnection().GetCategories();
                Assert.NotEmpty(categories);

                var articles = stores.UseDefaultDbConnection().GetArticles();
                Assert.Empty(articles);

                articles = stores.UseWriteDbConnection().GetArticles();
                Assert.NotEmpty(articles);
            }
        }

    }
}
