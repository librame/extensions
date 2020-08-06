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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Data.Accessors;
    using Data.Validators;

    /// <summary>
    /// 数据存储初始化器。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class DataStoreInitializer<TAccessor>
        : DataStoreInitializer<TAccessor, Guid, int, Guid>
        where TAccessor : class, IDataAccessor
    {
        /// <summary>
        /// 构造一个数据存储初始化器。
        /// </summary>
        /// <param name="generator">给定的 <see cref="IStoreIdentificationGenerator"/>。</param>
        /// <param name="validator">给定的 <see cref="IDataInitializationValidator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected DataStoreInitializer(IDataInitializationValidator validator,
            IStoreIdentificationGenerator generator, ILoggerFactory loggerFactory)
            : base(validator, generator, loggerFactory)
        {
        }

    }


    /// <summary>
    /// 数据存储初始化器。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataStoreInitializer<TAccessor, TGenId, TIncremId, TCreatedBy>
        : DataStoreInitializer<TAccessor,
            DataAudit<TGenId, TCreatedBy>,
            DataAuditProperty<TIncremId, TGenId>,
            DataMigration<TGenId, TCreatedBy>,
            DataTabulation<TGenId, TCreatedBy>,
            DataTenant<TGenId, TCreatedBy>,
            TGenId, TIncremId, TCreatedBy>
        where TAccessor : class, IDataAccessor<TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个数据存储初始化器。
        /// </summary>
        /// <param name="generator">给定的 <see cref="IStoreIdentificationGenerator"/>。</param>
        /// <param name="validator">给定的 <see cref="IDataInitializationValidator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected DataStoreInitializer(IDataInitializationValidator validator,
            IStoreIdentificationGenerator generator, ILoggerFactory loggerFactory)
            : base(validator, generator, loggerFactory)
        {
        }

    }


    /// <summary>
    /// 数据存储初始化器。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的数据实体类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataStoreInitializer<TAccessor, TAudit, TAuditProperty, TMigration, TTabulation, TTenant,
        TGenId, TIncremId, TCreatedBy>
        : AbstractStoreInitializer<TAccessor>
        where TAccessor : class, IDataAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant>
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
        /// 构造一个数据存储初始化器。
        /// </summary>
        /// <param name="validator">给定的 <see cref="IDataInitializationValidator"/>。</param>
        /// <param name="generator">给定的 <see cref="IStoreIdentificationGenerator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected DataStoreInitializer(IDataInitializationValidator validator,
            IStoreIdentificationGenerator generator, ILoggerFactory loggerFactory)
            : base(validator, generator, loggerFactory)
        {
            DataGenerator = generator.CastTo<IStoreIdentificationGenerator,
                IDataStoreIdentificationGenerator<TGenId>>(nameof(generator));
        }


        /// <summary>
        /// 数据标识生成器。
        /// </summary>
        /// <value>返回 <see cref="IDataStoreIdentificationGenerator{TGenId}"/>。</value>
        protected IDataStoreIdentificationGenerator<TGenId> DataGenerator { get; }


        /// <summary>
        /// 初始化存储集合。
        /// </summary>
        protected override void InitializeStores()
            => InitializeTenants();

        /// <summary>
        /// 异步初始化存储集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected override Task InitializeStoresAsync(CancellationToken cancellationToken)
            => InitializeTenantsAsync(cancellationToken);


        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        protected virtual void InitializeTenants()
        {
            // 尝试添加当前租户
            Accessor.TenantsManager.TryAdd(p => p.Equals(Accessor.CurrentTenant),
                () =>
                {
                    TTenant currentTenant;

                    if (Accessor.CurrentTenant is TTenant tenant)
                    {
                        currentTenant = tenant;
                    }
                    else
                    {
                        currentTenant = ObjectExtensions.EnsureCreate<TTenant>();

                        // 使用当前租户数据
                        Accessor.CurrentTenant.EnsurePopulate(currentTenant);
                    }

                    // 填充创建属性
                    currentTenant.PopulateCreation(Clock);

                    // 设定标识
                    currentTenant.Id = DataGenerator.GenerateTenantId();

                    Logger.LogTrace($"Add default tenant '{currentTenant}' to {Accessor.CurrentConnectionString}.");

                    return currentTenant;
                },
                addedPost =>
                {
                    if (!Accessor.RequiredSaveChanges)
                        Accessor.RequiredSaveChanges = true;
                });
        }

        /// <summary>
        /// 异步初始化租户集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task InitializeTenantsAsync(CancellationToken cancellationToken)
        {
            // 尝试添加当前租户
            return Accessor.TenantsManager.TryAddAsync(p => p.Equals(Accessor.CurrentTenant),
                async () =>
                {
                    TTenant currentTenant;

                    if (Accessor.CurrentTenant is TTenant tenant)
                    {
                        currentTenant = tenant;
                    }
                    else
                    {
                        currentTenant = ObjectExtensions.EnsureCreate<TTenant>();

                        // 使用当前租户数据
                        Accessor.CurrentTenant.EnsurePopulate(currentTenant);
                    }

                    // 填充创建属性
                    await currentTenant.PopulateCreationAsync(Clock).ConfigureAwait();

                    // 设定标识
                    currentTenant.Id = await DataGenerator.GenerateTenantIdAsync().ConfigureAwait();

                    Logger.LogTrace($"Add default tenant '{currentTenant}' to {Accessor.CurrentConnectionString}.");

                    return currentTenant;
                },
                addedPost =>
                {
                    if (!Accessor.RequiredSaveChanges)
                        Accessor.RequiredSaveChanges = true;
                },
                cancellationToken);
        }

    }
}
