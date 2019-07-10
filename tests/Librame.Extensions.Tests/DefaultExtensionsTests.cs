using System;
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
        public void EnsureCreateTest()
        {
            var test = typeof(TestClass1).EnsureCreate();
            Assert.NotNull(test);

            var test1 = DefaultExtensions.EnsureCreate<TestClass1>();
            Assert.NotNull(test1);

            Assert.Equal((test as TestClass1).Property3, test1.Property3);

            // Change Property
            test1.Property3 = nameof(EnsureCreateTest);

            var testParameter = test1.EnsureCreate<TestParameterCreate>();
            Assert.NotNull(testParameter);

            Assert.Equal(testParameter.TestClass.Property3, test1.Property3);
        }

        [Fact]
        public void EnsureStringTest()
        {
            var defaultString = "1";
            var str = string.Empty;
            Assert.Equal(defaultString, str.EnsureString(defaultString));

            str = " ";
            Assert.NotEqual(defaultString, str.EnsureString(defaultString));
        }

        [Fact]
        public void EnsureValueTest()
        {
            // Number
            var defaultInt = 1;
            int? i = null;
            Assert.Equal(defaultInt, i.EnsureValue(defaultInt));

            // Guid
            var defaultGuid = Guid.Empty;
            Guid? guid = null;
            Assert.Equal(defaultGuid, guid.EnsureValue(defaultGuid));
        }


        class TestParameterCreate
        {
            public TestParameterCreate(TestClass1 testClass)
            {
                TestClass = testClass;
            }

            public TestClass1 TestClass { get; }
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
