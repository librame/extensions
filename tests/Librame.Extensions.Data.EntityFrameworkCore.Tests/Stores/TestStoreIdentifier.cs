using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class TestStoreIdentifier : StoreIdentifierBase
    {
        public TestStoreIdentifier(IClockService clock, ILoggerFactory loggerFactory)
            : base(clock, loggerFactory)
        {
        }


        public Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
        {
            return GenerateCombGuidAsync(cancellationToken, "ArticleId");
        }

    }
}
