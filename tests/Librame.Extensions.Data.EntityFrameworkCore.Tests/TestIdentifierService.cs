using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    /// <summary>
    /// 测试标识符服务。
    /// </summary>
    public class TestIdentifierService : IdentifierServiceBase
    {
        /// <summary>
        /// 构造一个 <see cref="TestIdentifierService"/> 实例。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public TestIdentifierService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取文章标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public virtual Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string articleId = GuIdentifier.New();
                Logger.LogInformation($"Get ArticleId: {articleId}");

                return articleId;
            });
        }

    }
}
