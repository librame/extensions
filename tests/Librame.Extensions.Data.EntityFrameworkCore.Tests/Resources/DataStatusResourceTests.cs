using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class DataStatusResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataStatusResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<DataStatusResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            // Groups
            var globalGroup = localizer.GetString(r => r.GlobalGroup);
            Assert.False(globalGroup.ResourceNotFound);

            var scopeGroup = localizer.GetString(r => r.ScopeGroup);
            Assert.False(scopeGroup.ResourceNotFound);

            var stateGroup = localizer.GetString(r => r.StateGroup);
            Assert.False(stateGroup.ResourceNotFound);

            // Global Group
            var _default = localizer.GetString(r => r.Default);
            Assert.False(_default.ResourceNotFound);

            var delete = localizer.GetString(r => r.Delete);
            Assert.False(delete.ResourceNotFound);

            // Scope Group
            var _public = localizer.GetString(r => r.Public);
            Assert.False(_public.ResourceNotFound);

            var _protect = localizer.GetString(r => r.Protect);
            Assert.False(_protect.ResourceNotFound);

            var _internal = localizer.GetString(r => r.Internal);
            Assert.False(_internal.ResourceNotFound);

            var _private = localizer.GetString(r => r.Private);
            Assert.False(_private.ResourceNotFound);

            // State Group
            var separation = localizer.GetString(r => r.Active);
            Assert.False(separation.ResourceNotFound);

            var pending = localizer.GetString(r => r.Pending);
            Assert.False(pending.ResourceNotFound);

            var inactive = localizer.GetString(r => r.Inactive);
            Assert.False(inactive.ResourceNotFound);

            var locking = localizer.GetString(r => r.Locking);
            Assert.False(locking.ResourceNotFound);

            var ban = localizer.GetString(r => r.Ban);
            Assert.False(ban.ResourceNotFound);
        }

    }
}
