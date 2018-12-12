using Microsoft.Extensions.Localization;

namespace Librame.Extensions.Data.Tests
{
    /// <summary>
    /// 测试资源。
    /// </summary>
    [ResourceMapping(false)]
    public class TestResource
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }
    }
}
