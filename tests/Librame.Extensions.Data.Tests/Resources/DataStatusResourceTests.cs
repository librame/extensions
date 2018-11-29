using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class DataStatusResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<DataStatusResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IEnhancedStringLocalizer<DataStatusResource> localizer, string cultureName)
        {
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);

            // Groups
            var globalGroup = localizer[r => r.GlobalGroup];
            Assert.False(globalGroup.ResourceNotFound);

            var scopeGroup = localizer[r => r.ScopeGroup];
            Assert.False(scopeGroup.ResourceNotFound);

            var stateGroup = localizer[r => r.StateGroup];
            Assert.False(stateGroup.ResourceNotFound);

            // Global Group
            var _default = localizer[r => r.Default];
            Assert.False(_default.ResourceNotFound);

            var delete = localizer[r => r.Delete];
            Assert.False(delete.ResourceNotFound);

            // Scope Group
            var _public = localizer[r => r.Public];
            Assert.False(_public.ResourceNotFound);

            var _protect = localizer[r => r.Protect];
            Assert.False(_protect.ResourceNotFound);

            var _internal = localizer[r => r.Internal];
            Assert.False(_internal.ResourceNotFound);

            var _private = localizer[r => r.Private];
            Assert.False(_private.ResourceNotFound);

            // State Group
            var separation = localizer[r => r.Active];
            Assert.False(separation.ResourceNotFound);

            var pending = localizer[r => r.Pending];
            Assert.False(pending.ResourceNotFound);

            var inactive = localizer[r => r.Inactive];
            Assert.False(inactive.ResourceNotFound);

            var locking = localizer[r => r.Locking];
            Assert.False(locking.ResourceNotFound);

            var ban = localizer[r => r.Ban];
            Assert.False(ban.ResourceNotFound);
        }

    }
}
