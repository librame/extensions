using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class DataEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionLocalizer<DataEntityResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IExpressionLocalizer<DataEntityResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var schema = localizer[r => r.Schema];
            Assert.False(schema.ResourceNotFound);

            var name = localizer[r => r.Name];
            Assert.False(name.ResourceNotFound);

            var description = localizer[r => r.Description];
            Assert.False(description.ResourceNotFound);

            var entityName = localizer[r => r.EntityName];
            Assert.False(entityName.ResourceNotFound);

            var assemblyName = localizer[r => r.AssemblyName];
            Assert.False(assemblyName.ResourceNotFound);
        }

    }
}
