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
    /// 初始化器服务基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class InitializerServiceBase<TAccessor> : AbstractService, IInitializerService<TAccessor>
        where TAccessor : DbContextAccessor
    {
        private readonly IIdentifierService _identifierService;


        /// <summary>
        /// 构造一个 <see cref="InitializerServiceBase{TAccessor}"/> 实例。
        /// </summary>
        /// <param name="identifierService">给定的 <see cref="IIdentifierService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InitializerServiceBase(IIdentifierService identifierService, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _identifierService = identifierService.NotNull(nameof(identifierService));
        }


        /// <summary>
        /// 初始化数据。
        /// </summary>
        /// <param name="storeHub">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        public virtual void Initialize(IStoreHub<TAccessor> storeHub)
        {
            storeHub.NotNull(nameof(storeHub));

            InitializeTenants(storeHub);
        }

        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        /// <param name="storeHubBase">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected virtual void InitializeTenants(IStoreHub<TAccessor> storeHubBase)
        {
            var defaultTenant = storeHubBase.Accessor.BuilderOptions.DefaultTenant;
            if (!storeHubBase.ContainTenantAsync(defaultTenant.Name, defaultTenant.Host).Result)
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

                tenant.Id = _identifierService.GetTenantIdAsync().Result;

                storeHubBase.TryCreateAsync(default, tenant).Wait();
                Logger.LogInformation($"Add default tenant to database.");
            }
        }

    }
}
