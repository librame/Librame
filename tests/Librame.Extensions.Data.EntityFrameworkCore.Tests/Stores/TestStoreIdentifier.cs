﻿using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Services;
    using Stores;

    public class TestStoreIdentifier : StoreIdentifier
    {
        public TestStoreIdentifier(IClockService clock, ILoggerFactory loggerFactory)
            : base(clock, loggerFactory)
        {
        }


        public Task<string> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombGuidAsync("ArticleId", cancellationToken);
    }
}
