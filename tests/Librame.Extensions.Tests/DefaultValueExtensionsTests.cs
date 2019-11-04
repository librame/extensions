using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultValueExtensionsTests
    {
        [Fact]
        public void ValidateOrDefaultTest()
        {
            var defaultString = nameof(DefaultValueExtensionsTests);
            var str = string.Empty;
            Assert.Equal(defaultString, str.NotEmptyOrDefault(defaultString));
            str = " ";
            Assert.Equal(defaultString, str.NotWhiteSpaceOrDefault(defaultString));

            var defaultItems = Enumerable.Range(1, 9);
            var items = Enumerable.Empty<int>();
            Assert.True(defaultItems.SequenceEqual(items.NotEmptyOrDefault(() => defaultItems)));

            var defaultInt = 1;
            int? i = null;
            Assert.Equal(defaultInt, i.NotNullOrDefault(defaultInt));

            var defaultGuid = Guid.Empty;
            Guid? guid = null;
            Assert.Equal(defaultGuid, guid.NotNullOrDefault(defaultGuid));

            var defaultClass2 = new TestSubClass { Abbr = nameof(TestSubClass) };
            TestSubClass test2 = null;
            Assert.Equal(defaultClass2.Abbr, test2.NotNullOrDefault(defaultClass2).Abbr);

            var defaultClass1 = new TestClass();
            TestClass test1 = null;
            Assert.Throws<ArgumentException>(() =>
            {
                test1.NotNullOrDefault(() => null, throwIfDefaultInvalid: true);
            });

            var num = 2.NotGreaterOrDefault(1);
            Assert.Equal(1, num); // return compare
            num = 2.NotGreaterOrDefault(1, 0);
            Assert.Equal(0, num); // return default
            num = 2.NotGreaterOrDefault(2, 0, equals: true);
            Assert.Equal(2, num);

            num = 1.NotLesserOrDefault(2);
            Assert.Equal(2, num); // return compare
            num = 1.NotLesserOrDefault(2, 3);
            Assert.Equal(3, num); // return default
            num = 1.NotLesserOrDefault(1, 3, equals: true);
            Assert.Equal(1, num);

            var httpPort = 0.NotOutOfRangeOrDefault(1, 65536, 80);
            Assert.Equal(80, httpPort);
            httpPort = 1.NotOutOfRangeOrDefault(1, 65536, 80, equalMinimum: true);
            Assert.Equal(80, httpPort);
            httpPort = 1.NotOutOfRangeOrDefault(1, 65536, 80);
            Assert.Equal(1, httpPort);
        }

    }
}
