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
    /// 数据迁移存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    public interface IDataMigrationStore<out TAccessor, TMigration> : IStore<TAccessor>
        where TAccessor : IAccessor
        where TMigration : class
    {
        /// <summary>
        /// 迁移查询。
        /// </summary>
        IQueryable<TMigration> Migrations { get; }


        /// <summary>
        /// 异步查找迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TMigration"/> 的异步操作。</returns>
        ValueTask<TMigration> FindMigrationAsync(CancellationToken cancellationToken, params object[] keyValues);

        /// <summary>
        /// 异步获取分页迁移集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TMigration}"/> 的异步操作。</returns>
        ValueTask<IPageable<TMigration>> GetPagingMigrationsAsync(int index, int size,
            Func<IQueryable<TMigration>, IQueryable<TMigration>> queryFactory = null,
            CancellationToken cancellationToken = default);
    }
}
