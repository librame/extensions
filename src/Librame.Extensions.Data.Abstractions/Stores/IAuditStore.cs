#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 审计存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    public interface IAuditStore<TAccessor, TAudit> : IStore<TAccessor>
        where TAccessor : IAccessor
        where TAudit : class
    {
        /// <summary>
        /// 异步获取审计列表。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="orderedFactory">给定的排序工厂方法。</param>
        /// <param name="queryFactory">给定的查询工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{TAudit}"/> 的异步操作。</returns>
        Task<IPagingList<TAudit>> GetAuditsAsync(int index, int size,
            Func<IQueryable<TAudit>, IOrderedQueryable<TAudit>> orderedFactory,
            Func<IQueryable<TAudit>, IQueryable<TAudit>> queryFactory = null,
            CancellationToken cancellationToken = default);
    }
}
