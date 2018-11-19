using Xunit;

namespace Librame.Extensions.Tests
{
    public class TypeExtensionsTests
    {

        class TestClass1
        {
            public string Name { get; set; }
        }

        class TestClass2 : TestClass1
        {
            public string Abbr { get; set; }
        }


        [Fact]
        public void PopulatePropertiesTest()
        {
            var c1 = new TestClass1
            {
                Name = nameof(TestClass1)
            };

            var c2 = new TestClass2();

            c1.PopulateProperties(c2);
            
            Assert.Equal(c1.Name, c2.Name);
        }

    }
}
