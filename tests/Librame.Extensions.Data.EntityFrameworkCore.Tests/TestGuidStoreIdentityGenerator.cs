using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Stores;

    public class TestGuidStoreIdentityGenerator : GuidDataStoreIdentityGenerator
    {
        public TestGuidStoreIdentityGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public Guid GenerateArticleId()
            => GenerateId("ArticleId");

        public Task<Guid> GenerateArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }
}
