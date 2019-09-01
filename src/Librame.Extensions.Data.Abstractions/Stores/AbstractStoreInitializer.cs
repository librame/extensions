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
    public abstract class AbstractStoreInitializer<TAccessor> : IStoreInitializer<TAccessor>
        where TAccessor : IAccessor
    {
        private readonly ILoggerFactory _loggerFactory;


        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer{TAccessor}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStoreInitializer(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory.NotNull(nameof(loggerFactory));

            Identifier = identifier.NotNull(nameof(identifier));
        }


        /// <summary>
        /// 标识符。
        /// </summary>
        public IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        public bool IsInitialized { get; protected set; }


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => _loggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        public virtual void Initialize(IStoreHub<TAccessor> stores)
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

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected abstract void InitializeCore(IStoreHub<TAccessor> stores);
    }
}
