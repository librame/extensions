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

    /// <summary>
    /// 数据租户服务。
    /// </summary>
    public class DataTenantService : AbstractExtensionBuilderService<DataBuilderOptions>, IDataTenantService
    {
        /// <summary>
        /// 构造一个 <see cref="DataTenantService"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DataTenantService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 启用服务。
        /// </summary>
        public bool Enabled
            => Options.TenantEnabled;


        /// <summary>
        /// 获取当前租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        public virtual ITenant GetCurrentTenant(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                return GetCurrentTenantCore(dbContextAccessor);

            return null;
        }

        /// <summary>
        /// 获取当前租户核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected virtual ITenant GetCurrentTenantCore(DbContextAccessor dbContextAccessor)
        {
            var defaultTenant = Options.DefaultTenant;
            Logger.LogInformation($"Get Default Tenant: Name={defaultTenant?.Name}, Host={defaultTenant?.Host}");

            return defaultTenant;
        }


        /// <summary>
        /// 异步获取当前租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="ITenant"/> 的异步操作。</returns>
        public virtual Task<ITenant> GetCurrentTenantAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                return GetCurrentTenantCoreAsync(dbContextAccessor, cancellationToken);

            return Task.FromResult((ITenant)null);
        }

        /// <summary>
        /// 异步获取当前租户核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{ITenant}"/>。</returns>
        protected virtual Task<ITenant> GetCurrentTenantCoreAsync(DbContextAccessor dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var defaultTenant = Options.DefaultTenant;
                Logger.LogInformation($"Get Default Tenant: Name={defaultTenant?.Name}, Host={defaultTenant?.Host}");

                return defaultTenant;
            });
        }

    }
}
