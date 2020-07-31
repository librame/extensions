using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Starters;

    public class PreStarterFactoryTests
    {
        [Fact]
        public void AllTest()
        {
            // Await IServiceCollection.AddLibrame() invoke IServiceCollection.UsePreStarter()
            var preStarter = (TestPreStarter)TestServiceProvider.Current
                .GetRequiredService<IEnumerable<IPreStarter>>().First();

            Assert.True(preStarter.IsStarting);
            Assert.NotEqual(DateTime.Now, preStarter.StartingTime);
            Assert.Equal(ExtensionSettings.Preference.BaseDateTime, preStarter.StartingTime);
        }

    }
}
