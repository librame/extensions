#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Services
{
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 多租户访问器服务接口。
    /// </summary>
    public interface IMultiTenancyAccessorService : IAccessorService
    {
        /// <summary>
        /// 获取当前租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        ITenant GetCurrentTenant(IAccessor accessor);

        /// <summary>
        /// 异步获取当前租户。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="ITenant"/> 的异步操作。</returns>
        Task<ITenant> GetCurrentTenantAsync(IAccessor accessor, CancellationToken cancellationToken = default);
    }
}
