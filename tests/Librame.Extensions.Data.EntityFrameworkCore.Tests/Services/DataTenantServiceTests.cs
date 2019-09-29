using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DataTenantServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IDataTenantService>();
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.GetCurrentTenantAsync(null).ConfigureAndWaitAsync();
            })
            .ConfigureAndWaitAsync();
        }
    }
}
