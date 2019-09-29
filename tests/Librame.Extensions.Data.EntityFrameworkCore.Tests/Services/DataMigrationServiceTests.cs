using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DataMigrationServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IDataMigrationService>();
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.MigrateAsync(null).ConfigureAndWaitAsync();
            })
            .ConfigureAndWaitAsync();
        }
    }
}
