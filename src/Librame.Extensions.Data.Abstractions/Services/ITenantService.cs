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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 租户服务接口。
    /// </summary>
    public interface ITenantService : IService
    {
        /// <summary>
        /// 异步获取租户。
        /// </summary>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IQueryable{TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="ITenant"/> 的异步操作。</returns>
        Task<ITenant> GetTenantAsync<TTenant>(IQueryable<TTenant> queryable, CancellationToken cancellationToken = default)
            where TTenant : ITenant;
    }
}
