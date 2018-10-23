using Xunit;

namespace Librame.Extensions.Tests
{
    public class AttributeExtensionsTests
    {

        [Fact]
        public void AttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(AttributeTest));
            var fact = method.Attribute<FactAttribute>();

            Assert.NotNull(fact);
        }


        [Fact]
        public void HasAttributeTest()
        {
            var method = typeof(AttributeExtensionsTests).GetMethod(nameof(HasAttributeTest));

            Assert.True(method.HasAttribute<FactAttribute>());
        }

    }
}
