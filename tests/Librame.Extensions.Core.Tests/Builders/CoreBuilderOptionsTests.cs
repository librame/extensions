using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Builders;

    public class CoreBuilderOptionsTests
    {
        [Fact]
        public void SerializeTest()
        {
            var dependency = TestServiceProvider.Current.GetRequiredService<CoreBuilderDependency>();
            var options = TestServiceProvider.Current.GetRequiredService<IOptions<CoreBuilderOptions>>().Value;

            // Change Encoding
            options.Encoding.ChangeSource(Encoding.ASCII);
            Assert.Equal(options.Encoding, dependency.Builder.Options.Encoding);

            var optionsJsonFromDependency = JsonConvert.SerializeObject(dependency.Builder.Options);
            var optionsJson = JsonConvert.SerializeObject(options);
            Assert.Equal(optionsJson, optionsJsonFromDependency);

            var fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, optionsJson);

            optionsJson = File.ReadAllText(fileName);
            var deserializeOptions = JsonConvert.DeserializeObject<CoreBuilderOptions>(optionsJson);
            Assert.Equal(deserializeOptions.Encoding, options.Encoding);

            File.Delete(fileName);
        }
    }
}
