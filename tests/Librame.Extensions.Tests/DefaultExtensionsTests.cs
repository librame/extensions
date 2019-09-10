using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultExtensionsTests
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
        public void RequiredResultTest()
        {
            var defaultString = nameof(DefaultExtensionsTests);
            var str = string.Empty;
            Assert.Equal(defaultString, str.RequiredNotNullOrEmpty(defaultString));
            str = " ";
            Assert.Equal(defaultString, str.RequiredNotNullOrWhiteSpace(defaultString));

            var defaultItems = Enumerable.Range(1, 9);
            var items = Enumerable.Empty<int>();
            Assert.True(defaultItems.SequenceEqual(items.RequiredNotNullOrEmpty(() => defaultItems)));

            var defaultInt = 1;
            int? i = null;
            Assert.Equal(defaultInt, i.RequiredNotNull(defaultInt));

            var defaultGuid = Guid.Empty;
            Guid? guid = null;
            Assert.Equal(defaultGuid, guid.RequiredNotNull(defaultGuid));

            var defaultClass2 = new TestClass2 { Abbr = nameof(TestClass2) };
            TestClass2 test2 = null;
            Assert.Equal(defaultClass2.Abbr, test2.RequiredNotNull(defaultClass2).Abbr);

            var defaultClass1 = new TestClass1();
            TestClass1 test1 = null;
            Assert.Throws<ArgumentException>(() =>
            {
                test1.RequiredNotNull(() => null, throwIfDefaultNotSatisfy: true);
            });

            var num = 2.RequiredNotGreater(1);
            Assert.Equal(1, num); // return compare
            num = 2.RequiredNotGreater(1, 0);
            Assert.Equal(0, num); // return default
            num = 2.RequiredNotGreater(2, 0, equals: true);
            Assert.Equal(2, num);

            num = 1.RequiredNotLesser(2);
            Assert.Equal(2, num); // return compare
            num = 1.RequiredNotLesser(2, 3);
            Assert.Equal(3, num); // return default
            num = 1.RequiredNotLesser(1, 3, equals: true);
            Assert.Equal(1, num);

            var httpPort = 0.RequiredNotOutOfRange(1, 65536, 80);
            Assert.Equal(80, httpPort);
            httpPort = 1.RequiredNotOutOfRange(1, 65536, 80, equalMinimum: true);
            Assert.Equal(80, httpPort);
            httpPort = 1.RequiredNotOutOfRange(1, 65536, 80);
            Assert.Equal(1, httpPort);
        }


        [Fact]
        public void EnsureCreateTest()
        {
            var test = typeof(TestClass1).EnsureCreateObject();
            Assert.NotNull(test);

            var test1 = typeof(TestClass2).EnsureCreate<TestClass1>();
            Assert.NotNull(test1);

            Assert.Equal((test as TestClass1).Property3, test1.Property3);

            // Change Property
            test1.Property3 = nameof(EnsureCreateTest);

            var testParameter = test1.EnsureConstruct<TestParameterCreate>();
                // == DefaultExtensions.EnsureCreate<TestParameterCreate>();
                // == typeof(TestParameterCreate).EnsureCreate<TestParameterCreate>(test1);
            Assert.NotNull(testParameter);

            Assert.Equal(testParameter.TestClass.Property3, test1.Property3);
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
            //string _field2 = null;
            public string Field3;

            static string Property1 { get; set; }
            string Property2 { get; set; }
            public string Property3 { get; set; }
        }
        class TestClass2 : TestClass1
        {
            public string Abbr { get; set; }
        }

    }
}
