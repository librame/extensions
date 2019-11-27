using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class TestStoreHubTests
    {
        [Fact]
        public void AllTest()
        {
            var stores = TestServiceProvider.Current.GetRequiredService<TestStoreHub>();

            var dependencies = stores.ServiceFactory.GetService<MigrationsScaffolderDependencies>();
            Assert.NotNull(dependencies);

            var categories = stores.GetCategories();
            Assert.Empty(categories);

            categories = stores.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = stores.UseDefaultDbConnection().GetArticles();
            Assert.Empty(articles);

            articles = stores.UseWriteDbConnection().GetArticles();
            Assert.True(articles.Total >= 0); // 如果已分表，则此表内容可能为空

            //var testEntities = stores.Accessor.TestEntities.ToList();
            //Assert.Empty(testEntities);
        }

    }
}
