using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Stores;

    public class TestStringStoreIdentityGenerator : StringDataStoreIdentityGenerator
    {
        public TestStringStoreIdentityGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public string GenerateArticleId()
            => GenerateId("ArticleId");

        public Task<string> GenerateArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }
}
