#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    class TenantService : ExtensionBuilderServiceBase<DataBuilderOptions>, ITenantService
    {
        public TenantService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        public Task<ITenant> GetTenantAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            if (accessor is DbContextAccessor dbContextAccessor)
            {
                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    var tenant = Options.Tenants.Default;
                    Logger.LogInformation($"Get Default Tenant: Name={tenant?.Name}, Host={tenant?.Host}");

                    return tenant;
                });
            }

            return Task.FromResult((ITenant)null);
        }


        public Task<ITenant> AddTenantAsync()
        {

        }

    }
}
