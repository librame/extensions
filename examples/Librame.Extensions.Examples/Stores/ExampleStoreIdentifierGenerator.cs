﻿using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Examples
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Stores;

    public class ExampleStoreIdentifierGenerator : GuidDataStoreIdentityGenerator
    {
        public ExampleStoreIdentifierGenerator(IClockService clock,
            IIdentityGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        public Guid GetArticleId()
            => GenerateId("ArticleId");

        public Task<Guid> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }
}
