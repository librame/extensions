using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class AuditResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionStringLocalizer<AuditResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IExpressionStringLocalizer<AuditResource> localizer, string cultureName)
        {
            BuilderGlobalization.RegisterCultureInfos(cultureName);

            var entityId = localizer[r => r.EntityId];
            Assert.False(entityId.ResourceNotFound);

            var entityName = localizer[r => r.TableName];
            Assert.False(entityName.ResourceNotFound);

            var entityTypeName = localizer[r => r.EntityTypeName];
            Assert.False(entityTypeName.ResourceNotFound);

            var state = localizer[r => r.State];
            Assert.False(state.ResourceNotFound);

            var stateName = localizer[r => r.StateName];
            Assert.False(stateName.ResourceNotFound);

            var createdBy = localizer[r => r.CreatedBy];
            Assert.False(createdBy.ResourceNotFound);

            var createdTime = localizer[r => r.CreatedTime];
            Assert.False(createdTime.ResourceNotFound);

            var properties = localizer[r => r.Properties];
            Assert.False(properties.ResourceNotFound);
        }

    }
}
