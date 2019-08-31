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
    /// 存储初始化器基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
    public class StoreInitializerBase<TAccessor, TIdentifier> : StoreInitializerBase<TAccessor>, IStoreInitializer<TAccessor, TIdentifier>
        where TAccessor : DbContextAccessor
        where TIdentifier : IStoreIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="StoreInitializerBase{TAccessor, TIdentifier}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializerBase(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            // Cast
            Identifier = identifier.CastTo<IStoreIdentifier, TIdentifier>(nameof(identifier));
        }

        /// <summary>
        /// 构造一个 <see cref="StoreInitializerBase{TAccessor, TIdentifier}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializerBase(TIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            // Override
            Identifier = identifier;
        }


        /// <summary>
        /// 覆盖标识符接口实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        public new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 存储初始化器基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class StoreInitializerBase<TAccessor> : IStoreInitializer<TAccessor>
        where TAccessor : DbContextAccessor
    {
        private readonly ILoggerFactory _loggerFactory;


        /// <summary>
        /// 构造一个 <see cref="StoreInitializerBase{TAccessor}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializerBase(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
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
        public bool IsInitialized { get; private set; }


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => _loggerFactory.CreateLogger<StoreInitializerBase<TAccessor>>();


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        public void Initialize(IStoreHub<TAccessor> stores)
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
        protected virtual void InitializeCore(IStoreHub<TAccessor> stores)
        {
            InitializeTenants(stores);
        }

        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected virtual void InitializeTenants(IStoreHub<TAccessor> stores)
        {
            // 初始化默认租户
            var defaultTenant = stores.Accessor.BuilderOptions.Tenants.Default;
            if (defaultTenant.IsNotNull()
                && !stores.ContainTenantAsync(defaultTenant.Name, defaultTenant.Host).Result)
            {
                Tenant tenant;

                // 添加默认租户到数据库
                if (defaultTenant is Tenant _tenant)
                {
                    tenant = _tenant;
                }
                else
                {
                    tenant = new Tenant();
                    defaultTenant.EnsurePopulate(tenant);
                }

                tenant.Id = Identifier.GetTenantIdAsync().Result;

                stores.TryCreateAsync(default, tenant).Wait();
                Logger.LogTrace($"Add default tenant (name={tenant.Name}, host={tenant.Host}) to database.");
            }
        }

    }
}
