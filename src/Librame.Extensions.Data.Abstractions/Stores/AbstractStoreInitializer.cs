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
    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public abstract class AbstractStoreInitializer<TAccessor> : AbstractStoreInitializer, IStoreInitializer<TAccessor>
        where TAccessor : IAccessor
    {
        private static byte[] _locker = new byte[0];


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
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        public virtual void Initialize(IStoreHub<TAccessor> stores)
        {
            lock (_locker)
            {
                if (IsInitialized)
                    return;

                stores.NotNull(nameof(stores));

                // 提前切换为写入数据库，以便支持数据重复验证
                stores.Accessor.ToggleTenant(tenant => tenant.WritingConnectionString);

                InitializeCore(stores);

                // 已重写，保存时会自行尝试还原为读取数据库
                stores.Accessor.SaveChanges();

                IsInitialized = true;
            }
        }

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected abstract void InitializeCore(IStoreHub<TAccessor> stores);


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TTable">指定的实体表类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor, TAudit, TTable, TTenant}"/>。</param>
        public virtual void Initialize<TAudit, TTable, TTenant>(IStoreHub<TAccessor, TAudit, TTable, TTenant> stores)
            where TAudit : DataAudit
            where TTable : DataEntity
            where TTenant : DataTenant
        {
            lock (_locker)
            {
                if (IsInitialized)
                    return;

                stores.NotNull(nameof(stores));

                // 提前切换为写入数据库，以便支持数据重复验证
                stores.Accessor.ToggleTenant(tenant => tenant.WritingConnectionString);

                InitializeCore(stores);

                // 已重写，保存时会自行尝试还原为读取数据库
                stores.Accessor.SaveChanges();

                IsInitialized = true;
            }
        }

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TTable">指定的实体表类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor, TAudit, TTable, TTenant}"/>。</param>
        protected abstract void InitializeCore<TAudit, TTable, TTenant>(IStoreHub<TAccessor, TAudit, TTable, TTenant> stores)
            where TAudit : DataAudit
            where TTable : DataEntity
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
