using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Stores;

    public class TestLongStoreIdentityGenerator : LongDataStoreIdentityGenerator
    {
        public TestLongStoreIdentityGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public long GenerateArticleId()
            => GenerateId("ArticleId");

        public Task<long> GenerateArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }
}
