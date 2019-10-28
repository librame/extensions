using Xunit;

namespace Librame.Extensions.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void EnsureLeadingTest()
        {
            var str = nameof(StringExtensionsTests);

            var testChar = '#';

            var insert = str.EnsureLeading(testChar);
            Assert.Equal($"{testChar}{str}", insert); // inserted

            insert = insert.EnsureLeading(testChar);
            Assert.Equal($"{testChar}{str}", insert); // no insert

            var testString = nameof(StringExtensions);

            insert = str.EnsureLeading(testString);
            Assert.Equal($"{str}", insert); // no insert, str contains testString
        }

        [Fact]
        public void EnsureTrailingTest()
        {
            var str = nameof(StringExtensionsTests);

            var testChar = '#';

            var append = str.EnsureTrailing(testChar);
            Assert.Equal($"{str}{testChar}", append); // appended

            append = append.EnsureTrailing(testChar);
            Assert.Equal($"{str}{testChar}", append); // no append

            var testString = nameof(StringExtensions);

            append = str.EnsureTrailing(testString);
            Assert.Equal($"{str}{testString}", append); // appended

            append = append.EnsureTrailing(testString);
            Assert.Equal($"{str}{testString}", append); // no append
        }


        #region FormatString

        [Fact]
        public void FormatStringTest()
        {
            var i = 3;
            var format = i.FormatString();
            Assert.Equal("03", format);

            format = i.FormatString(4);
            Assert.Equal("0003", format);
        }

        #endregion


        #region Naming Conventions

        private readonly string _camelCasingWords = "one,two,three,four,five,six,seven,eight,nine,ten,eleven,twelve,twentyOne";
        private readonly string _pascalCasingWords = "One,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten,Eleven,Twelve,TwentyOne";

        [Fact]
        public void AsPascalCasingTest()
        {
            var pascalCasing = _camelCasingWords.AsPascalCasing(',');
            Assert.Equal(_pascalCasingWords, pascalCasing);
        }

        [Fact]
        public void AsCamelCasingTest()
        {
            var camelCasing = _pascalCasingWords.AsCamelCasing(',');
            Assert.Equal(_camelCasingWords, camelCasing);
        }

        #endregion


        #region Singular & Plural

        private readonly string _word = "aphorism";

        [Fact]
        public void AsSingularizeTest()
        {
            var singular = "aphorisms".AsSingularize();

            Assert.Equal(_word, singular);
        }

        [Fact]
        public void AsPluralizeTest()
        {
            var plural = _word.AsPluralize();

            Assert.Equal("aphorisms", plural);
        }

        #endregion


        #region SplitPair

        [Fact]
        public void SplitPairTest()
        {
            var test = "key=123";
            var pair = test.SplitPair();
            Assert.True(pair.Key == "key" && pair.Value == "123");

            test = "key::123";
            pair = test.SplitPair("::");
            Assert.True(pair.Key == "key" && pair.Value == "123");

            pair = test.SplitPair(':');
            Assert.True(pair.Key == "key" && pair.Value == ":123");

            test = "123:456=789:987";
            pair = test.SplitPair(':');
            Assert.Equal("123", pair.Key);
            Assert.Equal("456=789:987", pair.Value);

            pair = test.SplitPairByLastIndexOf(':');
            Assert.Equal("123:456=789", pair.Key);
            Assert.Equal("987", pair.Value);

            pair = test.SplitPair(":456=789:");
            Assert.Equal("123", pair.Key);
            Assert.Equal("987", pair.Value);

            pair = test.SplitPairByLastIndexOf("=789:987");
            Assert.Equal("123:456", pair.Key);
            Assert.Empty(pair.Value);
        }

        #endregion


        #region Trim

        [Fact]
        public void TrimCommaTest()
        {
            var str = ",123,456,";
            Assert.Equal("123,456", str.TrimComma());
        }


        [Fact]
        public void TrimPeriodTest()
        {
            var str = ".123.456.";
            Assert.Equal("123.456", str.TrimPeriod());
        }


        [Fact]
        public void TrimSemicolonTest()
        {
            var str = ";123;456;";
            Assert.Equal("123;456", str.TrimSemicolon());
        }


        [Fact]
        public void TrimTest()
        {
            var str = "000abcdefg000";
            Assert.Equal("abcdefg", str.Trim("000"));
        }


        [Fact]
        public void TrimStartTest()
        {
            var str = "000abcdefg000";
            Assert.Equal("abcdefg000", str.TrimStart("000"));
        }


        [Fact]
        public void TrimEndTest()
        {
            var str = "000abcdefg000";
            Assert.Equal("000abcdefg", str.TrimEnd("000"));
        }

        #endregion

    }
}
