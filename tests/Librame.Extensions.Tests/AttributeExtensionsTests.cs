using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class AttributeExtensionsTests
    {
        [Fact]
        public void IsDefinedTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(IsDefinedTest));
            Assert.True(method.IsDefined<FactAttribute>());
        }


        [Fact]
        public void GetCustomAttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(GetCustomAttributeTest));
            var fact = method.GetCustomAttribute<FactAttribute>();
            Assert.NotNull(fact);
        }

        [Test]
        [Test]
        [Fact]
        public void GetCustomAttributesTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(GetCustomAttributesTest));
            var tests = method.GetCustomAttributes<TestAttribute>();
            Assert.False(tests.IsEmpty());
        }


        [Fact]
        public void TryGetCustomAttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(TryGetCustomAttributeTest));
            Assert.True(method.TryGetCustomAttribute(out FactAttribute fact));
            Assert.NotNull(fact);
        }

        [Fact]
        public void TryGetCustomAttributesTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(TryGetCustomAttributesTest));
            Assert.False(method.TryGetCustomAttributes(out IEnumerable<TestAttribute> tests));
            Assert.True(tests.IsEmpty());
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestAttribute : Attribute
    {
    }
}
