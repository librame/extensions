using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Services;
    using Data.Builders;
    using Data.Stores;

    public class TestGuidStoreIdentifierGenerator : GuidStoreIdentifierGenerator
    {
        public TestGuidStoreIdentifierGenerator(IOptions<DataBuilderOptions> options,
            IClockService clock, ILoggerFactory loggerFactory)
            : base(options, clock, loggerFactory)
        {
        }


        public Task<Guid> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateGenericIdAsync("ArticleId", cancellationToken);
    }
}
