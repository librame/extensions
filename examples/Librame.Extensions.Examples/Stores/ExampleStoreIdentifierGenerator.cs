using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Examples
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Stores;

    public class ExampleStoreIdentifierGenerator : GuidDataStoreIdentificationGenerator
    {
        public ExampleStoreIdentifierGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public Guid GetArticleId()
            => GenerateId("ArticleId");

        public Task<Guid> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }
}
