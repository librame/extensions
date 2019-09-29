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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public abstract class AbstractStoreInitializer<TAccessor> : AbstractStoreInitializer, IStoreInitializer<TAccessor>
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer{TAccessor}"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStoreInitializer(IClockService clock,
            IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(clock, identifier, loggerFactory)
        {
        }


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor, TAudit, TEntity, TMigration, TTenant}"/>。</param>
        public virtual void Initialize<TAudit, TEntity, TMigration, TTenant>(IStoreHub<TAccessor, TAudit, TEntity, TMigration, TTenant> stores)
            where TAudit : DataAudit
            where TEntity : DataEntity
            where TMigration : DataMigration
            where TTenant : DataTenant
        {
            if (IsInitialized)
                return;

            stores.NotNull(nameof(stores));

            // 提前切换为写入数据库，以便支持数据重复验证
            if (stores.Accessor.SwitchTenant(tenant => tenant.WritingConnectionString))
            {
                Clock.Locker.WaitAction(() =>
                {
                    InitializeCore(stores);
                });

                if (IsCreated)
                {
                    // 已重写，保存时会自行尝试还原为读取数据库
                    stores.Accessor.SaveChanges();
                    IsCreated = false;
                }
                else
                {
                    // 未创建则手动切换为默认数据库
                    stores.Accessor.SwitchTenant(tenant => tenant.DefaultConnectionString);
                }

                IsInitialized = true;
            }
        }

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor, TAudit, TEntity, TMigration, TTenant}"/>。</param>
        protected abstract void InitializeCore<TAudit, TEntity, TMigration, TTenant>(IStoreHub<TAccessor, TAudit, TEntity, TMigration, TTenant> stores)
            where TAudit : DataAudit
            where TEntity : DataEntity
            where TMigration : DataMigration
            where TTenant : DataTenant;
    }


    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    public abstract class AbstractStoreInitializer : IStoreInitializer
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStoreInitializer(IClockService clock,
            IStoreIdentifier identifier, ILoggerFactory loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
            Identifier = identifier.NotNull(nameof(identifier));
            LoggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        /// <summary>
        /// 时钟。
        /// </summary>
        public IClockService Clock { get; }

        /// <summary>
        /// 标识符。
        /// </summary>
        public IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }


        /// <summary>
        /// 是否已创建。
        /// </summary>
        public bool IsCreated { get; protected set; }

        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        public bool IsInitialized { get; protected set; }


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => LoggerFactory.CreateLogger(GetType());
    }
}
