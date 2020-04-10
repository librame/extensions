using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Examples
{
    using Data.Stores;
    using Core.Services;

    public class ExampleStoreIdentifier : GuidStoreIdentifier
    {
        public ExampleStoreIdentifier(IClockService clock, ILoggerFactory loggerFactory)
            : base(clock, loggerFactory)
        {
        }


        public Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombGuidAsync("ArticleId", cancellationToken);
    }
}
