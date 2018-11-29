using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class AbstractEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<AbstractEntityResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IEnhancedStringLocalizer<AbstractEntityResource> localizer, string cultureName)
        {
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);

            var id = localizer[r => r.Id];
            Assert.False(id.ResourceNotFound);

            var tenantId = localizer[r => r.TenantId];
            Assert.False(tenantId.ResourceNotFound);

            var dataRank = localizer[r => r.DataRank];
            Assert.False(dataRank.ResourceNotFound);

            var dataStatus = localizer[r => r.DataStatus];
            Assert.False(dataStatus.ResourceNotFound);

            var createTime = localizer[r => r.CreateTime];
            Assert.False(createTime.ResourceNotFound);

            var creatorId = localizer[r => r.CreatorId];
            Assert.False(creatorId.ResourceNotFound);

            var updateTime = localizer[r => r.UpdateTime];
            Assert.False(updateTime.ResourceNotFound);

            var updatorId = localizer[r => r.UpdatorId];
            Assert.False(updatorId.ResourceNotFound);
        }

    }
}
