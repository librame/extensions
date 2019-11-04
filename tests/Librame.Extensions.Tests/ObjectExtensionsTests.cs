using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void EnsureSingletonTest()
        {
            string str = null;

            str = str.EnsureSingleton(() => DateTime.Now.ToString());
            Assert.NotEmpty(str);

            for (var i = 0; i < 10; i++)
            {
                Assert.NotEmpty(str.EnsureSingleton(() => string.Empty));
            }
        }


        [Fact]
        public void EnsureCreateTest()
        {
            var test = typeof(TestClass).EnsureCreateObject();
            Assert.NotNull(test);

            var test1 = typeof(TestSubClass).EnsureCreate<TestClass>();
            Assert.NotNull(test1);

            Assert.Equal((test as TestClass).Property3, test1.Property3);

            // Change Property
            test1.Property3 = nameof(EnsureCreateTest);

            var testParameter = test1.EnsureConstruct<TestClassReference>();
            // == DefaultExtensions.EnsureCreate<TestClassReference>();
            // == typeof(TestParameterCreate).EnsureCreate<TestClassReference>(test1);
            Assert.NotNull(testParameter);

            Assert.Equal(testParameter.TestClass.Property3, test1.Property3);
        }


        [Fact]
        public void EnsureCloneTest()
        {
            var source = new TestClass
            {
                Field3 = nameof(TestClass),
                Property3 = nameof(TestClass)
            };

            var target = source.EnsureClone(typeof(TestClass));
            Assert.NotEqual(source, target);
        }


        [Fact]
        public void EnsurePopulateTest()
        {
            var c1 = new TestClass
            {
                Field3 = nameof(TestClass),
                Property3 = nameof(TestClass)
            };

            var c2 = new TestSubClass();

            c1.EnsurePopulate(c2);

            Assert.Equal(c1.Field3, c2.Field3);
            Assert.Equal(c1.Property3, c2.Property3);
        }
    }
}
