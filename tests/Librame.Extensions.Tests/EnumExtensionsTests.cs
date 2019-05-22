using System.ComponentModel;
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
        }

        [Fact]
        public void AsEnumFieldsTest()
        {
            var fields = typeof(TestEnum).AsEnumFields();
            Assert.False(fields.IsNullOrEmpty());
        }

        [Fact]
        public void AsEnumResultsTest()
        {
            var description = TestEnum.One.AsEnumResult(f => f.GetCustomAttribute<DescriptionAttribute>());
            Assert.Equal("一", description.Description);

            var results = typeof(TestEnum).AsEnumResults(f => f.GetCustomAttribute<DescriptionAttribute>());
            Assert.False(results.IsNullOrEmpty());
        }

        [Fact]
        public void AsEnumDictionaryTest()
        {
            var dict = typeof(TestEnum).AsEnumDictionary();
            Assert.False(dict.IsNullOrEmpty());

            var dict2 = typeof(TestEnum).AsEnumDictionary(f => f.GetCustomAttribute<DescriptionAttribute>());
            Assert.False(dict2.IsNullOrEmpty());
        }
    }

    public enum TestEnum
    {
        [Description("一")]
        One = 1,

        [Description("二")]
        Two = 2
    }
}
