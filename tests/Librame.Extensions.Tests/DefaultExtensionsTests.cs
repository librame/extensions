using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultExtensionsTests
    {
        private string _str;

        [Fact]
        public void EnsureSingletonTest()
        {
            _str = _str.EnsureSingleton(() => "123");
            Assert.NotEmpty(_str);

            for (var i = 0; i < 10; i++)
            {
                Assert.NotEmpty(_str.EnsureSingleton(() => string.Empty));
            }
        }

        [Fact]
        public void EnsureValueTest()
        {
            // Number
            var defaultInt = 1;
            int? nullable = null;
            Assert.Equal(defaultInt, nullable.EnsureValue(defaultInt));

            // String
            var defaultString = "1";
            var str = string.Empty;
            Assert.Equal(defaultString, str.EnsureValue(defaultString));

            str = " ";
            Assert.NotEqual(defaultString, str.EnsureValue(defaultString));
        }


        class TestClass1
        {
            static string _field1 = nameof(_field1);
            string _field2;
            public string Field3;

            static string Property1 { get; set; }
            string Property2 { get; set; }
            public string Property3 { get; set; }
        }
        class TestClass2 : TestClass1
        {
            public string Abbr { get; set; }
        }

        [Fact]
        public void EnsureCloneTest()
        {
            var source = new TestClass1
            {
                Field3 = nameof(TestClass1),
                Property3 = nameof(TestClass1)
            };
            var target = source.EnsureClone(typeof(TestClass1));
            Assert.NotEqual(source, target);
        }

        [Fact]
        public void EnsurePopulateTest()
        {
            var c1 = new TestClass1
            {
                Field3 = nameof(TestClass1),
                Property3 = nameof(TestClass1)
            };
            var c2 = new TestClass2();

            c1.EnsurePopulate(c2);
            Assert.Equal(c1.Field3, c2.Field3);
            Assert.Equal(c1.Property3, c2.Property3);
        }

    }
}
