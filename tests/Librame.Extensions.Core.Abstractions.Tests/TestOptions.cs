using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Core.Tests
{
    public class TestOptions
    {
        public string Name { get; set; }
    }

    public class TestConfigureOptions : IConfigureOptions<TestOptions>
    {
        public void Configure(TestOptions options)
        {
            options.Name = nameof(TestConfigureOptions);
        }
    }

    public class TestReplaceConfigureOptions : IConfigureOptions<TestOptions>
    {
        public void Configure(TestOptions options)
        {
            options.Name = nameof(TestReplaceConfigureOptions);
        }
    }
}
