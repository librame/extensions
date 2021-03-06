﻿using System.ComponentModel;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void AsEnumTest()
        {
            var field = TestEnum.One;

            var name = field.AsEnumName();
            Assert.Equal(nameof(TestEnum.One), name);

            var nameField = name.AsEnum<TestEnum>();
            Assert.Equal(field, nameField);

            var value = (int)field;
            var valueField = value.AsEnum<TestEnum, int>();
            Assert.Equal(field, valueField);

            var fields = typeof(TestEnum).AsEnumFields();
            Assert.False(fields.IsEmpty());
        }

        [Fact]
        public void AsEnumResultsTest()
        {
            var description = TestEnum.One.AsEnumResult(f => f.GetCustomAttribute<DescriptionAttribute>());
            Assert.Equal("一", description.Description);

            var results = typeof(TestEnum).AsEnumResults(f => f.GetCustomAttribute<DescriptionAttribute>());
            Assert.False(results.IsEmpty());
        }

        [Fact]
        public void AsEnumDictionaryTest()
        {
            var dict = typeof(TestEnum).AsEnumValuesDictionary();
            Assert.False(dict.IsEmpty());

            var dict2 = typeof(TestEnum).AsEnumDictionary(f => f.GetCustomAttribute<DescriptionAttribute>());
            Assert.False(dict2.IsEmpty());
        }

        [Fact]
        public void MatchEnumTest()
        {
            var field = TestEnum.One;

            var sameNameField = field.MatchEnum<TestEnum, TestSameNameEnum>();
            Assert.Equal(TestSameNameEnum.One, sameNameField);

            var sameValueField = field.MatchEnum<TestEnum, TestSameValueEnum, int>();
            Assert.Equal(TestSameValueEnum.First, sameValueField);
        }
    }


    public enum TestEnum
    {
        [Description("一")]
        One = 1,

        [Description("二")]
        Two = 2
    }

    public enum TestSameNameEnum
    {
        One = 0,
        Two
    }

    public enum TestSameValueEnum
    {
        First = 1,
        Second = 2
    }
}
