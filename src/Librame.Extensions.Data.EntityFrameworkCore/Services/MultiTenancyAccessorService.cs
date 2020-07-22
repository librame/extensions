#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Services
{
    using Core.Services;
    using Data.Accessors;
    using Data.Builders;
    using Data.Stores;

    /// <summary>
    /// 多租户访问器服务。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class MultiTenancyAccessorService<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
        : AbstractExtensionBuilderService<DataBuilderOptions>, IMultiTenancyAccessorService
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTabulation : DataTabulation<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个多租户访问器服务。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public MultiTenancyAccessorService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 获取当前租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        public virtual ITenant GetCurrentTenant(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                return GetCurrentTenantCore(dbContextAccessor);

            return null;
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

            if (accessor is DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                return GetCurrentTenantCoreAsync(dbContextAccessor, cancellationToken);

            return Task.FromResult((ITenant)null);
        }


        /// <summary>
        /// 获取当前租户核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected virtual ITenant GetCurrentTenantCore(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            // 默认不切换
            var defaultTenant = Options.DefaultTenant;
            Logger.LogInformation($"Get Default Tenant: Name={defaultTenant?.Name}, Host={defaultTenant?.Host}");

            return defaultTenant;
        }

        /// <summary>
        /// 异步获取当前租户核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task{ITenant}"/>。</returns>
        protected virtual Task<ITenant> GetCurrentTenantCoreAsync(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                // 默认不切换
                var defaultTenant = Options.DefaultTenant;
                Logger.LogInformation($"Get Default Tenant: Name={defaultTenant?.Name}, Host={defaultTenant?.Host}");

                return defaultTenant;
            });
        }

    }
}
