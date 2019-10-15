using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class DataAuditResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionLocalizer<DataAuditResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IExpressionLocalizer<DataAuditResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var entityId = localizer[r => r.EntityId];
            Assert.False(entityId.ResourceNotFound);

            var entityName = localizer[r => r.TableName];
            Assert.False(entityName.ResourceNotFound);

            var entityTypeName = localizer[r => r.EntityTypeName];
            Assert.False(entityTypeName.ResourceNotFound);

            var stateName = localizer[r => r.StateName];
            Assert.False(stateName.ResourceNotFound);

            var properties = localizer[r => r.Properties];
            Assert.False(properties.ResourceNotFound);
        }

    }
}
