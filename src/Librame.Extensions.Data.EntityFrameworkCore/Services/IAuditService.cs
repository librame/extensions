#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 审计服务接口。
    /// </summary>
    public interface IAuditService : IService
    {
        /// <summary>
        /// 异步获取审计列表。
        /// </summary>
        /// <param name="changeEntities">给定的 <see cref="IList{EntityEntry}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="List{BaseAudit}"/> 的异步操作。</returns>
        Task<List<BaseAudit>> GetAuditsAsync(IList<EntityEntry> changeEntities,
            CancellationToken cancellationToken = default);
    }
}
