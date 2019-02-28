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
    using Builders;

    /// <summary>
    /// 抽象存储。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractStore<TDbContext, TBuilderOptions> : AbstractStore<TDbContext, Tenant, TBuilderOptions>, IStore<TBuilderOptions>
        where TDbContext : IDbContext<Tenant, TBuilderOptions>
        where TBuilderOptions : DataBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore{TDbContext, TTenant, TBuilderOptions}"/> 实例。
        /// </summary>
        /// <param name="dbContext">给定的数据库上下文服务实例。</param>
        public AbstractStore(TDbContext dbContext)
            : base(dbContext)
        {
        }


        /// <summary>
        /// 获取租户分页列表。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{Tenant}"/> 的异步操作。</returns>
        public virtual Task<IPagingList<Tenant>> GetTenantsAsync(int index, int size, CancellationToken cancellationToken = default)
        {
            return DbContext.Tenants.AsPagingByIndexAsync(q => q.OrderByDescending(a => a.Id), index, size, cancellationToken);
        }
    }

    /// <summary>
    /// 抽象存储。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractStore<TDbContext, TTenant, TBuilderOptions> : AbstractStore<TDbContext>, IStore<TTenant, TBuilderOptions>
        where TDbContext : IDbContext<TTenant, TBuilderOptions>
        where TTenant : class, ITenant
        where TBuilderOptions : DataBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore{TDbContext, TTenant, TBuilderOptions}"/> 实例。
        /// </summary>
        /// <param name="dbContext">给定的数据库上下文服务实例。</param>
        public AbstractStore(TDbContext dbContext)
            : base(dbContext)
        {
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        public TBuilderOptions BuilderOptions => DbContext.BuilderOptions;


        /// <summary>
        /// 异步获取租户。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="Tenant"/> 的异步操作。</returns>
        public virtual Task<TTenant> GetTenantAsync(object id, CancellationToken cancellationToken = default)
        {
            return DbContext.Tenants.FindAsync(new object[] { id }, cancellationToken);
        }
    }


    /// <summary>
    /// 抽象存储。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    public abstract class AbstractStore<TDbContext> : AbstractDisposable, IStore
        where TDbContext : IDbContext
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore{TDbContext}"/> 实例。
        /// </summary>
        /// <param name="dbContext">给定的数据库上下文服务实例。</param>
        public AbstractStore(TDbContext dbContext)
        {
            DbContext = dbContext.NotDefault(nameof(dbContext));
        }


        /// <summary>
        /// 数据库上下文服务。
        /// </summary>
        public TDbContext DbContext { get; }

        /// <summary>
        /// 数据库提供程序。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IDbProvider"/>。
        /// </value>
        public IDbProvider DbProvider => DbContext;


        /// <summary>
        /// 异步获取审计。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="Audit"/> 的异步操作。</returns>
        public virtual Task<Audit> GetAuditAsync(int id, CancellationToken cancellationToken = default)
        {
            return DbContext.Audits.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// 异步获取审计分页列表。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{Audit}"/> 的异步操作。</returns>
        public virtual Task<IPagingList<Audit>> GetAuditsAsync(int index, int size, CancellationToken cancellationToken = default)
        {
            return DbContext.Audits.AsPagingByIndexAsync(q => q.OrderByDescending(a => a.Id), index, size, cancellationToken);
        }
    }
}
