using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ActionExtensionsTests
    {

        [Fact]
        public void NewlyTest()
        {
            var message = nameof(ActionExtensionsTests);

            Action<TestOptions> configureOptions = opts => opts.Message = message;

            var options = configureOptions.Newly();
            Assert.Equal(message, options.Message);

            configureOptions = null;
            options = configureOptions.Newly();
            Assert.True(options.Message.IsEmpty());
        }


        public class TestOptions
        {
            public string Message { get; set; }
        }

    }
}
