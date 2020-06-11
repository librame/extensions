using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

    public class DataTenantResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataTenantResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IStringLocalizer<DataTenantResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var localized = localizer.GetString(r => r.Name);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Host);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.DefaultConnectionString);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.WriteConnectionString);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.WriteSeparation);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.EncryptedConnectionStrings);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.DataSynchronization);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.StructureSynchronization);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
