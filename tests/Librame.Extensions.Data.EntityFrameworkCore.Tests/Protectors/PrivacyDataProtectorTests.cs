using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Core;
    using Data.Protectors;

    public class PrivacyDataProtectorTests
    {
        [Fact]
        public void AllTest()
        {
            var protector = TestServiceProvider.Current.GetRequiredService<IPrivacyDataProtector>();

            var str = nameof(PrivacyDataProtectorTests);

            var protect = protector.Protect(str);
            Assert.NotEmpty(protect);
            Assert.True(protect.Contains(CoreSettings.Preference.KeySeparator));

            var unprotect = protector.Unprotect(protect);
            Assert.Equal(str, unprotect);
        }

    }
}
