using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DbContextAccessorTests
    {
        [Fact]
        public void AllTest()
        {
            var store = TestServiceProvider.Current.GetRequiredService<ITestStore>();

            var categories = store.GetCategories();
            Assert.Empty(categories);

            categories = store.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = store.UseDefaultDbConnection().GetArticles();
            Assert.Empty(articles);

            articles = store.UseWriteDbConnection().GetArticles();
            Assert.NotEmpty(articles);

            //store.Dispose();
        }

    }
}
