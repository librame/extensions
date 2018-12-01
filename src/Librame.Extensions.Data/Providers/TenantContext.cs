#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;

namespace Librame.Extensions.Data
{
    using Builders;

    /// <summary>
    /// 租户上下文。
    /// </summary>
    public class TenantContext : ITenantContext
    {
        /// <summary>
        /// 获取租户。
        /// </summary>
        /// <param name="tenants">给定的 <see cref="IQueryable{Tenant}"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <returns>返回 <see cref="Tenant"/>。</returns>
        public Tenant GetTenant(IQueryable<Tenant> tenants, DataBuilderOptions builderOptions)
        {
            return builderOptions.LocalTenant;
        }

    }
}
