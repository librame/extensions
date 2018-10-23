using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Services;

    public class UnifiedTests
    {

        [Fact]
        public void UnifiedTest()
        {
            var store = TestServiceProvider.Current.GetRequiredService<ITestStore>();

            var categories = store.GetCategories();
            Assert.Empty(categories);

            // Use Write Database
            categories = store.UseWriteStore().GetCategories();
            Assert.NotEmpty(categories);

            // Restore
            store.UseDefaultStore();

            var articles = store.GetArticles();
            Assert.Empty(articles);

            // Use Write Database
            articles = store.UseWriteStore().GetArticles();
            Assert.NotEmpty(articles);

            // Restore
            store.UseDefaultStore();
        }

    }
}
