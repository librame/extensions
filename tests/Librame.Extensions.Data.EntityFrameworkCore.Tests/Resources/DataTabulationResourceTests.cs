using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

    public class DataTabulationResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataTabulationResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IStringLocalizer<DataTabulationResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var localized = localizer.GetString(r => r.Schema);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.TableName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.IsSharding);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Description);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.EntityName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.AssemblyName);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
