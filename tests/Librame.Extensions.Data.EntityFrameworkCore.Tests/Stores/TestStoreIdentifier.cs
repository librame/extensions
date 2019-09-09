using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    public class TestStoreIdentifier : StoreIdentifierBase
    {
        public TestStoreIdentifier(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
        {
            return GenerateCombGuidAsync(cancellationToken, "ArticleId");
        }

    }
}
