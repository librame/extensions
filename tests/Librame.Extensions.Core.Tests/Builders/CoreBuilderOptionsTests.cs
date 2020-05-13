using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Combiners;

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
            var dependency = TestServiceProvider.Current.GetRequiredService<CoreBuilderDependency>();
            Assert.True(ReferenceEquals(options, dependency.Options));
            Assert.Equal(options.Encoding, dependency.Options.Encoding);

            // 序列化为 JSON 文件并保存
            var filePath = Path.GetTempFileName().AsFilePathCombiner();
            filePath.WriteJson(options, options.Encoding);

            // 读取 JSON 文件并验证实例是否相同
            var readOptions = filePath.ReadJson<CoreBuilderOptions>(options.Encoding);
            Assert.Equal(readOptions.Encoding, options.Encoding);

            filePath.Delete();
        }

    }
}
