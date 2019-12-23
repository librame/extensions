using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Resources;

    public class DataAuditResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataAuditResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<DataAuditResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var entityId = localizer.GetString(r => r.EntityId);
            Assert.False(entityId.ResourceNotFound);

            var entityName = localizer.GetString(r => r.TableName);
            Assert.False(entityName.ResourceNotFound);

            var entityTypeName = localizer.GetString(r => r.EntityTypeName);
            Assert.False(entityTypeName.ResourceNotFound);

            var stateName = localizer.GetString(r => r.StateName);
            Assert.False(stateName.ResourceNotFound);

            var properties = localizer.GetString(r => r.Properties);
            Assert.False(properties.ResourceNotFound);
        }

    }
}
