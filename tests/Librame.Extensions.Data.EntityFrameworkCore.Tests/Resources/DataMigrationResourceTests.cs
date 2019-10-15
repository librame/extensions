using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class DataMigrationResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionLocalizer<DataMigrationResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IExpressionLocalizer<DataMigrationResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var accessorName = localizer[r => r.AccessorName];
            Assert.False(accessorName.ResourceNotFound);

            var modelSnapshotName = localizer[r => r.ModelSnapshotName];
            Assert.False(modelSnapshotName.ResourceNotFound);

            var modelHash = localizer[r => r.ModelHash];
            Assert.False(modelHash.ResourceNotFound);

            var modelBody = localizer[r => r.ModelBody];
            Assert.False(modelBody.ResourceNotFound);
        }

    }
}
