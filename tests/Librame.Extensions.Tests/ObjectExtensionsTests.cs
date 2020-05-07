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
            var test = (TestClass)typeof(TestClass).EnsureCreateObject();
            Assert.NotNull(test);

            var testSub = typeof(TestSubClass).EnsureCreate<TestSubClass>();
            Assert.NotNull(testSub);

            Assert.Equal(test.Property3, testSub.Property3);

            // Change Property
            testSub.Property3 = nameof(EnsureCreateTest);

            var testSubReference = testSub.EnsureConstruct<TestSubClassReference>();
            Assert.NotNull(testSubReference);

            Assert.Equal(testSubReference.TestSub.Property3, testSub.Property3);
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
