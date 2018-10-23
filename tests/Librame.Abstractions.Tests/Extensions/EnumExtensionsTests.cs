using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EnumExtensionsTests
    {

        public enum TestEnum
        {
            [Description("一")]
            One = 1,

            [Description("二")]
            Two = 2
        }


        [Fact]
        public void AsEnumTest()
        {
            var field = TestEnum.One;

            var name = field.AsEnumName();
            Assert.Equal(nameof(TestEnum.One), name);

            var nameField = name.AsEnumField<TestEnum>();
            Assert.Equal(field, nameField);

            var value = (int)field;
            var valueField = value.AsEnumField<TestEnum>();
            Assert.Equal(field, valueField);

            var description = field.AsEnumValueWithAttribute<TestEnum, DescriptionAttribute, string>((f, a) => a.Description);
            Assert.Equal("一", description);

            var fields = typeof(TestEnum).AsEnumFields();
            Assert.True(fields.Length > 0);
        }


        [Fact]
        public void AsEnumResults()
        {
            var results = typeof(TestEnum).AsEnumResultsWithAttribute<DescriptionAttribute, object>((f, a, v) => new
            {
                Name = f.Name,
                Description = a.Description,
                Value = (int)v
            });

            Assert.True(results.IsNotEmpty());
        }


        [Fact]
        public void AsEnumTextValues()
        {
            var pairs = typeof(TestEnum).AsEnumTextValuesWithAttribute<DescriptionAttribute, string, int, KeyValuePair<string, int>>
                (
                    // Text
                    (f, a, v) => a.Description,
                    // Value
                    (f, a, v) => (int)v,
                    // Pair
                    (t, v) => new KeyValuePair<string, int>(t, v)
                );

            Assert.True(pairs.IsNotEmpty());
        }

    }
}
