using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Examples
{
    using Core.Services;
    using Data.Builders;
    using Data.Stores;

    public class ExampleStoreIdentifier : GuidStoreIdentifier
    {
        public ExampleStoreIdentifier(IOptions<DataBuilderOptions> options,
            IClockService clock, ILoggerFactory loggerFactory)
            : base(options, clock, loggerFactory)
        {
        }


        public Task<Guid> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }
}
