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
    /// 租户服务。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class TenantService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : AbstractExtensionBuilderService<DataBuilderOptions>
        , ITenantService<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个租户服务。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public TenantService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        public virtual ITenant GetSwitchTenant(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                return GetSwitchTenantCore(dbContextAccessor);

            return null;
        }

        /// <summary>
        /// 获取切换的租户核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected virtual ITenant GetSwitchTenantCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
            // 默认不切换
            var defaultTenant = Options.DefaultTenant;
            Logger.LogInformation($"Get Default Tenant: Name={defaultTenant?.Name}, Host={defaultTenant?.Host}");

            return defaultTenant;
        }


        /// <summary>
        /// 异步获取切换的租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="ITenant"/> 的异步操作。</returns>
        public virtual Task<ITenant> GetSwitchTenantAsync(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                return GetSwitchTenantCoreAsync(dbContextAccessor, cancellationToken);

            return Task.FromResult((ITenant)null);
        }

        /// <summary>
        /// 异步获取切换的租户核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{ITenant}"/>。</returns>
        protected virtual Task<ITenant> GetSwitchTenantCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                // 默认不切换
                var defaultTenant = Options.DefaultTenant;
                Logger.LogInformation($"Get Default Tenant: Name={defaultTenant?.Name}, Host={defaultTenant?.Host}");

                return defaultTenant;
            });
        }

    }
}
