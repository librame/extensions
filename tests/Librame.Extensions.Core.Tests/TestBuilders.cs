using System;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Dependencies;

    public class TestCoreBuilderDependency : CoreBuilderDependency
    {
        public OptionsDependency<TestOptions> Test { get; }
            = new OptionsDependency<TestOptions>();
    }
}
