using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Resources;

    public class AbstractEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<AbstractEntityResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<AbstractEntityResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var id = localizer.GetString(r => r.Id);
            Assert.False(id.ResourceNotFound);

            var tenantId = localizer.GetString(r => r.TenantId);
            Assert.False(tenantId.ResourceNotFound);

            var dataRank = localizer.GetString(r => r.Rank);
            Assert.False(dataRank.ResourceNotFound);

            var dataStatus = localizer.GetString(r => r.Status);
            Assert.False(dataStatus.ResourceNotFound);
        }

    }
}
