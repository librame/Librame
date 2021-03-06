﻿using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class LongDataStoreIdentificationGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = TestServiceProvider.Current.GetRequiredService<TestLongStoreIdentificationGenerator>();

            var id = generator.GenerateAuditId();
            Assert.NotEqual(0, id);

            id = generator.GenerateMigrationId();
            Assert.NotEqual(0, id);

            id = generator.GenerateTabulationId();
            Assert.NotEqual(0, id);

            id = generator.GenerateTenantId();
            Assert.NotEqual(0, id);

            id = generator.GenerateArticleId();
            Assert.NotEqual(0, id);
        }

    }
}
