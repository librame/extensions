using System;

namespace Librame.Extensions.Data.Tests
{
    using Builders;

    public class TestBuilderOptions : DataBuilderOptions
    {
        /// <summary>
        /// 分类表选项。
        /// </summary>
        public ITableSchema CategoryTable { get; set; }

        /// <summary>
        /// 文章表选项。
        /// </summary>
        public IShardingSchema ArticleTable { get; set; }
    }
}
