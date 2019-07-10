using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DbContextAccessorTests
    {
        [Fact]
        public void AllTest()
        {
            using (var store = TestServiceProvider.Current.GetRequiredService<ITestStoreHub>())
            {
                var mediator = store.GetRequiredService<Core.IMediator>();
                store.Accessor.AuditNotificationAction = notification => mediator.Publish(notification);

                var categories = store.GetCategories();
                Assert.Empty(categories);

                categories = store.UseWriteDbConnection().GetCategories();
                Assert.NotEmpty(categories);

                var articles = store.UseDefaultDbConnection().GetArticles();
                Assert.Empty(articles);

                articles = store.UseWriteDbConnection().GetArticles();
                Assert.NotEmpty(articles);
            }
        }

    }
}
