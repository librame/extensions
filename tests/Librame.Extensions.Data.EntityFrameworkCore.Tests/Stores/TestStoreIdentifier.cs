using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    public class TestStoreIdentifier : AbstractStoreIdentifier
    {
        public TestStoreIdentifier(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
        {
            return GenerateIdAsync(cancellationToken, "ArticleId");
        }

    }
}
