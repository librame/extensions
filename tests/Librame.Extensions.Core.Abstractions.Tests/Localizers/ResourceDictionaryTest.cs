using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ResourceDictionaryTest
    {
        [Fact]
        public void AllTest()
        {
            var value = nameof(ResourceDictionaryTest);

            var dictionary = new ResourceDictionary();
            dictionary.GetOrAdd("Name", value);

            Assert.Equal(value, dictionary["Name"].ToString());
        }
    }
}
