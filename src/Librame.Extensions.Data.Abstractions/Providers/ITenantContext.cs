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
    /// 租户上下文接口。
    /// </summary>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface ITenantContext<TTenant>
        where TTenant : class, ITenant
    {
        /// <summary>
        /// 获取租户。
        /// </summary>
        /// <param name="tenants">给定的 <see cref="IQueryable{TTenant}"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <returns>返回 <see cref="Tenant"/>。</returns>
        ITenant GetTenant(IQueryable<TTenant> tenants, DataBuilderOptions builderOptions);
    }
}
