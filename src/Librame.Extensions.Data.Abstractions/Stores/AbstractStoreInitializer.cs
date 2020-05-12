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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public abstract class AbstractStoreInitializer<TGenId> : AbstractService, IStoreInitializer<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer{TGenId}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier{TGenId}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreInitializer(IStoreIdentifier<TGenId> identifier, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Identifier = identifier.NotNull(nameof(identifier));
        }


        /// <summary>
        /// 标识符。
        /// </summary>
        public IStoreIdentifier<TGenId> Identifier { get; }

        /// <summary>
        /// 时钟服务。
        /// </summary>
        public IClockService Clock
            => Identifier.Clock;


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// 需要保存变化。
        /// </summary>
        public bool RequiredSaveChanges { get; protected set; }


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的存储中心。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual void Initialize<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TIncremId>
            (IStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TIncremId : IEquatable<TIncremId>
        {
            stores.NotNull(nameof(stores));

            // 切换为写入数据连接
            stores.Accessor.ChangeConnectionString(tenant => tenant.WritingConnectionString);

            // 如果未能成功切换，则直接直接退出
            if (!stores.Accessor.IsWritingConnectionString())
                return;

            InitializeCore(stores);

            if (RequiredSaveChanges)
            {
                stores.Accessor.SaveChanges();

                RequiredSaveChanges = false;
                IsInitialized = true;
            };

            // 还原为默认数据连接
            stores.Accessor.ChangeConnectionString(tenant => tenant.DefaultConnectionString);
        }

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的存储中心。</param>
        protected virtual void InitializeCore<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TIncremId>
            (IStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TIncremId : IEquatable<TIncremId>
            => InitializeTenants(stores);

        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的存储中心。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void InitializeTenants<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TIncremId>
            (IStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TIncremId : IEquatable<TIncremId>
        {
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

                tenant.Id = Identifier.GetTenantIdAsync().ConfigureAndResult();

                tenant.UpdatedTime = tenant.CreatedTime = Clock.GetNowOffsetAsync()
                    .ConfigureAndResult();
                tenant.UpdatedTimeTicks = tenant.CreatedTimeTicks = tenant.UpdatedTime.Ticks;
                tenant.UpdatedBy = tenant.CreatedBy = EntityPopulator.FormatTypeName(GetType());

                stores.TryCreate(tenant);
                RequiredSaveChanges = true;

                Logger.LogTrace($"Add default tenant '{tenant}' to database.");
            }
        }

    }
}
