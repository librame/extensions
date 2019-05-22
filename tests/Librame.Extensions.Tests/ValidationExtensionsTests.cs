using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ValidationExtensionsTests
    {
        [Fact]
        public void IsNullOrEmptyTest()
        {
            string str = null;
            Assert.True(str.IsNullOrEmpty());

            str = string.Empty;
            Assert.True(str.IsNullOrEmpty());

            Assert.False("123".IsNullOrEmpty());

            // IEnumerable
            IEnumerable<string> items = null;
            Assert.True(items.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullableTypeTest()
        {
            Assert.True(typeof(bool?).IsNullableType());
            Assert.False(typeof(bool).IsNullableType());
        }

        [Fact]
        public void IsMultiplesTest()
        {
            Assert.True(4.IsMultiples(2));
            Assert.True(9.IsMultiples(3));
            Assert.False(5.IsMultiples(9));
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
        public void IsAssignableFromTargetTest()
        {
            var baseType = typeof(IAnimal);
            var catType = typeof(Cat);
            var dogType = typeof(Dog);

            Assert.True(baseType.IsAssignableFromTarget(catType));
            Assert.True(baseType.IsAssignableFromTarget(dogType));
            Assert.False(catType.IsAssignableFromTarget(dogType));

            Assert.True(catType.IsAssignableToBase(baseType));
            Assert.True(dogType.IsAssignableToBase(baseType));
            Assert.False(catType.IsAssignableToBase(dogType));
        }

    }


    interface IAnimal
    {
    }

    class Cat : IAnimal
    {
    }

    class Dog : IAnimal
    {
    }
}
