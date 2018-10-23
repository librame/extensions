using Xunit;

namespace Librame.Extensions.Tests
{
    public class StringExtensionsTests
    {

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

        private readonly string[] _words = "one,two,three,four,five,six,seven,eight,nine,ten".Split(',');

        [Fact]
        public void AsPascalCasingTest()
        {
            var casing = _words.AsPascalCasing();

            Assert.NotEmpty(casing);
        }

        [Fact]
        public void AsCamelCasingTest()
        {
            var casing = _words.AsCamelCasing();

            Assert.NotEmpty(casing);
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

            pair = test.SplitPair(":");
            Assert.True(pair.Key == "key" && pair.Value == ":123");
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
