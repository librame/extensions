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
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Stores
{
    using Data.Accessors;
    using Data.ValueGenerators;

    /// <summary>
    /// 抽象数据存储初始化器。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public abstract class AbstractDataStoreInitializer<TGenId, TIncremId, TCreatedBy>
        : AbstractDataStoreInitializer<DataAudit<TGenId, TCreatedBy>, DataAuditProperty<TIncremId, TGenId>,
            DataEntity<TGenId, TCreatedBy>, DataMigration<TGenId, TCreatedBy>, DataTenant<TGenId, TCreatedBy>,
            TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个抽象数据存储初始化器。
        /// </summary>
        /// <param name="createdByGenerator">给定的 <see cref="IDefaultValueGenerator{TCreatedBy}"/>。</param>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreInitializer(IDefaultValueGenerator<TCreatedBy> createdByGenerator,
            IStoreIdentifierGenerator<TGenId> identifierGenerator, ILoggerFactory loggerFactory)
            : base(createdByGenerator, identifierGenerator, loggerFactory)
        {
        }
    }


    /// <summary>
    /// 抽象数据存储初始化器。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的数据实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public abstract class AbstractDataStoreInitializer<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>
        : AbstractStoreInitializer<TGenId>
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId, TCreatedBy>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个抽象数据存储初始化器。
        /// </summary>
        /// <param name="createdByGenerator">给定的 <see cref="IDefaultValueGenerator{TCreatedBy}"/>。</param>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreInitializer(IDefaultValueGenerator<TCreatedBy> createdByGenerator,
            IStoreIdentifierGenerator<TGenId> identifierGenerator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, loggerFactory)
        {
            CreatedByGenerator = createdByGenerator.NotNull(nameof(createdByGenerator));
        }


        /// <summary>
        /// 创建者默认值生成器。
        /// </summary>
        public IDefaultValueGenerator<TCreatedBy> CreatedByGenerator { get; }

        /// <summary>
        /// 数据存储标识符生成器。
        /// </summary>
        protected IDataStoreIdentifierGenerator<TGenId> DataIdentifierGenerator
            => IdentifierGenerator.CastTo<IStoreIdentifierGenerator<TGenId>,
                IDataStoreIdentifierGenerator<TGenId>>(nameof(IdentifierGenerator));


        /// <summary>
        /// 设置已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected abstract void SetInitialized(IAccessor accessor);


        /// <summary>
        /// 初始化存储。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TGenId}"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public override void Initialize(IStoreHub<TGenId> stores)
        {
            stores.NotNull(nameof(stores));

            // 切换为写入数据连接
            stores.Accessor.ChangeConnectionString(tenant => tenant.WritingConnectionString);

            // 如果未能成功切换，则直接直接退出
            if (!stores.Accessor.IsWritingConnectionString())
                return;

            if (stores is IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId> dataStores)
                InitializeData(dataStores);

            if (RequiredSaveChanges)
            {
                stores.Accessor.SaveChanges();

                RequiredSaveChanges = false;

                SetInitialized(stores.Accessor);
            };

            // 还原为默认数据连接
            stores.Accessor.ChangeConnectionString(tenant => tenant.DefaultConnectionString);
        }


        /// <summary>
        /// 初始化数据。
        /// </summary>
        /// <param name="dataStores">给定的数据存储中心。</param>
        protected virtual void InitializeData(IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId> dataStores)
            => InitializeDataTenants(dataStores);

        /// <summary>
        /// 初始化数据租户集合。
        /// </summary>
        /// <param name="dataStores">给定的数据存储中心。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void InitializeDataTenants(IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId> dataStores)
        {
            // 如果当前租户未有效存储，则初始化保存
            if (!dataStores.ContainTenantAsync(dataStores.Accessor.CurrentTenant.Name,
                dataStores.Accessor.CurrentTenant.Host).ConfigureAndResult())
            {
                TTenant tenant;

                // 添加默认租户到数据库
                if (dataStores.Accessor.CurrentTenant is TTenant _tenant)
                {
                    tenant = _tenant;
                }
                else
                {
                    tenant = typeof(TTenant).EnsureCreate<TTenant>();

                    // 使用当前租户数据
                    dataStores.Accessor.CurrentTenant.EnsurePopulate(tenant);
                }

                // 填充更新数据
                var createdBy = CreatedByGenerator.GetValueAsync(GetType()).ConfigureAndResult();
                tenant.PopulateCreationAsync(Clock, createdBy).ConfigureAndResult();

                tenant.Id = DataIdentifierGenerator.GenerateTenantIdAsync().ConfigureAndResult();

                dataStores.TryCreate(tenant);
                RequiredSaveChanges = true;

                Logger.LogTrace($"Add default tenant '{tenant}' to database.");
            }
        }

    }
}
