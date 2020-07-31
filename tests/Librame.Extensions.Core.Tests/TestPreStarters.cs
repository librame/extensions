using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Tests
{
    using Starters;

    public class TestPreStarter : AbstractPreStarter
    {
        public DateTime StartingTime { get; private set; }


        public override IServiceCollection StartCore(IServiceCollection services)
        {
            StartingTime = ExtensionSettings.Preference.BaseDateTime;

            return services;
        }

    }
}
