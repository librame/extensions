using System;

namespace Librame.Extensions.Examples
{
    using Core.Builders;
    using Core.Dependencies;

    public class ExampleCoreBuilderDependency : CoreBuilderDependency
    {
        public OptionsDependency<ExampleOptions> Example { get; }
            = new OptionsDependency<ExampleOptions>();
    }

    public class ExampleOptions
    {
        public string Message { get; set; }
    }
}
