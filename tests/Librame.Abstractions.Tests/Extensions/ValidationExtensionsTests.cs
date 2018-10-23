using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ValidationExtensionsTests
    {

        [Fact]
        public void IsDefaultTest()
        {
            // Class
            ValidationExtensionsTests c = null;
            Assert.True(c.IsDefault());

            c = new ValidationExtensionsTests();
            Assert.True(c.IsNotDefault());

            // Struct
            TimeSpan ts = default;
            Assert.True(ts.IsDefault());

            ts = TimeSpan.MinValue;
            Assert.True(ts.IsNotDefault());
        }


        [Fact]
        public void IsWhiteSpaceTest()
        {
            var str = " ";
            Assert.True(str.IsWhiteSpace());

            str = "123";
            Assert.True(str.IsNotWhiteSpace());
        }


        [Fact]
        public void IsEmptyTest()
        {
            string str = null;
            Assert.True(str.IsEmpty());

            str = string.Empty;
            Assert.True(str.IsEmpty());

            str = "123";
            Assert.True(str.IsNotEmpty());

            // IEnumerable
            IEnumerable<string> items = null;
            Assert.True(items.IsEmpty());

            items = new List<string>
            {
                "123"
            };
            Assert.True(items.IsNotEmpty());
        }


        [Fact]
        public void IsGreaterTest()
        {
            var num = 3;
            Assert.True(num.IsGreater(2));
            Assert.True(num.IsGreater(3, true));
            Assert.False(num.IsGreater(4));
        }


        [Fact]
        public void IsLesserTest()
        {
            var num = 3;
            Assert.True(num.IsLesser(4));
            Assert.True(num.IsLesser(3, true));
            Assert.False(num.IsLesser(2));
        }


        [Fact]
        public void IsOutOfRangeTest()
        {
            var num = 3;
            Assert.False(num.IsOutOfRange(1, 9));
            Assert.True(num.IsOutOfRange(3, 9, true));
            Assert.False(num.IsOutOfRange(1, 4, false, true));
            Assert.True(num.IsOutOfRange(10, 30));
        }


        [Fact]
        public void IsMultiplesTest()
        {
            Assert.True(4.IsMultiples(2));
            Assert.True(9.IsMultiples(3));
            Assert.False(5.IsMultiples(9));
        }


        [Fact]
        public void IsNullableTypeTest()
        {
            Assert.True(typeof(bool?).IsNullableType());
        }


        [Fact]
        public void IsStringTypeTest()
        {
            Assert.False(typeof(byte).IsStringType());
        }

    }
}
