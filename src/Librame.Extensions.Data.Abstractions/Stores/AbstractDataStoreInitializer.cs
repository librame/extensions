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
    using Core.Services;

    /// <summary>
    /// 抽象数据存储初始化器。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public abstract class AbstractDataStoreInitializer<TGenId, TIncremId, TCreatedBy>
        : AbstractDataStoreInitializer<DataAudit<TGenId, TCreatedBy>,
            DataAuditProperty<TIncremId, TGenId>,
            DataEntity<TGenId, TCreatedBy>,
            DataMigration<TGenId, TCreatedBy>,
            DataTenant<TGenId, TCreatedBy>,
            TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个抽象数据存储初始化器。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="validator">给定的 <see cref="IStoreInitializationValidator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreInitializer(IStoreIdentifierGenerator identifierGenerator,
            IStoreInitializationValidator validator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, validator, loggerFactory)
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
    public abstract class AbstractDataStoreInitializer<TAudit, TAuditProperty, TEntity, TMigration, TTenant,
        TGenId, TIncremId, TCreatedBy>
        : AbstractStoreInitializer
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
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="validator">给定的 <see cref="IStoreInitializationValidator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreInitializer(IStoreIdentifierGenerator identifierGenerator,
            IStoreInitializationValidator validator, ILoggerFactory loggerFactory)
            : base(validator, identifierGenerator, loggerFactory)
        {
            DataIdentifierGenerator = identifierGenerator.CastTo<IStoreIdentifierGenerator,
                AbstractDataStoreIdentifierGenerator<TGenId>>(nameof(identifierGenerator));

            Clock = DataIdentifierGenerator.Clock;
        }


        /// <summary>
        /// 数据标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="AbstractDataStoreIdentifierGenerator{TGenId}"/>。</value>
        protected AbstractDataStoreIdentifierGenerator<TGenId> DataIdentifierGenerator { get; }

        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        protected IClockService Clock { get; }


        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub"/>。</param>
        protected override void InitializeCore(IStoreHub stores)
        {
            if (stores is IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant> dataStores)
                InitializeData(dataStores);
        }

        /// <summary>
        /// 初始化数据。
        /// </summary>
        /// <param name="dataStores">给定的数据存储中心。</param>
        protected virtual void InitializeData(IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant> dataStores)
            => InitializeDataTenants(dataStores);

        /// <summary>
        /// 初始化数据租户集合。
        /// </summary>
        /// <param name="dataStores">给定的数据存储中心。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual void InitializeDataTenants(IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant> dataStores)
        {
            if (dataStores.ContainTenantAsync(dataStores.Accessor.CurrentTenant).ConfigureAndResult())
                return;

            // 如果当前租户未存储，则初始化保存
            TTenant currentTenant;

            // 添加默认租户到数据库
            if (dataStores.Accessor.CurrentTenant is TTenant tenant)
            {
                currentTenant = tenant;
            }
            else
            {
                currentTenant = typeof(TTenant).EnsureCreate<TTenant>();

                // 使用当前租户数据
                dataStores.Accessor.CurrentTenant.EnsurePopulate(currentTenant);
            }

            // 填充创建属性
            currentTenant.PopulateCreationAsync(Clock).ConfigureAndResult();

            // 设定标识
            currentTenant.Id = DataIdentifierGenerator.GenerateTenantIdAsync().ConfigureAndResult();

            // 创建租户
            dataStores.TryCreate(currentTenant);

            RequiredSaveChanges = true;

            Logger.LogTrace($"Add default tenant '{currentTenant}' to database.");
        }

    }
}
