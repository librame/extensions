using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Localizations;

    public class MigrationAuditResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionStringLocalizer<MigrationAuditResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IExpressionStringLocalizer<MigrationAuditResource> localizer, string cultureName)
        {
            LocalizationRegistration.Register(cultureName);

            var name = localizer[r => r.SnapshotHash];
            Assert.False(name.ResourceNotFound);

            var host = localizer[r => r.SnapshotCode];
            Assert.False(host.ResourceNotFound);
        }

    }
}
