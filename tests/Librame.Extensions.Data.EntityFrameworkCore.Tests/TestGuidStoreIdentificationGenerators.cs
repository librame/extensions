using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Stores;

    public class TestGuidStoreIdentificationGenerator : GuidDataStoreIdentificationGenerator
    {
        public TestGuidStoreIdentificationGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public Guid GenerateArticleId()
            => GenerateId("ArticleId");

        public Task<Guid> GenerateArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }


    public class TestLongStoreIdentificationGenerator : LongDataStoreIdentificationGenerator
    {
        public TestLongStoreIdentificationGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public long GenerateArticleId()
            => GenerateId("ArticleId");

        public Task<long> GenerateArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }


    public class TestStringStoreIdentificationGenerator : StringDataStoreIdentificationGenerator
    {
        public TestStringStoreIdentificationGenerator(IClockService clock,
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
