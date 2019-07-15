using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class TestIdentifierService : IdentifierServiceBase
    {
        public TestIdentifierService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public virtual Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string articleId = UniqueIdentifier.New();
                Logger.LogInformation($"Get ArticleId: {articleId}");

                return articleId;
            });
        }

    }
}
