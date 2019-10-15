using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class AbstractEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionLocalizer<AbstractEntityResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IExpressionLocalizer<AbstractEntityResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var id = localizer[r => r.Id];
            Assert.False(id.ResourceNotFound);

            var tenantId = localizer[r => r.TenantId];
            Assert.False(tenantId.ResourceNotFound);

            var dataRank = localizer[r => r.Rank];
            Assert.False(dataRank.ResourceNotFound);

            var dataStatus = localizer[r => r.Status];
            Assert.False(dataStatus.ResourceNotFound);
        }

    }
}
