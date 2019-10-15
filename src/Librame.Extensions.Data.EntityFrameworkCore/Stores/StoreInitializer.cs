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
using System;
using System.Globalization;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 存储初始化器。
    /// </summary>
    /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
    public class StoreInitializer<TIdentifier> : StoreInitializer
        where TIdentifier : IStoreIdentifier
    {
        /// <summary>
        /// 构造一个存储初始化器。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected StoreInitializer(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            Identifier = identifier.CastTo<IStoreIdentifier, TIdentifier>(nameof(identifier));
        }

        /// <summary>
        /// 构造一个存储初始化器。
        /// </summary>
        /// <param name="identifier">给定的 <typeparamref name="TIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected StoreInitializer(TIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            Identifier = identifier;
        }


        /// <summary>
        /// 标识符。
        /// </summary>
        /// <value>返回 <typeparamref name="TIdentifier"/>。</value>
        public new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 存储初始化器。
    /// </summary>
    public class StoreInitializer : AbstractStoreInitializer
    {
        /// <summary>
        /// 构造一个存储初始化器。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializer(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
        }


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的 <see cref="StoreHub{TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        public virtual void Initialize<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
            (StoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAccessor : IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
        {
            stores.NotNull(nameof(stores));

            if (IsInitialized)
                return;

            // 确保已切换到写入数据连接
            stores.Accessor.ChangeDbConnection(tenant => tenant.WritingConnectionString);

            Clock.Locker.WaitAction(() => InitializeCore(stores));

            if (RequiredSaveChanges)
            {
                stores.Accessor.SaveChanges();

                RequiredSaveChanges = false;
                IsInitialized = true;
            }

            // 确保已切换到默认数据连接
            stores.Accessor.ChangeDbConnection(tenant => tenant.DefaultConnectionString);
        }

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的 <see cref="StoreHub{TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected virtual void InitializeCore<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
            (StoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAccessor : IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
            => InitializeTenants(stores);


        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的 <see cref="StoreHub{TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected virtual void InitializeTenants<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
            (StoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAccessor : IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
        {
            // 注册租户需作写入请求限制
            if (!stores.Accessor.IsWritingRequest())
                return;

            // 如果当前租户未有效存储，则初始化保存
            if (!stores.ContainTenantAsync(stores.Accessor.CurrentTenant.Name,
                stores.Accessor.CurrentTenant.Host).ConfigureAndResult())
            {
                TTenant tenant;

                // 添加默认租户到数据库
                if (stores.Accessor.CurrentTenant is TTenant _tenant)
                {
                    tenant = _tenant;
                }
                else
                {
                    tenant = typeof(TTenant).EnsureCreate<TTenant>();
                    stores.Accessor.CurrentTenant.EnsurePopulate(tenant);
                }

                tenant.Id = GetTenantId<TGenId>();
                tenant.UpdatedTime = tenant.CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true).ConfigureAndResult();
                tenant.UpdatedTimeTicks = tenant.CreatedTimeTicks = tenant.UpdatedTime.Ticks.ToString(CultureInfo.InvariantCulture);
                tenant.UpdatedBy = tenant.CreatedBy = GetType().GetSimpleName();

                stores.TryCreate(tenant);
                RequiredSaveChanges = true;

                Logger.LogTrace($"Add default tenant '{tenant}' to database.");
            }
        }

        /// <summary>
        /// 获取租户标识。
        /// </summary>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <returns>返回 <typeparamref name="TGenId"/>。</returns>
        protected virtual TGenId GetTenantId<TGenId>()
            where TGenId : IEquatable<TGenId>
        {
            var tenantId = Identifier.GetTenantIdAsync().ConfigureAndResult();
            return tenantId.CastTo<string, TGenId>(nameof(tenantId));
        }
    }
}
