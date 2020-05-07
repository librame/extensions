using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Resources;

    public class DataEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataEntityResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IStringLocalizer<DataEntityResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var schema = localizer.GetString(r => r.Schema);
            Assert.False(schema.ResourceNotFound);

            var name = localizer.GetString(r => r.Name);
            Assert.False(name.ResourceNotFound);

            var description = localizer.GetString(r => r.Description);
            Assert.False(description.ResourceNotFound);

            var entityName = localizer.GetString(r => r.EntityName);
            Assert.False(entityName.ResourceNotFound);

            var assemblyName = localizer.GetString(r => r.AssemblyName);
            Assert.False(assemblyName.ResourceNotFound);
        }

    }
}
