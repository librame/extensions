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
            var options = TestServiceProvider.Current.GetRequiredService<IOptions<CoreBuilderOptions>>().Value;
            var encoding = options.Encoding.Value;

            // Change Encoding
            options.Encoding.ChangeSource(Encoding.ASCII);

            // 对比字符串形式，如果是 Encoding 实例则相同，因为引用实例会发生变化
            Assert.NotEqual(options.Encoding.Value, encoding);

            // 依赖的选项实例同样发生了变化
            var dependencyOptions = TestServiceProvider.Current.GetRequiredService<CoreBuilderDependency>();
            Assert.Equal(options.Encoding.Value, dependencyOptions.Options.Encoding.Value);

            // 序列化为 JSON 文件并保存
            var json = JsonConvert.SerializeObject(options);
            var fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, json);

            // 读取 JSON 文件并验证实例是否相同
            json = File.ReadAllText(fileName);
            var deserializeOptions = JsonConvert.DeserializeObject<CoreBuilderOptions>(json);
            Assert.Equal(deserializeOptions.Encoding, options.Encoding);

            File.Delete(fileName);
        }
    }
}
