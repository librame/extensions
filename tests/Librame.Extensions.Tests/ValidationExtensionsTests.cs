using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ValidationExtensionsTests
    {
        interface IAnimal
        {
        }
        class Cat : IAnimal
        {
        }
        class Dog : IAnimal
        {
        }


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
        public void IsNullableTypeTest()
        {
            Assert.True(typeof(bool?).IsNullableType());
            Assert.False(typeof(bool).IsNullableType());
        }

        [Fact]
        public void IsAssignableFromTargetTest()
        {
            var baseType = typeof(IAnimal);
            var catType = typeof(Cat);
            var dogType = typeof(Dog);

            Assert.True(baseType.IsAssignableFromTargetType(catType));
            Assert.True(baseType.IsAssignableFromTargetType(dogType));
            Assert.False(catType.IsAssignableFromTargetType(dogType));

            Assert.True(catType.IsAssignableToBaseType(baseType));
            Assert.True(dogType.IsAssignableToBaseType(baseType));
            Assert.False(catType.IsAssignableToBaseType(dogType));
        }

        [Fact]
        public void DigitAndLetterTest()
        {
            Assert.True("012x".HasDigit());
            Assert.False("012x".IsDigit());
            Assert.True("012".IsDigit());

            Assert.True("xX".HasLower());
            Assert.False("xX".IsLower());
            Assert.True("x".IsLower());

            Assert.True("xX".HasUpper());
            Assert.False("xX".IsUpper());
            Assert.True("X".IsUpper());

            Assert.True("xX$".HasSpecial());
            Assert.False("xX$".IsSpecial());
            Assert.True("$".IsSpecial());

            Assert.True("012xX".HasLetter());
            Assert.False("012xX".IsLetter());
            Assert.True("xX".IsLetter());

            Assert.True("012xX!".HasLetterAndDigit());
            Assert.False("012xX!".IsLetterAndDigit());
            Assert.True("012xX".IsLetterAndDigit());

            Assert.True("012xX$^*".HasSafety());
            Assert.False("xX$^*".IsSafety());
            Assert.True("012xX$^*".IsSafety());
        }
    }
}
