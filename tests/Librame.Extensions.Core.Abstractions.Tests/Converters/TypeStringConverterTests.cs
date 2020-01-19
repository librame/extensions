using System;
using System.ComponentModel;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Converters;

    public class TypeStringConverterTests
    {
        [Fact]
        public void AllTest()
        {
            var test = typeof(TypeStringConverterTests);
            var converter = TypeStringConverter.Default;

            var value = converter.ConvertToString(test);
            Assert.NotEmpty(value);

            var source = converter.ConvertFromString<Type>(value);
            Assert.NotNull(source);
            Assert.Equal(test, source);
        }
    }
}
