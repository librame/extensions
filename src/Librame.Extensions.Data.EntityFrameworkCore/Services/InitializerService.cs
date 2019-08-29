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
    /// 初始化器服务。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TIdentifier">指定的标识符服务类型。</typeparam>
    public class InitializerService<TAccessor, TIdentifier> : InitializerService<TAccessor>, IInitializerService<TAccessor, TIdentifier>
        where TAccessor : DbContextAccessor
        where TIdentifier : IIdentifierService
    {
        /// <summary>
        /// 构造一个 <see cref="InitializerService{TAccessor, TIdentifier}"/>（可用于容器构造）。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IIdentifierService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InitializerService(IIdentifierService identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            // Cast
            Identifier = identifier.CastTo<IIdentifierService, TIdentifier>(nameof(identifier));
        }

        /// <summary>
        /// 构造一个 <see cref="InitializerService{TAccessor, TIdentifier}"/>（可用于手动构造）。
        /// </summary>
        /// <param name="identifier">给定的 <typeparamref name="TIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected InitializerService(TIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            // Override
            Identifier = identifier;
        }


        /// <summary>
        /// 标识符服务。
        /// </summary>
        /// <value>返回 <typeparamref name="TIdentifier"/>。</value>
        public new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 初始化器服务。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class InitializerService<TAccessor> : AbstractService, IInitializerService<TAccessor>
        where TAccessor : DbContextAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="InitializerService{TAccessor}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IIdentifierService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InitializerService(IIdentifierService identifier, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Identifier = identifier.NotNull(nameof(identifier));
        }


        /// <summary>
        /// 标识符服务。
        /// </summary>
        /// <value>返回 <see cref="IIdentifierService"/>。</value>
        public IIdentifierService Identifier { get; }


        /// <summary>
        /// 初始化服务。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        public virtual void InitializeService(IStoreHub<TAccessor> stores)
        {
            stores.NotNull(nameof(stores));

            // 提前切换为写入数据库，以便支持数据重复验证
            stores.Accessor.TryChangeDbConnection(tenant => tenant.WritingConnectionString);

            InitializeStores(stores);

            // 已重写，保存时会自行尝试还原为读取数据库
            stores.Accessor.SaveChanges();
        }

        /// <summary>
        /// 初始化存储集合。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected virtual void InitializeStores(IStoreHub<TAccessor> stores)
        {
            InitializeTenants(stores);
        }

        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected virtual void InitializeTenants(IStoreHub<TAccessor> stores)
        {
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


        /// <summary>
        /// 处置对象。
        /// </summary>
        public override void Dispose()
        {
            Identifier.Dispose();

            base.Dispose();
        }

    }
}
