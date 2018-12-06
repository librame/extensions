using Xunit;

namespace Librame.Extensions.Tests
{
    public class AttributeExtensionsTests
    {

        [Fact]
        public void IsAttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(IsAttributeTest));

            Assert.True(method.IsAttribute<FactAttribute>());
        }


        [Fact]
        public void AsAttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(AsAttributeTest));
            var fact = method.AsAttribute<FactAttribute>();

            Assert.NotNull(fact);
        }


        [Fact]
        public void TryAsAttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(TryAsAttributeTest));

            Assert.True(method.TryAsAttribute(out FactAttribute fact));
            Assert.NotNull(fact);
        }

    }
}
