using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
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
            var writeStore = store.UseWriteStore();
            if (writeStore.DbProvider is ITestDbContext dbContext)
            {
                // 如果是分表，则可能为空表
                if (dbContext.Articles.Any())
                {
                    articles = writeStore.GetArticles();
                    Assert.NotEmpty(articles);
                }
            }
            
            // Restore
            store.UseDefaultStore();
        }

    }
}
