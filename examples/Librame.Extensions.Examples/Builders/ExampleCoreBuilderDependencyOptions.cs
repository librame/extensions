using System;

namespace Librame.Extensions.Examples
{
    using Core;

    public class ExampleCoreBuilderDependencyOptions : CoreBuilderDependencyOptions
    {
        public OptionsActionConfigurator<ExampleOptions> Example { get; }
            = new OptionsActionConfigurator<ExampleOptions>();
    }

    public class ExampleOptions
    {
        public string Message { get; set; }
    }
}
